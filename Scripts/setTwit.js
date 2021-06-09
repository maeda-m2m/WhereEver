//↓をLiteralに貼り付けることで実行可能
/*
<p class="center">
	<script src="../Scripts/setTwit.js" type="text/javascript" charset="utf-8"></script>
	<button class="btn_loginlist" type="submit" onclick="location.href=javascript:twitText('任意の投稿する文字列')">tweet test</button>
</p>

   //Twetterボタン作成 test
   Label_Tweet.Text = "<p class=\"center\"><script src = \"../Scripts/setTwit.js\" type=\"text/javascript\" charset=\"utf-8\"></script>\r\f<button class=\"btn_loginlist\" type=\"button\" onclick=\"twitText('[m2m server]: tweet test.');\">tweet(test)</button></p>";

*/

//p1 = string s 投稿するテキスト
function twitText(s) {
		//宣言
		var url;
		//s = "投稿するテキスト";
		url = document.location.href;
		if (s != "") {
			if (s.length > 140) {
				//文字数制限
				alert("テキストが140文字を超えています！");
			} else {
				//投稿画面を開く
				alert("投稿画面を開きます。");
				url = "http://twitter.com/share?url=" + escape(url) + "&text=" + s;
				window.open(url, "_blank", "width=600,height=300");
			}
		}
}