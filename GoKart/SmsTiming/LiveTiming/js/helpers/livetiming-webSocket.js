function onError(evt) {
    noRaces(slerror);
}

function onMessage(evt) {
    if (evt.data != '{}') {
        onJSONReceived(evt.data);
    } else {
        noRaces(slnoheat);
    }
}

function onOpen(evt) {
    doSend("START " + eventData["liveServerKey"]);
}

function onClose(evt) {
    noRaces(slunavailable);
    setTimeout(function () {
        startWebSocket();
    }, 5000)
}