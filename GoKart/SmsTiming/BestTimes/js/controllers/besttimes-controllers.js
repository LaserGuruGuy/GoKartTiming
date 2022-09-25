function getBestTimes(baseConnection, params) {
    return createBestTimesModel(baseConnection, "/api/besttimes/records/", params);
}

function getBestTimesSettings(baseConnection, params) {
    return createBestTimesSettingsModel(baseConnection, "/api/besttimes/settings/", params);
}

function getBestTimesResources(baseConnection, params) {
    return createBestTimesResources(baseConnection, "/api/besttimes/resources/", params);
}

function getBestTimesTranslations(baseConnection, params) {
    return createBestTimesTranslations(baseConnection, "/api/besttimes/translations/", params);
}
