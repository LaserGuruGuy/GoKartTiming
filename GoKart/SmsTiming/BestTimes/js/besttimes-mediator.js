function initialize() {
    if (window.external.baseUrl != null) {
        baseUrl = window.external.baseUrl;
    }
    else {
        baseUrl = "https://backend.sms-timing.com";
    }

    if (window.external.auth != null) {
        auth = window.external.auth;
    }
    else {
        auth = "aGV6ZW1hbnM6aW5kb29ya2FydGluZw%3D%3D";
    }

    params = getUrlParams();

    can.when(getConnectionInfo(baseUrl, auth)).then(function (connectionInfo) {
        window.external.ClientKey = connectionInfo.ClientKey;
        window.external.ServiceAddress = connectionInfo.ServiceAddress;
        window.external.AccessToken = connectionInfo.AccessToken;

        baseConnection = parseConnectionInfo(connectionInfo);

        can.when(getBestTimesResources(baseConnection, params)).then(function (resources) {
            try {
                window.external.onModel(JSON.stringify({ resourceId: resources[0]["resourceId"] }));
            }
            catch (error) {
            }
        });
    });
}

function getBestTimesGroup(rscId, scgId, startDate) {
    params = getUrlParams();

    params.rscId = rscId;
    params.scgId = scgId;
    params.startDate = startDate;

    connectionInfo = { ClientKey: window.external.ClientKey, ServiceAddress: window.external.ServiceAddress, AccessToken: window.external.AccessToken };

    baseConnection = parseConnectionInfo(connectionInfo);

    getBestTimes(baseConnection, params);
}