var oldHeat;
var start = "00:00:01"
var M3 = [];
var M4 = [];
var lapsD = [];
var newRec = [];
var sl;
var ml;
var hl;
var secP;
var sPP;
var output;
var startPP = "0";
var data = 0;
var prewQ = 0;
var lapsLeft = 0;
var paused = false;
var stop = false;
var started = false;
var clockStarted = false;
var lapsMode = false;
var upd = true;
var available = true;
var finished = false;
var timeLeft;
var countingUp = false;
var firstInit = true;

for (var i = 0; i < 50; i++) {
    M4[i] = 0;
    M3[i] = 0;
    lapsD[i] = 0;
    newRec[i] = 0;
}

function getUrlParams() {
    var styleId = null,
        resourceId = null,
        locale = null,
        customCSS = null,
        nodeId = null;

    locale = getLocale();

    styleId = url('?styleId');
    resourceId = url('?resourceId');
    customCSS = url('?customCSS');
    nodeId = url('?nodeId');

    var params = { locale: locale, styleId: styleId, resourceId: resourceId, customCSS: customCSS, nodeId: nodeId };
    return params;
}

function initializeLiveTiming(settings, connectionInfo) {
    eventData = settings;
    eventConnection = connectionInfo;
    translations(eventData);
    if (checkBrowser()) {
        init();
    } else {
        initColors();      
        var infoDiv = document.getElementById("info_races");
        infoDiv.innerHTML = "<a href='http://www.browserchoice.eu/BrowserChoice/browserchoice_en.htm' target='_blank'>" + slupdatebrowser + "</a>";
    }
}

function init() {
    onTimer();
    onTimerP();
    if ("WebSocket" in window) {
        startWebSocket();
    } else {
        if (/msie [1-8]./.test(navigator.userAgent.toLowerCase()) && window.XDomainRequest) {
            // Use Microsoft XDR
            setInterval(function () {
                var xdr = new XDomainRequest();
                xdr.open("get", 'https://' + eventConnection.ServiceAddress + '/api/livetiming/settings/' + eventConnection.ClientKey + '?locale=&styleId=&ressourceId=&accessToken=' + eventConnection.AccessToken);
                xdr.onload = function () {
                    onHTTPMessage(xdr.responseText);
                };
                xdr.onerror = function () {
                    noRaces(slerror);
                };
                xdr.ontimeout = function () { };
                xdr.onprogress = function () { };
                xdr.send();
            }, 2000);
        } else {
            $.support.cors = true;
            setInterval(function () {
                $.ajax({
                    error: function (jqXHR, textStatus, errorThrown) {
                        noRaces(slerror);
                    },
                    crossDomain: true,
                    cache: false,
                    url: 'https://' + eventConnection.ServiceAddress + '/api/livetiming/settings/' + eventConnection.ClientKey + '?locale=&styleId=&ressourceId=&accessToken=' + eventConnection.AccessToken, success: function (evt) {
                        var jsonString = JSON.stringify(evt);
                        onHTTPMessage(jsonString);
                    }, dataType: "json"
                });
            }, 2000);
        }
    }
}

function startWebSocket() {
    var wsUri = null;
    if (location.protocol == "https:") {
        wsUri = "wss://" + eventData["liveServerHost"] + ":" + eventData["liveServerWssPort"] + "/";
    } else {
        wsUri = "ws://" + eventData["liveServerHost"] + ":" + eventData["liveServerWsPort"] + "/";
    }
    websocket = new WebSocket(wsUri);

    websocket.onopen = function (evt) {
        onOpen(evt)
    };

    websocket.onclose = function (evt) {
        onClose(evt)
    };

    websocket.onmessage = function (evt) {
        onMessage(evt)
    };

    websocket.onerror = function (evt) {
        onError(evt)
    };
}

function record() {
    $('.modal_bg').fadeIn(1000);
    $('.modal_bg').fadeTo("slow", 0.8);
    $('.modal_window').css({ 'display': 'block' });
    setTimeout("$('.modal_bg, .modal_window').hide()", 5500);
    return false;
}

function deleteTable() {
    var Parent = document.getElementById('timingTable');
    while (Parent.hasChildNodes()) {
        Parent.removeChild(Parent.firstChild);
    }
}

