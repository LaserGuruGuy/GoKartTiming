function initialize() {
    var baseUrl;
    var auth = url('?key');
    var params = getUrlParams();
    var baseConnection;

    baseUrl = "https://backend.sms-timing.com";

    if (window.external.auth != null) {
        auth = window.external.auth;
    }
    else if (auth == null) {
        auth = "Y2lyY3VpdHBhcmtiZXJnaGVtOjNmZGIwZDY5LWQxYmItNDZmMS1hYTAyLWNkZDkzODljMmY1MQ==";
    }

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