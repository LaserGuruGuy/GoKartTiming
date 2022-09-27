function initialize() {
    baseUrl = window.external.baseUrl;
    auth = window.external.auth;
    params = getUrlParams();

    can.when(getConnectionInfo(baseUrl, auth)).then(function (connectionInfo) {

        window.external.ClientKey = connectionInfo.ClientKey;
        window.external.ServiceAddress = connectionInfo.ServiceAddress;
        window.external.AccessToken = connectionInfo.AccessToken;

        baseConnection = parseConnectionInfo(connectionInfo);

        can.when(getBestTimesResources(baseConnection, params)).then(function (resources) {
            params.rscId = resources[0]["resourceId"];
            window.external.onModel(JSON.stringify({ resourceId: resources[0]["resourceId"] }));
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