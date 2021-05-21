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
        //----------------------------------------------------------------------------------------------
        /// <summary>
        /// メールを送信します。
        /// 送信成功時はtrueを、送信失敗時はfalseを返します。
        /// </summary>
        /// <param name="email"></param>
        /// <param name="str">本文</param>
        /// <returns></returns>
        public static bool MailTo(string email, string cc, string bcc, string str)
        {

            //Trim
            email = email.Trim();
            cc = cc.Trim();
            bcc = bcc.Trim();

            if (email == null || email == "")
            {
                //mailの中身なし
                return false;
            }


            //--------------------------------------------------------------------------------------------
            //件名と本文の設定
            //--------------------------------------------------------------------------------------------
            //
            String sub = "NO TITLE";
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            sb.Append(@"<html><body>");

            // タイトルを入力
            sub = @"m2mポータルからのお知らせ";
            sb.Append(@"<h1>m2mポータルからのお知らせです。</h1>");
            // 内容を入力
            sb.Append(@str);

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
            sb.Append(@"このメールはサーバーからの自動送信です。返信することはできません。");
            sb.Append(@"</p>");
            //-------------------
            sb.Append(@"</body></html>");
            //-------------------
            String mes = sb.ToString();
            //--------------------------------------------------------------------------------------------


            // MimeMessageを作り、宛先やタイトルなどを設定する
            MimeKit.MimeMessage message = new MimeKit.MimeMessage();

            //p1: 名前（一覧に表示）  p2: メールアドレス（架空でもOK）
            message.From.Add(new MimeKit.MailboxAddress(@"m2mPortal@m2m-asp.com", @"m2mPortal@m2m-asp.com"));   //Test thnderbirdは暗号化されたパスワード認証

            // 左が@送り先名、右が@メールアドレス
            message.To.Add(new MimeKit.MailboxAddress("テスト", @email));

            if (cc != null || cc != "")
            {
                message.Cc.Add(new MimeKit.MailboxAddress("テストCC", @cc));
            }
            if (bcc != null || bcc != "")
            {
                message.Bcc.Add(new MimeKit.MailboxAddress("テストBCC", @bcc));
            }


            //件名を入力
            message.Subject = @sub;

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
                    //await client.Connect(@mail, port, mailSecOpt);

                    client.Connect("mail.m2m-asp.com", 587);  //debug用
                    //client.Connect("192.168.2.156", 25);    //公開用

                    //ユーザー認証　※SMTPサーバがユーザー認証を必要としない場合、client.Authenticateは不要
                    //string userName = @"abcdef";
                    //string password = @"123456";
                    //client.Authenticate(@userName, @password);

                    //SMTPサーバーにメッセージを送信
                    client.Send(@message);

                    //切断
                    //await client.DisconnectAsync(true);
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