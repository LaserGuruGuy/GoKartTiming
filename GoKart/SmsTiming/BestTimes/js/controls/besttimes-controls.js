function createBestTimesMainControl() {
    var mainControl = can.Control.extend({
        init: function (element, options) {

            createBestTimesView(options.resources, options.resourcesScoreGroups, options.records, baseConnection, options.settings, options.params, options.translations,null);
            //$("#group span").text(options.resourcesScoreGroups.scoreGroups[0]["scoreGroupName"]);

            $(".resourceListItem").click(function (args) {

                options.params.rscId = args.currentTarget.id;
                options.resourcesScoreGroups = retrieveSpecificResource(options.resources, options.params.rscId);
                addAllComponentToScoreGroup(options.resourcesScoreGroups, options.translations);
                if ($("#group").length > 0) {
                    $(".groupListItem").remove();
                }
                options.params.scgId = options.resourcesScoreGroups.scoreGroups[0]["scoreGroupId"];
                $("#group span").text(options.resourcesScoreGroups.scoreGroups[0]["scoreGroupName"]);

                can.when(getBestTimes(baseConnection, options.params)).then(function (records) {
                    createBestTimesView(options.resources, options.resourcesScoreGroups, records, baseConnection, options.settings, options.params, options.translations, true);
                });
            });

            $("ul#scoreDropDown").on("click", ".groupListItem", function (args) {
                options.params.scgId = args.currentTarget.id;
                options.resourcesScoreGroups = retrieveSpecificResource(options.resources, options.params.rscId);

                can.when(getBestTimes(baseConnection, options.params)).then(function (records) {
                    createBestTimesView(options.resources, options.resourcesScoreGroups, records, baseConnection, options.settings, options.params, options.translations, null, options.params);
                });
            });

            $(".dateListItem").click(function (args) {
                var dateId = args.currentTarget.id;

                options.params.startDate = getStartDate(dateId, getCurrentDate());

                can.when(getBestTimes(baseConnection, options.params)).then(function (records) {
                    createBestTimesView(options.resources, options.resourcesScoreGroups, records, baseConnection, options.settings, options.params, options.translations, null);
                });
            });
        }
    });
    return mainControl;
}