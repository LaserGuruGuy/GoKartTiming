function createBestTimesModel(baseConnection, path, params) {
    var mainPath = createFullPath(baseConnection, path);
    var bestTimes = getModel(mainPath);
    return bestTimes.findAll({ locale: params.locale, rscId: params.rscId, scgId: params.scgId, startDate: params.startDate, endDate: params.endDate, maxResult: params.maxResult, accessToken: baseConnection.accessToken });
}

function createBestTimesSettingsModel(baseConnection, path, params) {
    var mainPath = createFullPath(baseConnection, path);
    var settings = getModel(mainPath);
    return settings.findAll({ locale: params.locale, styleId: params.styleId, accessToken: baseConnection.accessToken });
}

function createBestTimesResources(baseConnection, path, params) {
    var mainPath = createFullPath(baseConnection, path);
    var resources = getModel(mainPath);
    return resources.findAll({ locale: params.locale, rscId: params.rscId, accessToken: baseConnection.accessToken });
}

function createBestTimesTranslations(baseConnection, path, params) {
    var mainPath = createFullPath(baseConnection, path);
    var resources = getModel(mainPath);
    return resources.findAll({ locale: params.locale, accessToken: baseConnection.accessToken });
}
