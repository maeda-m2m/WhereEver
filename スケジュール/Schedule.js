"use strict";

$(document).ready(function () {
    $.ajax({ url: "data.json", dataType: "json" })
        .done(function (data) {
            console.log(data);
        })
        .fail(function () {
            window.alert("読み込みエラー");
        });
});



function A() {
    return window.confirm('実行しますか？');


}

const now = new Date();
const year = now.getFullYear();
const month = now.getMonth();
const date = now.getDate();
const hour = now.getHours();
const min = now.getMinutes();

const output = `${year}年${month + 1}月${date}日 ${hour}時${min}分`;
document.getElementById('time').textContent = output;


recalc();


function recalc() {
    const now = new Date();
    const year = now.getFullYear();
    const month = now.getMonth();
    const date = now.getDate();
    const hour = now.getHours();
    const min = now.getMinutes();
    const sec = now.getSeconds();

    const output = `${year}年${month + 1}月${date}日 ${hour}時${min}分${sec}秒`;
    document.getElementById('time').textContent = output;
    refresh();
}

function refresh() {
    setTimeout(recalc, 1000);
}

$(document).ready(function () {
    $("#Image1").on("click", function () {
        location.href = "test1.aspx";

    });
});

/*
 未完成、いいえを選択してもデータが削除される。
 */



