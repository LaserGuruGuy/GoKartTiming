function getLiveTimingSettings(baseConnection, params) {
    return createLiveTimingSettingsModel(baseConnection, "/api/livetiming/settings/", params);
}