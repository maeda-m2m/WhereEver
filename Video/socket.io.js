

// ----- use socket.io ---

//socket.ioサーバーに接続
let port = 56803;    //ポートはデフォルトで3002です。ご自分で選んでください。
let socket = io.connect('http://localhost:' + port + '/');
socket.on('connect', function (evt) {
    // 接続したときの処理
});

//ルーム入出処理
let room = getRoomName();
socket.on('connect', function (evt) {
    console.log('socket.io connected. enter room=' + room);
    socket.emit('enter', room);
});

// -- room名を取得 --
function getRoomName() { // たとえば、 URLに  ?roomname  とする
    let url = document.location.href;
    let args = url.split('?');
    if (args.length > 1) {
        let room = args[1];
        if (room != '') {
            return room;
        }
    }
    return '_testroom';
}