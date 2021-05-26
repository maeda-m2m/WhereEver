//参考：https://nodejs.org/en/ ここからWebSocketをダウンロード

//コマンドプロンプトかターミナルで
//	npm install ws

//シグナリングサーバー起動
//  node signaling.js

//※現在このjsは未使用です

// ----- use socket.io ---
let port = 56803;
let socket = io.connect('http://localhost:' + port + '/');
let room = getRoomName();
socket.on('connect', function (evt) {
    socket.emit('enter', room);
});
socket.on('message', function (message) {
    let fromId = message.from;

    if (message.type === 'offer') {
        // -- got offer ---
        let offer = new RTCSessionDescription(message);
        setOffer(fromId, offer);
    }
    else if (message.type === 'answer') {
        // --- got answer ---
        let answer = new RTCSessionDescription(message);
        setAnswer(fromId, answer);
    }
    else if (message.type === 'candidate') {
        // --- got ICE candidate ---
        let candidate = new RTCIceCandidate(message.ice);
        addIceCandidate(fromId, candidate);
    }
    else if (message.type === 'call me') {
        if (!isReadyToConnect()) {
            console.log('Not ready to connect, so ignore');
            return;
        }
        else if (!canConnectMore()) {
            console.warn('TOO MANY connections, so ignore');
        }

        if (isConnectedWith(fromId)) {
            // already connnected, so skip
            console.log('already connected, so ignore');
        }
        else {
            // connect new party
            makeOffer(fromId);
        }
    }
    else if (message.type === 'bye') {
        if (isConnectedWith(fromId)) {
            stopConnection(fromId);
        }
    }
});
socket.on('user disconnected', function (evt) {
    let id = evt.id;
    if (isConnectedWith(id)) {
        stopConnection(id);
    }
});

// --- broadcast message to all members in room
function emitRoom(msg) {
    socket.emit('message', msg);
}

function emitTo(id, msg) {
    msg.sendto = id;
    socket.emit('message', msg);
}


/*
"use strict";

let WebSocketServer = require('ws').Server;
let port = 3001;
let wsServer = new WebSocketServer({ port: port });
console.log('websocket server start. port=' + port);

wsServer.on('connection', function (ws) {
    console.log('-- websocket connected --');
    ws.on('message', function (message) {
        wsServer.clients.forEach(function each(client) {
            if (isSame(ws, client)) {
                console.log('- skip sender -');
            }
            else {
                client.send(message);
            }
        });
    });
});

function isSame(ws1, ws2) {
    // -- compare object --
    return (ws1 === ws2);
}
*/