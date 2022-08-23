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