function noRaces(message) {
    var HeightScreen = $(window).height();
    var infoDiv = document.getElementById("info_races");
 
    initColors();

    $('.modal_bg2').css({ 'width': '100%' })
    $('.modal_bg2').css({ 'height': '100%' })
    $('.modal_bg2').fadeIn(500);
    $('.modal_bg2').fadeTo("slow", 1);

    $('.smsLogo').css({ 'visibility': 'hidden' });
    $('.smsLogoFooter').css({ 'visibility': 'visible' });
    $('.info_races').show();

    var Top_modal_window2 = $(document).scrollTop() + HeightScreen / 2 - $('.modal_window2').height() / 2;
    $('.modal_window2').css({ 'top': Top_modal_window2 + 'px', 'display': 'block' });
    infoDiv.innerHTML = message;
    $('#headerTable').hide();
    $('#timingTable').hide();
    return false;
}

function doSend(message) {
    websocket.send(message);
}

function Finish() {
    x = document.getElementById("clock");
    x.innerHTML = '';
    var cl = document.getElementById("finish");
    cl.innerHTML = '<img src=Livetiming/css/img/finished_small.png>' + " " + slfinished;
    cl.setAttribute("style", "visibility: visible");
    started = false;
}

function createTD(text, tr, attribute, value) {
    var td = document.createElement("TD");
    td.appendChild(document.createTextNode(text));
    td.setAttribute(attribute, value);
    td.setAttribute("style", "border: none;");
    tr.appendChild(td);
}

function createTDImg(text, tr, attribute, value, path) {
    var td = document.createElement("TD");
    var img = document.createElement("img");
    img.src = path;
    img.setAttribute('class', 'trophySize');
    td.appendChild(img);
    td.appendChild(document.createTextNode(text));
    td.setAttribute(attribute, value);
    td.setAttribute("style", "border: none;");
    tr.appendChild(td);
}

function createTH(text, tr, attribute, value) {
    var th = document.createElement("TH");
    th.appendChild(document.createTextNode(text));
    th.setAttribute(attribute, value);
    th.setAttribute("style", "border: none;");
    tr.appendChild(th);
}

function createImg(td, path) {
    var img = document.createElement('img');
    img.width = '15';
    img.src = path;
    img.className = 'up';
    td.appendChild(img);
}

function getTimeF(BTime) {
    if (BTime != 0) {
        return millisecondsToString(BTime);
    } else {
        return "";
    }
}

function millisecondsToString(milliseconds) {
    var oneHour = 3600000;
    var oneMinute = 60000;
    var oneSecond = 1000;
    var seconds = 0;
    var minutes = 0;
    var hours = 0;
    var result;

    if (milliseconds >= oneHour) {
        hours = Math.floor(milliseconds / oneHour);
    }

    milliseconds = hours > 0 ? (milliseconds - hours * oneHour) : milliseconds;

    if (milliseconds >= oneMinute) {
        minutes = Math.floor(milliseconds / oneMinute);
    }

    milliseconds = minutes > 0 ? (milliseconds - minutes * oneMinute) : milliseconds;

    if (milliseconds >= oneSecond) {
        seconds = Math.floor(milliseconds / oneSecond);
    }

    milliseconds = seconds > 0 ? (milliseconds - seconds * oneSecond) : milliseconds;

    if (hours > 0) {
        result = (hours > 9 ? hours : "0" + hours) + ":";
    }

    if (minutes > 0) {
        result = (minutes > 9 ? minutes : "0" + minutes) + ":";
    } else {
        result = "00:";
    }

    if (seconds > 0) {
        result += (seconds > 9 ? seconds : "0" + seconds) + ".";
    } else {
        result += "00.";
    }

    if (milliseconds > 0) {
        if (milliseconds > 99) {
            result += milliseconds;
        } else if (milliseconds > 9) {
            result += "0" + milliseconds;
        } else {
            result += "00" + milliseconds;
        }
    } else {
        result += "000";
    }

    return result;
}

function onTimerP() {
    sPP = parseInt(startPP);
    if (sPP <= 0) { sPP = 6 }
    sPP -= 1;
    startPP = sPP;
}

