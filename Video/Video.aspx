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


           <p class="index1">◆Hand Signaling 2016 改造版</p>
           <p>※Edgeでは動作しません。"https://～"か"http://localhost/～"で使用できます。事前にカメラを繋いでおいて下さい。</p>

           <p>PCにWebカメラを接続してからFirefox 47 またはChrome 51でアクセスしてみてください。</p>
           <p>通信するためにAさんとBさんで計2つのページを開く必要があります。</p>
           <p>参考：https://html5experts.jp/mganeko/19814/</p>

           <p>事前準備</p>
           <p>コマンドプロンプトかターミナルで</p>
           <p>	npm install ws</p>

            <p>シグナリングサーバー起動</p>
            <p>  node signaling.js</p>


           <ol>
           <li type="circle">カメラ起動方法：AさんとBさんの両方で[ビデオ撮影開始]をクリック（カメラ起動）</li>
           <li type="circle">接続方法：お手元の[SDP接続開始]をクリック（SDP接続）</li>
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
    let wsUrl = 'ws://localhost:3001/'; //localhosutはお使いの環境に合わせて下さい。
    let ws = new WebSocket(wsUrl);
    ws.onopen = function (evt) {
        console.log('ws open()');
    };
    ws.onerror = function (err) {
        console.error('ws onerror() ERR:', err);
    };

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
    };

    // ---------------------- media handling ----------------------- 
    // start local video
    function startVideo() {
        getDeviceStream({ video: true, audio: false })
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

    function sendSdp(sessionDescription) {
        console.log('---sending sdp ---');

        textForSendSdp.value = sessionDescription.sdp;
        /*--- テキストエリアをハイライトするのを止める
        textForSendSdp.focus();
        textForSendSdp.select();
        ----*/

        // --- シグナリングサーバーに送る ---
        let message = JSON.stringify(sessionDescription);
        console.log('sending SDP=' + message);
        ws.send(message);
    }
    // ---------------------- connection handling -----------------------
    function prepareNewConnection() {
        let pc_config = { "iceServers": [] };
        let peer = new RTCPeerConnection(pc_config);

        // --- on get remote stream ---
        if ('ontrack' in peer) {
            peer.ontrack = function (event) {
                console.log('-- peer.ontrack()');
                let stream = event.streams[0];
                playVideo(remoteVideo, stream);
            };
        }
        else {
            peer.onaddstream = function (event) {
                console.log('-- peer.onaddstream()');
                let stream = event.stream;
                playVideo(remoteVideo, stream);
            };
        }

        // --- on get local ICE candidate
        peer.onicecandidate = function (evt) {
            if (evt.candidate) {
                console.log(evt.candidate);

                // Trickle ICE の場合は、ICE candidateを相手に送る
                // Vanilla ICE の場合には、何もしない
            } else {
                console.log('empty ice event');

                // Trickle ICE の場合は、何もしない
                // Vanilla ICE の場合には、ICE candidateを含んだSDPを相手に送る
                sendSdp(peer.localDescription);
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

    function makeOffer() {
        peerConnection = prepareNewConnection();
        peerConnection.createOffer()
            .then(function (sessionDescription) {
                console.log('createOffer() succsess in promise');
                return peerConnection.setLocalDescription(sessionDescription);
            }).then(function () {
                console.log('setLocalDescription() succsess in promise');

                // -- Trickle ICE の場合は、初期SDPを相手に送る -- 
                // -- Vanilla ICE の場合には、まだSDPは送らない --
                //sendSdp(peerConnection.localDescription);
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
                // -- Vanilla ICE の場合には、まだSDPは送らない --
                //sendSdp(peerConnection.localDescription);
            }).catch(function (err) {
                console.error(err);
            });
    }

    function setAnswer(sessionDescription) {
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
