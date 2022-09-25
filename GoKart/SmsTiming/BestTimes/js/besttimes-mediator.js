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
        console.log("auth:" + auth);
        
        baseConnection = parseConnectionInfo(connectionInfo);

        can.when(getBestTimesResources(baseConnection, params)).then(function (resources) {
            can.when(getBestTimesTranslations(baseConnection, params)).then(function (translations) {
                translations = parseTranslations(translations);
                addAllComponentToScoreGroup(resources, translations);
                params.rscId = resources[0]["resourceId"];

                //if "all" scoregroup is hidden, fetch data from first available group
                if (params.scgHideArray != null && params.scgHideArray[0] == 'null') {
                    params.scgId = resources[0].scoregroups[params.scgHideArray.length - 1].id;
                }

                var currentDate = getCurrentDate();
                currentDate = getDateForThisMonth(currentDate);
                params.startDate = createDateForApiCall(currentDate);

                can.when(getBestTimes(baseConnection, params)).then(function (records) {
                    can.when(getBestTimesSettings(baseConnection, params)).then(function (settings) {
                        var resourcesScoreGroups = retrieveSpecificResource(resources, params.rscId);
                        var mainControl = createBestTimesMainControl();
                        var mC = new mainControl("#wrapper", { resources: resources, resourcesScoreGroups: resourcesScoreGroups, records: records, params: params, settings: settings, translations: translations });
                    });
                    addCustomCSS(params.customCSS);
                });
            });
        });
    });
}