function onTimer(i) {
    var x = document.getElementById("clock");
    var hl = document.getElementById("laps");
    var cl = document.getElementById("finish");
    if (stop == true) {
        Finish();
    } else {
        var currentTime = CountTime();
        start = currentTime;

        if (clockStarted) {
            if (lapsMode == false) {
                x.setAttribute("style", "visibility:visible");
                hl.setAttribute("style", "visibility:hidden");
                cl.setAttribute("style", "visibility:hidden");
                cl.innerHTML = '';
                hl.innerHTML = '';
                if (paused == true) {
                    x.innerHTML = slpaused + ' ' + currentTime;
                }
                else {
                    x.innerHTML = currentTime;
                }
            }
            else {
                x.setAttribute('style', "visibility: hidden");
                hl.setAttribute("style", "visibility:visible");
                cl.innerHTML = '';
                hl.innerHTML = sllaps + ': ' + lapsLeft;
            }
        }
        else if (finished != true) {
            x.setAttribute('style', "visibility: hidden");
            hl.setAttribute("style", "visibility: hidden");
            cl.setAttribute("style", "visibility: hidden");
        }
        else {
            //finished
            x.setAttribute('style', "visibility: hidden");
            hl.setAttribute("style", "visibility: hidden");
            cl.setAttribute("style", "visibility: visible");
            cl.innerHTML = '';
            cl.innerHTML = '<img src=LiveTiming/css/img/finished_small.png>' + " " + slfinished;
        }
    }
    setTimeout(function () {
        onTimer();
    }, 1000);
}

function GetTimeOnM(timeRem) {
    if (timeLeft > timeRem) {
        countingUp = false;
    } else if (timeLeft < timeRem) {
        countingUp = true;
    }
    timeLeft = timeRem;
    var HoursL = parseInt(timeLeft / 3600000);
    var MinutesL = parseInt((timeLeft - HoursL * 3600000) / 60000);
    var SecondsL = parseInt((timeLeft - (HoursL * 3600000 + MinutesL * 60000)) / 1000);
    sl = new Number(SecondsL);
    ml = new Number(MinutesL);
    hl = new Number(HoursL);
    var ssl = sl < 10 ? ("0" + sl) : sl;
    var sml = ml < 10 ? ("0" + ml) : ml;
    var shl = hl < 10 ? ("0" + hl) : hl;
    return shl + ":" + sml + ":" + ssl;
}


function CountTime() {
    var hms = new String(start).split(":");
    if (!isNaN(hms[2])) {
        var s = new Number(hms[2]);
    } else {
        var s = 00;
    }
    if (!isNaN(hms[1])) {
        var m = new Number(hms[1]);
    } else {
        var m = 00;
    }
    if (!isNaN(hms[0])) {
        var h = new Number(hms[0]);
    } else {
        var h = 00;
    }
    var didHitZero = false;
    if (Number(s) != 0 && Number(m) != 0 && Number(h) != 0) {
        didhitZero = true;
    }
    if (paused == false) {
        if (countingUp || didHitZero) {
            s = s + 1;
            if (s > 59) {
                s = 00;
                m = m + 1;
                if (m > 59) {
                    m = 00;
                    h = h + 1;
                }
            }
        } else {
            s -= 1;
            if (s < 0) {
                s = 59;
                m -= 1;
                if (m < 0) {
                    m = 59;
                    if (h != 0) {
                        h -= 1;
                    }
                }
            }
        }
    } else if (paused == true) {
        s = s + 1;
        if (s > 59) {
            s = 00;
            m = m + 1;
            if (m > 59) {
                m = 00;
                h = h + 1;
            }
        }
    }
    ss = s < 10 ? ("0" + s) : s;
    var sm = m < 10 ? ("0" + m) : m;
    var sh = h < 10 ? ("0" + h) : h;
    return sh + ":" + sm + ":" + ss;
}

function writeToScreen(message) {
    var pre = document.createElement("p");
    pre.style.wordWrap = "break-word";
    pre.innerHTML = message;
    output.appendChild(pre);
}

function onHTTPMessage(jsonData) {
    if (jsonData != '{}') {
        onJSONReceived(jsonData);
    } else {
        noRaces(slnoheat);
    }
}

