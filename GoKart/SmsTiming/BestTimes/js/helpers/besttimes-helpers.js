function getUrlParams() {
    var params = { locale: getLocale(), styleId: null, rscId: null, scgId: null, startDate: null, endDate: null, maxResult: null, customCSS: null, scgha: null, scGroup: null, scgHideArray: null, nodeId: null };
    return params;
}

function createDateForApiCall(date) {
    var twoDigitMonth = ((date.getMonth().length + 1) === 1) ? (date.getMonth() + 1) : (date.getMonth() + 1);
    date = date.getFullYear() + "-" + twoDigitMonth + "-" + date.getDate() + " 06:00:00";
    return date;
}

function getStartDate(dateId, currentDate) {
    switch (dateId) {
        case 0:
            currentDate.setYear(currentDate.getFullYear() - 100);
            return createDateForApiCall(currentDate);
        case 1:
            currentDate = getDateForThisYear(currentDate);
            return createDateForApiCall(currentDate);
        case 2:
            currentDate = getDateForThisMonth(currentDate);
            return createDateForApiCall(currentDate);
        case 3:
            currentDate = getDateForThisWeek(currentDate);
            return createDateForApiCall(currentDate);
        case 4:
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