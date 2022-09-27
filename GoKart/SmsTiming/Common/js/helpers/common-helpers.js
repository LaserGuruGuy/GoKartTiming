function addCustomCSS(customCSS) {
    if (customCSS !=null) {
        $('head').append('<link href=' + customCSS + ' rel="stylesheet" />');
    }
}

function getLocale() {
    var locale = url('?locale');
    if (locale == null) {
        locale = navigator.language || navigator.userLanguage;
    }
    return locale;
}

function getModel(mainPath) {
    var model = can.Model.extend({
        findAll: 'GET ' + mainPath,
        parseModels: function (data) {
            try {
                if (data.records != null) {
                    var report = { recordgroup: data.records }
                }
                else if (data[0].scoregroups != null) {
                    var report = { scoregroup: data[0].scoregroups }
                }
                else {
                    var report = data;
                }
                window.external.onModel(JSON.stringify(report));
            }
            catch (error) {
            }
            return data;
        }
    }, {});
    return model;
}

function createModel(mainPath) {
    var model = can.Model.extend({
        create: 'POST ' + mainPath,
        parseModels: function (data) {
            return data;
        }
    }, {});
    return model;
}