function onJSONReceived(parsedJSON) {
    var parsedJSON = JSON.parse(parsedJSON);
    var heat = new Heat(parsedJSON);

    $('.modal_bg2').css('background', 'transparent');
    $('.smsLogo').css({ 'visibility': 'visible' });
    $('.smsLogoFooter').css({ 'visibility': 'hidden' });
    $('.info_races').hide();

    var latestLapTime = [], lastLapTime = [], newId = [], newPosition = [], previousId = [], previousPosition = [], previousBest = [];
    var currentTime = new Date();

    counter = 0; startPP = 6;
    $('#headerTable').show();
    $('#timingTable').show();
    clockStarted = heat.getCounterStarted() == 1 ? true : false;

    switch (heat.getHeatState()) {
        case 1:
            started = true;
            stop = false;
            paused = false;
            finished = false;
            break;
        case 2:
            started = true;
            paused = true;
            stop = false;
            finished = false;
            break;
        case 3:
            stop = true;
            started = false;
            paused = false;
            finished = false;
            break;
        default:
            finished = true;
            started = false;
            stop = false;
            clockStarted = false;
    }

    for (var a in oldHeat) if (oldHeat[a] != "") counter++; //counter++ if array!=zero
    var driverDataLength = heat.getDrivers().length;

    for (var i = 0; i < driverDataLength; i++) {
        if (counter <= 0) {
            lastLapTime[i] = 0;
            newId[i] = 0;
            newPosition[i] = 0;
            previousId[i] = 0;
            previousPosition[i] = 0;
        } else {
            var previousDriverDataLength = oldHeat.getDrivers().length;
            if (previousDriverDataLength < driverDataLength) {
                var coef = previousDriverDataLength + (driverDataLength - previousDriverDataLength);
                lastLapTime[coef] = 0;
                previousId[coef] = 0;
                previousPosition[coef] = 0;
            } else {
                lastLapTime[i] = oldHeat.getDriverAtIndex(i).getLapTime(); //last lap time
                previousId[i] = oldHeat.getDriverAtIndex(i).getIdentifier(); //prev id
                previousPosition[i] = oldHeat.getDriverAtIndex(i).getPosition(); //prev position
                previousBest[i] = oldHeat.getDriverAtIndex(i).getBestLapTime(); //prev bestTime
            }
        }

        latestLapTime[i] = heat.getDriverAtIndex(i).getLapTime(); //new lap time
        newId[i] = heat.getDriverAtIndex(i).getIdentifier(); //new id
        newPosition[i] = heat.getDriverAtIndex(i).getPosition(); //new position
    }

    deleteTable();

    //get time
    lapsMode = heat.getEndCondition() == 2 ? true : false;

    //Replace header left
    document.getElementById('headerLeft').innerHTML = heat.getHeatName().replace("[HEAT]", slheat);

    //Colors and background image
    initColors();

    //Live Timing Header
    $('#t1').css('background-color', '#' + StyleBackColor);
    $('#headerTable').css('background', 'url(LiveTiming/css/img/back6.png) bottom right no-repeat');
    $('#headerLeft').css('color', '#' + StyleFontColor);
    $('#headerRight').css('color', '#' + StyleFontColor);

    //Table header
    table = document.getElementById('timingTable');
    table.setAttribute('border', 0);
    table.setAttribute('class', "timingTable");
    table.setAttribute('cellpadding', 0);
    table.setAttribute('cellspacing', 0);

    //Table header
    tr = document.createElement("TR");
    tr.setAttribute("style", "background-color: #" + StyleHighlightBackColor + "; color: #" + StyleFontColor + ";");
    createTH(slpos, tr, "class", "position");
    createTH('', tr, "class", "arrows");
    createTH(sldriver, tr, "class", "name");
    createTH(slvehicle, tr, "class", "vehicle");
    createTH(sllaps, tr, "class", "laps");
    createTH(slbest, tr, "class", "best");
    createTH(slaverage, tr, "class", "average");
    createTH(sllast, tr, "class", "last");
    createTH(slgap, tr, "class", "gap");
    table.appendChild(tr);

    //table rows
    for (var i = 0; i < driverDataLength; i++) {
        var driver = heat.getDriverAtIndex(i);
        var firstDriver = heat.getDriverAtIndex(0);
        if (firstDriver.getLaps() < 2) {
            GenerateTable(false, eventData["showDriversWithNoLaps"]);
        } else {
            GenerateTable(true, eventData["showDriversWithNoLaps"]);
        }
    }
    // lap passing animation
    if (upd != true) {
        for (var i = 0; i < driverDataLength; i++) {
            var lapsN = [];
            lapsN[i] = heat.getDriverAtIndex(i).getLaps();
            if (lapsN[i] > lapsD[i]) {
                var white = document.getElementById("row" + i);
                white.setAttribute("style", "background-color: #" + StyleHighlightBackColor + "; color: #" + StyleFontColor);
                var rowID = white.id;
                var rowMod = i % 2;
                if (rowMod == 0) {
                    $("#row" + i).animate({ backgroundColor: '#' + StyleEvenRowColor, color: '#' + StyleFontColor }, 1000);
                }
                else {
                    $("#row" + i).animate({ backgroundColor: '#' + StyleHighlightFontColor, color: '#' + StyleFontColor }, 1000);
                }
            }
        }
    }

    for (i = 0; i < driverDataLength; i++) {
        lapsD[i] = heat.getDriverAtIndex(i).getLaps();
        newRec[i] = heat.getDriverAtIndex(i).getLastRecord();
    }

    //time
    if (lapsMode == false && clockStarted) {
        $('#clock').css('visibility', 'visible');
        document.getElementById('laps').innerHTML = '';
        if (timeLeft != heat.getTimeLeft()) {
            start = GetTimeOnM(heat.getTimeLeft());
        }
    }

    startPP = 6;
    lapsLeft = heat.getRemainingLaps();
    upd = false;
    oldHeat = heat;

    function GenerateTable(check, ShowDriversWithNoLaps) {
        if (check && ShowDriversWithNoLaps) {
            if (driver.getLaps() != 0) {
                GenerateInternalTableHelper();
            }
        } else {
            GenerateInternalTableHelper();
        }
    }

    function GenerateInternalTableHelper() {
        var k = 0;
        var r = 0;
        tr = document.createElement("TR");
        tr.setAttribute("id", "row" + i);
        var rowMod = i % 2;
        if (rowMod == 0) {
            tr.setAttribute("style", "color: #" + StyleFontColor + "; background-color: " + StyleEvenRowColor + ";");
        } else {
            tr.setAttribute("style", "color: #" + StyleFontColor + "; background-color: #" + StyleHighlightFontColor + ";");
        }

        //position
        if (driver.getPosition() == 1) {
            path = 'LiveTiming/css/img/gold_compact.png';
            createTDImg(driver.getPosition(), tr, "class", "position", path);
        } else if (driver.getPosition() == 2) {
            path = 'LiveTiming/css/img/silver_compact.png';
            createTDImg(driver.getPosition(), tr, "class", "position", path);
        } else if (driver.getPosition() == 3) {
            path = 'LiveTiming/css/img/bronze_compact.png';
            createTDImg(driver.getPosition(), tr, "class", "position", path);
        } else {
            createTD(driver.getPosition(), tr, "class", "position");
        }

        //arrows
        td = document.createElement("TD");
        td.setAttribute("style", "border: none;");

        if (previousPosition[i] != 0) {
            if (newPosition[i] < previousPosition[i]) {
                console.log('newposition: ' + newPosition[i] + ' //  oldposition: ' + previousPosition[i] + ' SO: UP');
            } else if (newPosition[i] > previousPosition[i]) {
                console.log('newposition: ' + newPosition[i] + ' //  oldposition: ' + previousPosition[i] + ' SO: DOWN');
            } else {
                console.log('position is the same');
            }
        }

        if (counter > 0) {
            if (newId[i] != previousId[i]) {
                for (var g = 0; g < driverDataLength; g++) {
                    if (previousId[g] == newId[i]) { k = g + 1; }
                    if (newPosition[i] < k) {
                        td = document.createElement("TD");
                        td.setAttribute("style", "border: none;");
                        createImg(td, 'LiveTiming/css/img/green.png');
                        M3[i] = 1;
                    }
                    else {
                        td = document.createElement("TD");
                        td.setAttribute("style", "border: none;");
                        createImg(td, 'LiveTiming/css/img/red.png');
                        M3[i] = 2;
                    }
                }
                secP = currentTime.getSeconds();
            }
            else {
                if (M3[i] == 0) {
                    td.appendChild(document.createTextNode(''));
                }
                else {
                    if (secP != 0) {
                        var secCH;
                        secCH = currentTime.getSeconds();
                        if (secP > secCH) { secCH = secCH + 60 }
                        if ((secCH - secP) >= 20) {
                            td.appendChild(document.createTextNode(''));
                            secP = 0;
                        }
                        else {
                            td.appendChild(document.createTextNode(''));
                            if (M3[i] == 1) {
                                td = document.createElement("TD");
                                td.setAttribute("style", "border: none;");
                                createImg(td, 'LiveTiming/css/img/green.png');
                            }
                            else {
                                td = document.createElement("TD");
                                td.setAttribute("style", "border: none;");
                                createImg(td, 'LiveTiming/css/img/red.png');
                            }
                        }
                    }
                    else {
                        M3[i] = 0;
                        td.appendChild(document.createTextNode(''));
                    }
                }
            }
        }
        else {
            M3[i] = 0;
            td.appendChild(document.createTextNode(''));
        }
        tr.appendChild(td);

        //name
        createTD(driver.getDriverName(), tr, "class", "name");
        //kart
        createTD(driver.getKartNumber(), tr, "class", "vehicle");
        //laps
        createTD(driver.getLaps(), tr, "class", "laps");
        //best
        var currentBest = driver.getBestLapTime();
        var currentBestTime = getTimeF(currentBest);
        if (previousBest[i] < currentBest) {
            createTD(currentBestTime, tr, "class", "best", "class", "lapTimeUp");
        } else {
            createTD(currentBestTime, tr, "class", "best");
        }

        //average
        timeA = getTimeF(driver.getAvarageLapTime());
        createTD(timeA, tr, "class", "average");

        //last
        var timeT = getTimeF(driver.getLapTime());
        if (driver.getLaps() <= 1) {
            createTD(timeT, tr);
            M4[i] = 0;
        }
        else {
            if (newId[i] != previousId[i]) //if new position != prev position
            {
                for (var g = 0; g < driverDataLength; g++) {
                    if (previousId[g] == newId[i]) { r = g; } //r = prev ID
                }
                if (latestLapTime[i] > lastLapTime[r]) {
                    if (counter > 0) {
                        createTD(timeT, tr, "class", "lapTimeDown");
                        M4[i] = 1;
                    }
                    else {
                        createTD(timeT, tr);
                        M4[i] = 0;
                    }
                }
                else if (latestLapTime[i] < lastLapTime[r]) {
                    createTD(timeT, tr, "class", "lapTimeUp");
                    M4[i] = 2;
                }
                else {
                    if (M4[i] == 1) { createTD(timeT, tr, "class", "lapTimeDown"); }
                    else if (M4[i] == 2) { createTD(timeT, tr, "class", "lapTimeUp"); }
                    else { createTD(timeT, tr, "class", "lapTimeUp"); }
                }
            }
            else {
                if (latestLapTime[i] > lastLapTime[i]) {
                    if (counter > 0) {
                        createTD(timeT, tr, "class", "lapTimeDown");
                        M4[i] = 1;
                    }
                    else {
                        createTD(timeT, tr);
                        M4[i] = 0;
                    }
                }
                else if (latestLapTime[i] < lastLapTime[i]) {
                    createTD(timeT, tr, "class", "lapTimeUp");
                    M4[i] = 2;
                }
                else {
                    if (M4[i] == 1) { createTD(timeT, tr, "class", "lapTimeDown"); }
                    else if (M4[i] == 2) { createTD(timeT, tr, "class", "lapTimeUp"); }
                    else { M4[i] = 0; createTD(timeT, tr); }
                }
            }
        }
        //gap
        //if first lap, don't put any data inside the tabledata.
        if (i != 0) {
            createTD(driver.getGapTime(), tr, "class", "gap");
        } else {
            createTD('', tr, "class", "gap");
        }
        table.appendChild(tr);

        //best time
        var rec = driver.getLastRecord();
        if (newRec[i] != 0 && newRec[i] != rec) {
            if (rec != 5) {
                var finishPP = "0";
                var timerPP = null;
                sPP = 6;
                switch (rec) {
                    case 0: document.getElementById("best").innerHTML = slday;
                        break;
                    case 1: document.getElementById("best").innerHTML = slweek;
                        break;
                    case 2: document.getElementById("best").innerHTML = slmonth;
                        break;
                    case 3: document.getElementById("best").innerHTML = slyear;
                        break;
                    case 4: document.getElementById("best").innerHTML = slever;
                        break;
                }
                document.getElementById("bestTime").innerHTML = currentBestTime;
                document.getElementById("bestDriver").innerHTML = driver.getDriverName();
                record();
                $('.modal_window button').click(function () {
                    $('.modal_bg, .modal_window').hide();
                    $("body").css({ "overflow": "auto" });
                });
            }
        }
    }
}