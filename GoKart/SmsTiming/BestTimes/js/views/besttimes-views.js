var templatePath = 'BestTimes/templates/';
//var templatePath = "https://modules.sms-timing.com/BestTimes/template/";
var result;

function createBestTimesView(resources, resourcesScoreGroups, records, baseConnection, settings, params, translations, eventCall) {
    createBestTimesWrapper(resources, resourcesScoreGroups, settings, params, translations, eventCall);
    createBestTimesTable(records, baseConnection, settings, translations);
    enableCustomCssPage(settings);
}

function createBestTimesWrapper(resources, resourcesScoreGroups, settings, params, translations, eventCall) {

    var hide = params.scgha;

    var hideGroup = params.scgHideArray;

    if (hideGroup != null) {
        for (var i = 0; i < hideGroup.length; i++) {
            hideScoreGroup(resourcesScoreGroups, hideGroup[i], settings);
        }
    }
    
    if (!$("#resource").length) {
        if (url('?rscid') == null) {
            checkOnlyOneResource(resources, settings);
        }
        $.support.cors = true;
        var resourceTemplate = can.view(templatePath + 'besttimes-template-resources.html', { resources: resources });
        document.getElementById('wrapper').appendChild(resourceTemplate);
        enableCustomCssResourceMenu(settings);
        if (resources.length == 1) {;
            $('body').append('<style> div#resource:after{visibility: hidden;} div#resource{cursor: default;}');
        } else {      
            enableDropDownMenu("#resource");
        }
    }

    if (!$("#group").length) {
        if (hideGroup == null) {
        getScoreGroupById(resourcesScoreGroups, params.scgId);
        }
        var scoreGroupsTemplate = can.view(templatePath + 'besttimes-template-scoregroups.html', { resourcesScoreGroups: resourcesScoreGroups, result: result });
        document.getElementById('wrapper').appendChild(scoreGroupsTemplate);
    }

    if (!$("#group li").length) {
        var scoreGroupsItemsTemplate = can.view(templatePath + 'besttimes-template-scoregroups-items.html', { resourcesScoreGroups: resourcesScoreGroups, hide: hide });
        document.getElementById('scoreDropDown').appendChild(scoreGroupsItemsTemplate);
        enableCustomCssGroupMenu(settings);
        if (eventCall == true) {
            enableDropDownMenu("#group");
        }
        if (hide != null) {;
            $('body').append('<style> div#group:after{visibility: hidden;} div#group{cursor: default; padding-left: 0px;}');
        } else {
            enableDropDownMenu("#group");
        }
    }
    if (!$("#time").length) {
        var timeTemplate = can.view(templatePath + 'besttimes-template-times.html', { translations: translations });
        document.getElementById('wrapper').appendChild(timeTemplate);
        enableCustomCssTimeMenu(settings);
        enableDropDownMenu("#time");
    }
}

function createBestTimesTable(records, baseConnection, settings, translations) {
    records = records.records;

    for (var i = 0; i < records.length; i++) {
        var date = parseDate(records[i]["date"]);
        records[i].attr("date", date);
    }

    var firstRecord = records[0];
    var secondRecord = records[1];
    var thirdRecord = records[2];
    records.splice(0, 3);

    if ($("#dataTable").length > 0) {
        $('#dataTable').dataTable().empty();
        $("#dataTable").remove();
    }

    //check branding
    var branding = false;
    if (settings.branding == 'BmiLeisure') { branding = true };

    var template = can.view(templatePath + 'besttimes-template-table.html', { records: records, translations: translations, firstRecord: firstRecord, secondRecord: secondRecord, thirdRecord: thirdRecord, brandingFEC: branding  });
    document.getElementById('container').appendChild(template);
    enableCustomCssDataTable(settings);
    enableDataTable();
}

function disableAfterDropDownArrow(resourcesScoreGroups, scoreGroupNumber) {
    $("#group span").text(resourcesScoreGroups.scoreGroups[scoreGroupNumber]["scoreGroupName"]);
    var css = '<style id="pseudo">#group::after{display: none !important;}</style>';
    document.head.insertAdjacentHTML('beforeEnd', css);
}
