function createLiveTimingSettingsModel(baseConnection, path, params) {
    var mainPath = createFullPath(baseConnection, path);
    var settings = getModel(mainPath);
    debugger;
    return settings.findAll({ locale: params.locale, styleId: params.styleId, resourceId: params.resourceId, accessToken: baseConnection.accessToken });
}