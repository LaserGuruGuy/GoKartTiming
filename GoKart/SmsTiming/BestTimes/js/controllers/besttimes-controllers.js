function getBestTimes(baseConnection, params) {
    var bestTimes = getModel(createFullPath(baseConnection, "/api/besttimes/records/"));
    return bestTimes.findAll({ locale: params.locale, rscId: params.rscId, scgId: params.scgId, startDate: params.startDate, endDate: params.endDate, maxResult: params.maxResult, accessToken: baseConnection.accessToken });
}

function getBestTimesResources(baseConnection, params) {
    var resources = getModel(createFullPath(baseConnection, "/api/besttimes/resources/"));
    return resources.findAll({ locale: params.locale, rscId: params.rscId, accessToken: baseConnection.accessToken });
}
