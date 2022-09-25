function getUrlParams() {
    var rscId,
        scgId = null,
        styleId = null,
        locale = null,
        startDate = null,
        endDate = null,
        maxResult = null,
        customCSS = null,
        scgha = null,
        scGroup = null,
        scgHide = null,
        nodeId = null;

    locale = getLocale();

    styleId = url('?styleId');
    rscId = url('?rscid');
    scgId = url('?scgid');
    startDate = url('?startDate');
    endDate = url('?endDate'),
    maxResult = url('?maxResult');
    customCSS = url('?customCSS');
    scgha = url('?scgha');
    scGroup = url('?scgo');
    scgHide = url('?scgh');
    nodeId = url('?nodeId');

    //split scgHide into array
    if (scgHide != null) {
        var scgHideArray = scgHide.split(',');
    }

    var params = { locale: locale, styleId: styleId, rscId: rscId, scgId: scgId, startDate: startDate, endDate: endDate, maxResult: maxResult, customCSS: customCSS, scgha: scgha, scGroup: scGroup, scgHideArray: scgHideArray, nodeId: nodeId };
    return params;
}

function parseDate(date) {
    locale = getLocale();
    var dateOnly = date.split("T");
    date = dateOnly[0];
    var p = date.split(/\D/g);
    if (locale == "ENG") {
        return [p[1], p[2], p[0]].join("-");
    } else {
        return [p[2], p[1], p[0]].join("-");
    }
}

function createDateForApiCall(date) {
    var twoDigitMonth = ((date.getMonth().length + 1) === 1) ? (date.getMonth() + 1) : (date.getMonth() + 1);
    date = date.getFullYear() + "-" + twoDigitMonth + "-" + date.getDate() + " 06:00:00";
    return date;
}

function getStartDate(dateId, currentDate) {
    switch (dateId) {
        case "0":
            currentDate.setYear(currentDate.getFullYear() - 100);
            return createDateForApiCall(currentDate);
        case "1":
            currentDate = getDateForThisYear(currentDate);
            return createDateForApiCall(currentDate);
        case "2":
            currentDate = getDateForThisMonth(currentDate);
            return createDateForApiCall(currentDate);
        case "3":
            currentDate = getDateForThisWeek(currentDate);
            return createDateForApiCall(currentDate);
        case "4":
            return createDateForApiCall(currentDate);
    }
}

function getDateForThisWeek(date) {
    var day = date.getDay();
    var diff = date.getDate() - day + (day == 0 ? -6 : 1);
    return new Date(date.setDate(diff));
}

function getDateForThisMonth(date) {
    date.setDate(1);
    return date;
}

function getDateForThisYear(date) {
    date.setDate(1);
    date.setMonth(0);
    return date;
}

function getCurrentDate() {
    var date = new Date();
    date.setUTCHours(0);
    return date;
}

function retrieveSpecificResource(resources, resourceId) {
    if (resources != null) {
        for (var i = 0; i < resources.length; i++) {
            var r = resources[i];
            if (r.resourceId == resourceId) {
                return r;
            }
        }
    }
    return null;
}

function addAllComponentToScoreGroup(resources, translations) {
    for (var i = 0; i < resources.length; i++) {
        resources[i]["scoreGroups"].splice(0, 0, { scoreGroupId: "null", scoreGroupName: translations.allTranslation });
    }

}

function checkOnlyOneResource(resources, settings) {
    if (resources.length == 1) {
        resources[0].attr("resourceName", settings["title"]);
    }
}

function getScoreGroupById(scoreGroups, id) {
    if (scoreGroups != null) {
        for (var i = 0; i < scoreGroups.scoreGroups.length; i++) {
            if (scoreGroups.scoreGroups[i].scoreGroupId == id) {
                return scoreGroups.scoreGroups[i];
            }
        }
    }
    return null;
}

function parseTranslations(translations) {
    var listToTranslate = {
        allTranslation: "All",
        positionTranslation: "Pos",
        driverTranslation: "Driver",
        timeTranslation: "Time",
        dateTranslation: "Date",
        //bestTimesTranslation: "Best times", <- Time dropdown comment this for 'Ever', uncomment this for 'Best Times Ever'.
        everTranslation: "Ever",
        thisYearTranslation: "This year",
        thisMonthTranslation: "This month",
        thisWeekTranslation: "This week",
        todayTranslation: "Today"
    };
    return translate(translations, listToTranslate);
}

function translate(translations, listToTranslate) {
    for (var el in listToTranslate) {
        for (var i = 0; i < translations.length; i++) {
            if (translations[i].key == listToTranslate[el]) {
                var t = translations[i].value;
                listToTranslate[el] = translations[i].value;
            }
        }
    }
    return listToTranslate;
}
function hideScoreGroup(scoreGroups, groupToHide, settings) {
    if (scoreGroups.length != 0) {
        var hideGroup = (groupToHide).replace(/_/g, ' ');
        for (var i = 0; i < scoreGroups.scoreGroups.length; i++) {
            var highestIndex = i;
            if (hideGroup == scoreGroups.scoreGroups[i]["scoreGroupId"]) {
                var int = i;
            }
        }
        //delete scoreGroups.scoreGroups[int];
        scoreGroups.scoreGroups.splice(int, 1);
        return scoreGroups;
    }
}