/* ドラッグアンドドロップシステム */
//  参考:
//  http://www.htmq.com/dnd/

/***** ドラッグ開始時の処理 *****/
function f_dragstart(event) {
    //ドラッグするデータのid名をDataTransferオブジェクトにセット
    event.dataTransfer.setData("text", event.target.id);
}

/***** ドラッグ要素がドロップ要素に重なっている間の処理 *****/
function f_dragover(event) {
    //dragoverイベントをキャンセルして、ドロップ先の要素がドロップを受け付けるようにする
    event.preventDefault();
}

/***** ドロップ時の処理 *****/

function f_drop(event) {
    //移動したアイテムの背景色書き換え
    const color = 'black'
    f_color_change(event, color)
}

function f_drop1(event) {
    //移動したアイテムの背景色書き換え
    const color = 'red'
    f_color_change(event, color)
}

function f_drop2(event) {
    //移動したアイテムの背景色書き換え
    const color = 'blue'
    f_color_change(event, color)
}

function f_drop3(event) {
    //移動したアイテムの背景色書き換え
    const color = 'green'
    f_color_change(event, color)
}

function f_color_change(event, color) {
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    //id名からドラッグされた要素を取得
    var drag_elm = document.getElementById(id_name);
    //ドロップ先にドラッグされた要素を追加
    event.currentTarget.appendChild(drag_elm);

    //移動したアイテムの背景色書き換え
    document.getElementById(id_name).style.backgroundColor = color;

    var c_str = id_name + ',' + color + ',';
    var h_item = document.getElementById('Hidden_Label_item');


    if (h_item.value.match(id_name + ',[a-z]*,')) { //正規表現は原則RegularEcpressionsを使う。/.*/と変数は結合できない。
        var regExp = new RegExp(id_name + ',[a-z]*,', "g");
        h_item.value = h_item.value.replace(regExp, '');

        //alert(h_item.value + " : " + id_name);
    }

    if (!h_item.value.match(c_str)) {
        h_item.value += c_str;
    }

    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    event.preventDefault();
}