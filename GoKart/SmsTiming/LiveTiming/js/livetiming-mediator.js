function initialize() {
    var baseUrl = "https://backend.sms-timing.com";
    var auth = "Y2lyY3VpdHBhcmtiZXJnaGVtOjNmZGIwZDY5LWQxYmItNDZmMS1hYTAyLWNkZDkzODljMmY1MQ==";
    var params = getUrlParams();
    var baseConnection;

    if (params.customCSS != null) {
        addCustomCSS(params.customCSS);
    }

    can.when(getConnectionInfo(baseUrl, auth)).then(function (connectionInfo) {
        baseConnection = parseConnectionInfo(connectionInfo);
        can.when(getLiveTimingSettings(baseConnection, params)).then(function (settings) {
            initializeLiveTiming(settings, connectionInfo);
            initBranding(settings);
        });
    });
}