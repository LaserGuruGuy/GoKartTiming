function initialize() {
    var baseUrl = "https://backend.sms-timing.com";
    var auth = url('?key');
    var params = getUrlParams();
    var baseConnection;

    if (window.external.auth != null) {
        auth = window.external.auth;
    }
    else if (auth == null) {
        auth = "aGV6ZW1hbnM6aW5kb29ya2FydGluZw%3D%3D";
    }

    can.when(getConnectionInfo(baseUrl, auth)).then(function (connectionInfo) {
        baseConnection = parseConnectionInfo(connectionInfo);
        can.when(getBestTimesResources(baseConnection, params)).then(function (resources) {
            params.rscId = resources[0]["resourceId"];
            window.external.onModel(JSON.stringify({ resourceId: resources[0]["resourceId"] }));
        });
    });
}

function getBestTimesResourcesGroup() {
    var baseUrl = "https://backend.sms-timing.com";
    auth = window.external.auth;
    var params = getUrlParams();

    can.when(getConnectionInfo(baseUrl, auth)).then(function (connectionInfo) {
        baseConnection = parseConnectionInfo(connectionInfo);
        can.when(getBestTimesResources(baseConnection, params)).then(function (resources) {
            params.rscId = resources[0]["resourceId"];
            window.external.onModel(JSON.stringify({ resourceId: resources[0]["resourceId"] }));
        });
    });
}

function getBestTimesGroup(rscId, scgId, startDate) {
    var baseUrl = "https://backend.sms-timing.com";
    auth = window.external.auth;
    var params = getUrlParams();

    params.rscId = rscId;
    params.scgId = scgId;
    params.startDate = startDate;

    can.when(getConnectionInfo(baseUrl, auth)).then(function (connectionInfo) {
        baseConnection = parseConnectionInfo(connectionInfo);
            getBestTimes(baseConnection, params);
    });
}