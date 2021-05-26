<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Video.aspx.cs" Inherits="WhereEver.Video.WebForm1" %>
<%@ Register Src="~/MenuControl.ascx" TagName="c_menu" TagPrefix="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="Content-Style-Type" content="text/css" />
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../MenuControl.css" />
    <link rel="stylesheet" type="text/css" href="Video.css" />

    <title>P2PVideoChat</title>
    <script src="http://localhost:3002/socket.io.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td id="menu">
                        <menu:c_menu id="m" runat="server"></menu:c_menu>
                    </td>
                </tr>
            </table>
       </div>


       <div id="Wrap">

           <span class="hr"></span>

           <%-- 参考：https://html5experts.jp/series/webrtc2016/ --%>
           <%-- 参考：https://qiita.com/lighthouse/items/34bb8ccb6149bbfae427 --%>


           <p class="index1">◆リモート通話機能（Hand Signaling 2016 改造版）</p>
           <p>※Edgeでは動作しません。"https://～"か"http://localhost/～"で使用できます。事前にカメラを繋いでおいて下さい。</p>

           <hr />

           <p>PCにWebカメラを接続してからFirefox 47 またはChrome 51でアクセスしてみてください。</p>
           <p>ポート番号は手動で変更して下さい。現在の設定は 56803 です。</p>
           <p>F12キーを押してエラーがないか確認して下さい。</p>
           <p>参考：https://html5experts.jp/mganeko/19814/</p>

           <hr />

           <p>～事前準備～</p>
           <p>コマンドプロンプトかターミナルでsocket.ioモジュールのインストール</p>
           <p>  npm install socket.io</p>

           <p>シグナリングサーバー起動（↓でダメなら node Video/signaling_room.js）</p>
           <p>  node signaling_room.js</p>

           <hr />

           <ol>
           <li type="circle">カメラ起動方法：通信者は[ビデオ撮影開始]をクリック（カメラ起動）</li>
           <li type="circle">接続方法：[SDP接続開始]をクリック（SDP接続）</li>
           <li type="circle">送信または返信方法：自動</li>
           <li type="circle">受信方法：自動</li>
           </ol>

    <div id="Video">
    <div class="center">

  <button type="button" onclick="startVideo();" class="btn-flat-border">ビデオ撮影開始</button>
  <button type="button" onclick="stopVideo();" class="btn-flat-border">ビデオ撮影停止</button>
  &nbsp;
  <button type="button" onclick="connect();" class="btn-flat-border">SDP接続開始</button>
  <button type="button" onclick="hangUp();" class="btn-flat-border">SDP接続切断</button> 
  <div>
    <p>local　　　　　　　　　　　　　　　　remote</p>
    <video id="local_video" autoplay style="width: 300px; height: 300px; border: 1px solid white;"></video>
    <video id="remote_video" autoplay style="width: 300px; height: 300px; border: 1px solid white;"></video>
  </div>
  <p>SDP to send:<br />
    <textarea id="text_for_send_sdp" rows="8" cols="100" readonly="readonly">SDP to send</textarea>
  </p>
  <p>SDP to receive:&nbsp;<br />
    <%-- <button type="button" onclick="onSdpText();" class="btn-flat-border">SDP遠隔受信</button><br /> --%>
    <textarea id="text_for_receive_sdp" rows="8" cols="100"></textarea>
  </p>

    </div>
    </div> <%-- Video --%>



        </div> <%-- Wrap --%>
    </form>
</body>


<script type="text/javascript">
    let localVideo = document.getElementById('local_video');
    let remoteVideo = document.getElementById('remote_video');
    let localStream = null;
    let peerConnection = null;
    let textForSendSdp = document.getElementById('text_for_send_sdp');
    let textToReceiveSdp = document.getElementById('text_for_receive_sdp');

    // --- prefix -----
    navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia ||
        navigator.mozGetUserMedia || navigator.msGetUserMedia;
    RTCPeerConnection = window.RTCPeerConnection || window.webkitRTCPeerConnection || window.mozRTCPeerConnection;
    RTCSessionDescription = window.RTCSessionDescription || window.webkitRTCSessionDescription || window.mozRTCSessionDescription;


    // ---------------------- media handling ----------------------- 

