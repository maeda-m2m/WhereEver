//現在実行中のスクリプトを取得
let element = document.currentScript;

// ページの最終更新年月日を取得
modified = new Date(document.lastModified);

// 最終更新年月日をそれぞれ格納
year = modified.getFullYear();
month = modified.getMonth() + 1;
date = modified.getDate();
hour = modified.getHours();
minute = modified.getMinutes();
second = modified.getSeconds();

element.insertAdjacentHTML('beforebegin', '表示更新:' + year + "年" + month + "月" + date + "日 " + hour + ':' + minute + ':' + second);