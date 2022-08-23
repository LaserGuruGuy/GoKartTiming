function getConnectionInfo(baseUrl, authorizationToken) {
    authorizationToken = decodeURIComponent(authorizationToken);
    var url;
    if (window.external.url != null) {
        url = window.external.url;
    }
    else {
        url = baseUrl + "/api/connectioninfo?type=modules";
    }
    $.support.cors = true;
    return can.ajax({
        crossDomain: true,
        url: url,
        beforeSend: function (aRequest) {
            aRequest.setRequestHeader("Authorization", "Basic " + authorizationToken);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("HTTP Status: " + XMLHttpRequest.status + "; Error Text: " + XMLHttpRequest.responseText);
            console.log(XMLHttpRequest.responseText + ' ' + textStatus + ': ' + errorThrown);
         },
        success: function (data) {
            try {
                window.external.PolupateConnectionService(JSON.stringify(data));
            }
            catch (error) {
                console.error(error);
            }
        },
        dataType: 'json'
    });
}