/*  単体接続だけのときはWebSocketを直接用いてコメントアウトを解除すればよい
    let wsUrl = 'ws://localhost:3001/'; //localhostはお使いの環境に合わせて下さい。
    let ws = new WebSocket(wsUrl);
    ws.onopen = function (evt) {
        console.log('ws open()');
    };
    ws.onerror = function (err) {
        console.error('ws onerror() ERR:', err);
    };
*/

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
    //--------↑ここに設置で合っているか？--------



    //videoタグの管理JavaScript
    //---init---
    let remoteVideos = [];
    let container = document.getElementById('container');

    // --- video elements ---
    function addRemoteVideoElement(id = 0) {
        let video = createVideoElement('remote_video_' + id);
        remoteVideos[id] = video;
        return video;
    }

    function getRemoteVideoElement(id = 0) {
        let video = remoteVideos[id];
        return video;
    }

    function deleteRemoteVideoElement(id = 0) {
        removeVideoElement('remote_video_' + id);
        delete remoteVideos[id];
    }

    function createVideoElement(elementId) {
        let video = document.createElement('video');
        video.width = '160';
        video.height = '120';
        video.id = elementId;

        video.style.border = 'solid black 1px';
        video.style.margin = '2px';

        container.appendChild(video);

        return video;
    }

    function removeVideoElement(elementId) {
        let video = document.getElementById(elementId);
        container.removeChild(video);
        return video;
    }



    //------

    function attachVideo(id = 0, stream) {
        let video = addRemoteVideoElement(id);
        playVideo(video, stream);
        video.volume = 1.0;
    }

    function detachVideo(id = 0) {
        let video = getRemoteVideoElement(id);
        pauseVideo(video);
        deleteRemoteVideoElement(id);
    }

    function isRemoteVideoAttached(id = 0) {
        if (remoteVideos[id]) {
            return true;
        }
        else {
            return false;
        }
    }

    //複数PeerConnectionの管理JavaScript
    // ---- for multi party -----
    let peerConnections = [];
    const MAX_CONNECTION_COUNT = 3;

    // --- RTCPeerConnections ---
    function getConnectionCount() {
        return peerConnections.length;
    }

    function canConnectMore() {
        return (getConnectionCount() < MAX_CONNECTION_COUNT);
    }

    function isConnectedWith(id = 0) {
        if (peerConnections[id]) {
            return true;
        }
        else {
            return false;
        }
    }

    function addConnection(id = 0, peer) {
        peerConnections[id] = peer;
    }

    function getConnection(id = 0) {
        let peer = peerConnections[id];
        return peer;
    }

    function deleteConnection(id = 0) {
        delete peerConnections[id];
    }

    function stopConnection(id = 0) {
        detachVideo(id);

        if (isConnectedWith(id)) {
            let peer = getConnection(id);
            peer.close();
            deleteConnection(id);
        }
    }

    function stopAllConnection() {
        for (let id in peerConnections) {
            stopConnection(id);
        }
    }


    // ---------------------- Web Socket Message Receive ----------------------- 
    ws.onmessage = function (evt) {
        console.log('ws onmessage() data:', evt.data);
        let message = JSON.parse(evt.data);
        if (message.type === 'offer') {
            // -- got offer ---
            console.log('Received offer ...');
            textToReceiveSdp.value = message.sdp;
            let offer = new RTCSessionDescription(message);
            setOffer(offer);
        }
        else if (message.type === 'answer') {
            // --- got answer ---
            console.log('Received answer ...');
            textToReceiveSdp.value = message.sdp;
            let answer = new RTCSessionDescription(message);
            setAnswer(answer);
        }
        else if (message.type === 'candidate') { // <--- ここから追加
            // --- got ICE candidate ---
            console.log('Received ICE candidate ...');
            let candidate = new RTCIceCandidate(message.ice);
            console.log(candidate);
            addIceCandidate(candidate);
        }
    };

    function addIceCandidate(candidate) {
        if (peerConnection) {
            peerConnection.addIceCandidate(candidate);
        }
        else {
            console.error('PeerConnection not exist!');
            return;
        }
    }

    // ---------------------- media handling ----------------------- 
    // start local video
    function startVideo() {
        getDeviceStream({ video: true, audio: true })
            .then(function (stream) { // success
                localStream = stream;
                playVideo(localVideo, stream);
            }).catch(function (error) { // error
                console.error('getUserMedia error:', error);
                return;
            });
    }

    // stop local video
    function stopVideo() {
        pauseVideo(localVideo);
        stopLocalStream(localStream);
    }

    function stopLocalStream(stream) {
        let tracks = stream.getTracks();
        if (!tracks) {
            console.warn('NO tracks');
            return;
        }

        for (let track of tracks) {
            track.stop();
        }
    }

    function getDeviceStream(option) {
        if ('getUserMedia' in navigator.mediaDevices) {
            console.log('navigator.mediaDevices.getUserMadia');
            return navigator.mediaDevices.getUserMedia(option);
        }
        else {
            console.log('wrap navigator.getUserMadia with Promise');
            return new Promise(function (resolve, reject) {
                navigator.getUserMedia(option,
                    resolve,
                    reject
                );
            });
        }
    }

    function playVideo(element, stream) {
        if ('srcObject' in element) {
            element.srcObject = stream;
        }
        else {
            element.src = window.URL.createObjectURL(stream);
        }
        element.play();
        element.volume = 0;
    }

    function pauseVideo(element) {
        element.pause();
        if ('srcObject' in element) {
            element.srcObject = null;
        }
        else {
            if (element.src && (element.src !== '')) {
                window.URL.revokeObjectURL(element.src);
            }
            element.src = '';
        }
    }

    // ----- hand signaling ----
    function onSdpText() {
        let text = textToReceiveSdp.value;
        if (peerConnection) {
            console.log('Received answer text...');
            let answer = new RTCSessionDescription({
                type: 'answer',
                sdp: text,
            });
            setAnswer(answer);
        }
        else {
            console.log('Received offer text...');
            let offer = new RTCSessionDescription({
                type: 'offer',
                sdp: text,
            });
            setOffer(offer);
        }
        textToReceiveSdp.value = '';
    }

    function sendSdp(id = 0, sessionDescription) {     //複数人対応
        console.log('---sending sdp ---');

        let message = { type: sessionDescription.type, sdp: sessionDescription.sdp };
        emitTo(id, message);

        //textForSendSdp.value = sessionDescription.sdp;

        // --- シグナリングサーバーに送る ---
        /*
        let message = JSON.stringify(sessionDescription);
        console.log('sending SDP=' + message);
        ws.send(message);
        */
    }
    // ---------------------- connection handling -----------------------
    function prepareNewConnection(id = 0) {     //複数人用にidで設定できるようにしておく
        let pc_config = { "iceServers": [] };
        let peer = new RTCPeerConnection(pc_config);

        // --- on get remote stream ---
        if ('ontrack' in peer) {
            peer.ontrack = function (event) {
                console.log('-- peer.ontrack()');
                let stream = event.streams[0];
                if (isRemoteVideoAttached(id)) {
                    console.log('stream already attached, so ignore'); // <--- 同じ相手からの2回目以降のイベントは無視する
                }
                else {
                    //playVideo(remoteVideo, stream);
                    attachVideo(id, stream);
                }
            };
        }
        else {
            peer.onaddstream = function (event) {
                let stream = event.stream;
                console.log('-- peer.onaddstream() stream.id=' + stream.id);
                //playVideo(remoteVideo, stream);
                attachVideo(id, stream);
            };
        }

        // --- on get local ICE candidate
        peer.onicecandidate = function (evt) {
            if (evt.candidate) {
                console.log(evt.candidate);

                // Trickle ICE の場合は、ICE candidateを相手に送る
                sendIceCandidate(evt.candidate); // <--- ここを追加する

                // Vanilla ICE の場合には、何もしない
            } else {
                console.log('empty ice event');

                // Trickle ICE の場合は、何もしない

                // Vanilla ICE の場合には、ICE candidateを含んだSDPを相手に送る
                //sendSdp(peer.localDescription); // <-- ここをコメントアウトする
            }
        };


        // -- add local stream --
        if (localStream) {
            console.log('Adding local stream...');
            peer.addStream(localStream);
        }
        else {
            console.warn('no local stream, but continue.');
        }

        return peer;
    }

    function sendIceCandidate(candidate) {
        console.log('---sending ICE candidate ---');
        let obj = { type: 'candidate', ice: candidate };
        let message = JSON.stringify(obj);
        console.log('sending candidate=' + message);
        ws.send(message);
    }

    //SDPをすぐに送るJavaScript
    function makeOffer() {
        peerConnection = prepareNewConnection();
        peerConnection.createOffer()
            .then(function (sessionDescription) {
                console.log('createOffer() succsess in promise');
                return peerConnection.setLocalDescription(sessionDescription);
            }).then(function () {
                console.log('setLocalDescription() succsess in promise');

                // -- Trickle ICE の場合は、初期SDPを相手に送る -- 
                sendSdp(peerConnection.localDescription);　// <--- ここを加える

                // -- Vanilla ICE の場合には、まだSDPは送らない --
            }).catch(function (err) {
                console.error(err);
            });
    }

    function makeAnswer() {
        console.log('sending Answer. Creating remote session description...');
        if (!peerConnection) {
            console.error('peerConnection NOT exist!');
            return;
        }

        peerConnection.createAnswer()
            .then(function (sessionDescription) {
                console.log('createAnswer() succsess in promise');
                return peerConnection.setLocalDescription(sessionDescription);
            }).then(function () {
                console.log('setLocalDescription() succsess in promise');

                // -- Trickle ICE の場合は、初期SDPを相手に送る -- 
                sendSdp(peerConnection.localDescription);　// <--- ここを加える

                // -- Vanilla ICE の場合には、まだSDPは送らない --
            }).catch(function (err) {
                console.error(err);
            });
    }

    function makeOffer(id = 0) {    //複数人対応
        peerConnection = prepareNewConnection(id);
        addConnection(id, peerConnection);

        peerConnection.createOffer()
            .then(function (sessionDescription) {
                console.log('createOffer() succsess in promise');
                return peerConnection.setLocalDescription(sessionDescription);
            }).then(function () {
                console.log('setLocalDescription() succsess in promise');

                // -- Trickle ICE の場合は、初期SDPを相手に送る -- 
                sendSdp(id, peerConnection.localDescription);　// <--- ここを加える

                // -- Vanilla ICE の場合には、まだSDPは送らない --
            }).catch(function (err) {
                console.error(err);
            });
    }

    function setOffer(sessionDescription) {
        if (peerConnection) {
            console.error('peerConnection alreay exist!');
        }
        peerConnection = prepareNewConnection();
        peerConnection.setRemoteDescription(sessionDescription)
            .then(function () {
                console.log('setRemoteDescription(offer) succsess in promise');
                makeAnswer();
            }).catch(function (err) {
                console.error('setRemoteDescription(offer) ERROR: ', err);
            });
    }

    function makeAnswer() {
        console.log('sending Answer. Creating remote session description...');
        if (!peerConnection) {
            console.error('peerConnection NOT exist!');
            return;
        }

        peerConnection.createAnswer()
            .then(function (sessionDescription) {
                console.log('createAnswer() succsess in promise');
                return peerConnection.setLocalDescription(sessionDescription);
            }).then(function () {
                console.log('setLocalDescription() succsess in promise');

                // -- Trickle ICE の場合は、初期SDPを相手に送る -- 
                sendSdp(peerConnection.localDescription);　// <--- ここを加える

                // -- Vanilla ICE の場合には、まだSDPは送らない --
            }).catch(function (err) {
                console.error(err);
            });
    }

    function setAnswer(id = 0, sessionDescription) {        //複数人対応
        let peerConnection = getConnection(id);
        if (!peerConnection) {
            console.error('peerConnection NOT exist!');
            return;
        }

        peerConnection.setRemoteDescription(sessionDescription)
            .then(function () {
                console.log('setRemoteDescription(answer) succsess in promise');
            }).catch(function (err) {
                console.error('setRemoteDescription(answer) ERROR: ', err);
            });
    }

    // start PeerConnection
    function connect() {
        if (!peerConnection) {
            console.log('make Offer');
            makeOffer();
        }
        else {
            console.warn('peer already exist.');
        }
    }

    // close PeerConnection
    function hangUp() {
        if (peerConnection) {
            console.log('Hang up.');
            peerConnection.close();
            peerConnection = null;
            pauseVideo(remoteVideo);
        }
        else {
            console.warn('peer NOT exist.');
        }
    }

</script>




</html>
