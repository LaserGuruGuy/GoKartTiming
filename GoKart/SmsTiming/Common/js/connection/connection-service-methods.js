function createFullPath(baseConnection, path) {
    var myUrl = "https://" + baseConnection.serviceAddress + path + baseConnection.clientKey;
    return myUrl;
}

function parseConnectionInfo(connectionInfo) {
    return baseConnection = { clientKey: connectionInfo["ClientKey"], accessToken: connectionInfo["AccessToken"], serviceAddress: connectionInfo["ServiceAddress"] };
 }
