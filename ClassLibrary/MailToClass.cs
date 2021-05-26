using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using static System.Web.HttpUtility;
using System.Text;

namespace WhereEver.ClassLibrary
{
    public class MailToClass
    {
        //----------------------------------------------------------------------------------------------
        //メール送信
        // MailKit & MimeKit
        // 作成者：(C)Jeffrey Stedfast
        // MIT Licence
        // https://opensource.org/licenses/mit-license.php
        //
        //MIT License

        //        Copyright(C) 2013-2021 .NET Foundation and Contributors

        //Permission is hereby granted, free of charge, to any person obtaining a copy
        //of this software and associated documentation files(the "Software"), to deal
        //in the Software without restriction, including without limitation the rights
        //to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
        //copies of the Software, and to permit persons to whom the Software is
        //furnished to do so, subject to the following conditions:

        //The above copyright notice and this permission notice shall be included in
        //all copies or substantial portions of the Software.

        //THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
        //IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
        //FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
        //AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
        //LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
        //OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
        //THE SOFTWARE.
        //----------------------------------------------------------------------------------------------
        /// <summary>
        /// メールを送信します。
        /// 送信成功時はtrueを、送信失敗時はfalseを返します。
        /// </summary>
        /// <param name="email"></param>
        /// <param name="str">本文</param>
        /// <returns></returns>
        public static bool MailTo(string @email, string @cc, string @bcc, string @title, string @str, string @smtphost, int port, string @user, string @pass, int @usersmtp, bool annonimas)
        {

            //Trim
            @email = @email.Trim();
            @cc = @cc.Trim();
            @bcc = @bcc.Trim();

            if (@email == null || @email == "")
            {
                //mailの中身なし
                return false;
            }


            //--------------------------------------------------------------------------------------------
            //件名と本文の設定
            //--------------------------------------------------------------------------------------------
            //

            // MimeMessageを作り、宛先やタイトルなどを設定する
            MimeKit.MimeMessage message = new MimeKit.MimeMessage();

            //p1: 名前（一覧に表示）  p2: メールアドレス（架空でもOK）
            //匿名希望
            string fromname = "Annonimas";
            if (!annonimas)
            {
                fromname = HtmlEncode(SessionManager.User.M_User.name1.Trim());
            }
            message.From.Add(new MimeKit.MailboxAddress(@fromname, @"m2m-PortalServer@m2m-asp.com"));   //Test thnderbirdは暗号化されたパスワード認証

            // 左が@送り先名、右が@メールアドレス
            message.To.Add(new MimeKit.MailboxAddress(@email, @email));

            if (cc != null && cc != "")
            {
                message.Cc.Add(new MimeKit.MailboxAddress(@cc, @cc));
            }
            if (bcc != null && bcc != "")
            {
                message.Bcc.Add(new MimeKit.MailboxAddress(@bcc, @bcc));
            }


            //件名とタイトルを入力
            string @sub = @"NO_SUBJECT";
            if (@title != "" && @title != null)
            {
                @sub = @title;
            }
            message.Subject = @sub;


            //--------------------------------------------------------------------------------------------


            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb.Append(@"<html><body>");

            sb.Append(@"<h1>");
            sb.Append(@sub);
            sb.Append(@"</h1>");
            // 内容を入力
            sb.Append(@"<div><pre>");
            sb.Append(@str);
            sb.Append(@"</pre></div>");

            //\r\nで改行ができる。フォーマット方式の設定によってはhtml形式でも記述可能。

            //-------------------
            //シグネチャ
            //-------------------
            sb.Append(@"<hr />");
            sb.Append(@"<p>");
            sb.Append(@"送信日時：");
            sb.Append(DateTime.Now.ToString());
            sb.Append(@"</p>");
            //-------------------
            sb.Append(@"<p>");
            sb.Append(@"このメールはサーバーを媒介した送信あるいは自動送信です。返信することはできません。");
            sb.Append(@"</p>");
            //-------------------
            sb.Append(@"</body></html>");
            //-------------------
            string mes = sb.ToString();           
            //--------------------------------------------------------------------------------------------


            // 本文を入力
            MimeKit.TextPart textPart = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html);    //Plainなら平文 HtmlならHtml文
            textPart.Text = @mes;

            // MimeMessageを完成させる
            message.Body = textPart;

            //SMTPサーバーのセキュリティオプションに合わせる　設定しなくてもデフォルトでAuto
            //MailKit.Security.SecureSocketOptions mailSecOpt = MailKit.Security.SecureSocketOptions.Auto;    //None: なし Auto: SSL/TLS/なしを自動分類

            // SMTPサーバに接続してメールを送信する
            using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
            {
#if DEBUG
                // 開発用のSMTPサーバが暗号化に対応していないときは、次の行を追加する
                //client.ServerCertificateValidationCallback = (s, c, h, e) => true;
#endif

                try
                {
                    //SMTPサーバー ホスト名 mailSecOptはデフォルトでAutoのため省略可能
                    //client.Connect(@mail, port, mailSecOpt);
                    //--------------------------------------------------------------

                    if (usersmtp == 1)
                    {
                        client.Connect("mail.m2m-asp.com", 587);  //debug用
                    }
                    else if (usersmtp == 2)
                    {
                        client.Connect("192.168.2.156", 25);    //公開用（サーバーが異なるため、このホスト名だとつながらない？）
                    }
                    else if (usersmtp == 3)
                    {
                        client.Connect(@smtphost, port);    //手動
                    }
                    //--------------------------------------------------------------

                    if (usersmtp == 3)
                    {
                        //ユーザー認証　※SMTPサーバがユーザー認証を必要としない場合、client.Authenticateは不要
                        //いずれかが入力されていれば適用
                        if (user != "" || pass != "")
                        {
                            string userName = @user;
                            string password = @pass;
                            client.Authenticate(@userName, @password);
                        }
                    }

                    //SMTPサーバーにメッセージを送信
                    client.Send(@message);

                    //切断
                    client.Disconnect(true);
                }
                catch
                {
                    //送信失敗
                    return false;
                }
            }

            return true;
        }


    }
}