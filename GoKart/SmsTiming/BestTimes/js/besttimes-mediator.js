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
            for (var i = 0; i < 5; i++) {
                params.startDate = getStartDate(i, getCurrentDate());
                for (var j = 0; j < resources[0].scoregroups.length; j++) {
                    params.scgId = resources[0].scoregroups[j].id;
                    try {
                        window.external.onModel(JSON.stringify({ parametergroup: params }));
                    }
                    catch (error) {
                    }
                    can.when(getBestTimes(baseConnection, params)).then(function (records) {
                    });
                }
            }
        });
    });
}