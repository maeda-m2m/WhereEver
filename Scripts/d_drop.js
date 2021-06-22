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
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    //id名からドラッグされた要素を取得
    var drag_elm = document.getElementById(id_name);
    //ドロップ先にドラッグされた要素を追加
    event.currentTarget.appendChild(drag_elm);

    //移動したアイテムの背景色書き換え
    document.getElementById(id_name).style.backgroundColor = 'transparent';

    //ドラッグしたアイテムのCSSを強制変更　下だと枠が変わってしまう
    //event.currentTarget.className = drag_elm;//テスト

    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    event.preventDefault();
}

function f_drop1(event) {
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    //id名からドラッグされた要素を取得
    var drag_elm = document.getElementById(id_name);
    //ドロップ先にドラッグされた要素を追加
    event.currentTarget.appendChild(drag_elm);

    //移動したアイテムの背景色書き換え
    document.getElementById(id_name).style.backgroundColor = 'red';
    document.getElementById('Hidden_Label_item').value = 'red';
    //ドラッグしたアイテムのCSSを強制変更　下だと枠が変わってしまう
    //event.currentTarget.className = drag_elm;//テスト

    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    event.preventDefault();
}

function f_drop2(event) {
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    //id名からドラッグされた要素を取得
    var drag_elm = document.getElementById(id_name);
    //ドロップ先にドラッグされた要素を追加
    event.currentTarget.appendChild(drag_elm);

    //移動したアイテムの背景色書き換え
    document.getElementById(id_name).style.backgroundColor = 'blue';

    //ドラッグしたアイテムのCSSを強制変更　下だと枠が変わってしまう
    //event.currentTarget.className = drag_elm;//テスト

    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    event.preventDefault();
}

function f_drop3(event) {
    //ドラッグされたデータのid名をDataTransferオブジェクトから取得
    var id_name = event.dataTransfer.getData("text");
    //id名からドラッグされた要素を取得
    var drag_elm = document.getElementById(id_name);
    //ドロップ先にドラッグされた要素を追加
    event.currentTarget.appendChild(drag_elm);

    //移動したアイテムの背景色書き換え
    document.getElementById(id_name).style.backgroundColor = 'green';

    //ドラッグしたアイテムのCSSを強制変更　下だと枠が変わってしまう
    //event.currentTarget.className = drag_elm;//テスト

    //エラー回避のため、ドロップ処理の最後にdropイベントをキャンセルしておく
    event.preventDefault();
}