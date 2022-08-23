function getConnectionInfo(baseUrl, authorizationToken) {
    authorizationToken = decodeURIComponent(authorizationToken);
    $.support.cors = true;
    return can.ajax({
        crossDomain: true,
        url: baseUrl + "/api/connectioninfo?type=modules",
        beforeSend: function (aRequest) {
            aRequest.setRequestHeader("Authorization", "Basic " + authorizationToken);
            try {
                window.external.onLogMessage('ajax url:' + this.url + ' ' + authorizationToken);
            }
            catch (error) {
                console.error(error);
            }
            finally {
                console.log(this.url + ' ' + authorizationToken);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log("HTTP Status: " + XMLHttpRequest.status + "; Error Text: " + XMLHttpRequest.responseText);
            console.log(XMLHttpRequest.responseText + ' ' + textStatus + ': ' + errorThrown);
         },
        success: function (data) {
            try {
                window.external.onJSONReceived(JSON.stringify(data));
            }
            catch (error) {
                console.error(error);
            }
            finally {
                console.log(JSON.stringify(data));
            }
        },
        dataType: 'json'
    });
}