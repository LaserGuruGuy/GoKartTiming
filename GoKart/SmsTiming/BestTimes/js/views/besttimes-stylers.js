var backColor;
var fontColor;
var highlightBackColor;
var highlightFontColor;

function loadColors(settings) {
    backColor = parseHtmlColor(settings["backColor"]).colorSubstring;
    fontColor = parseHtmlColor(settings["fontColor"]).colorSubstring;
    highlightBackColor = parseHtmlColor(settings["highlightBackColor"]).colorSubstring;
    highlightFontColor = parseHtmlColor(settings["highlightFontColor"]).colorSubstring;

    // Fast 5 color default
    if (highlightFontColor = 'FFFFFF') {
        highlightFontColor = '333333';
    }
}

function enableCustomCssPage(settings) {
    loadColors(settings);
    if (isTransparent(settings["backColor"])) {
        jQuery('body').css({ 'background-color': 'transparent' });
    } else {
        jQuery('body').css({ 'background-color': '#' + backColor });
    }
    jQuery('div.container').css({ 'background-color': '#' + backColor });
}

function enableCustomCssMainMenu(settings) {
    loadColors(settings);
    jQuery('.wrapper-dropdown-3').css({ 'background-color': '#' + backColor });
    jQuery('.wrapper-dropdown-3 .dropdown').css({ 'background-color': '#' + backColor });
    jQuery('.wrapper-dropdown-3 .dropdown li a').css({ 'color': '#' + fontColor });
    jQuery('div.wrapper-demo').css({ 'background-color': '#' + backColor });
}

function enableCustomCssResourceMenu(settings) {
    loadColors(settings);
    jQuery('div#resource').css({ 'background-color': '#' + highlightBackColor, 'color': '#' + fontColor });
    jQuery('div.soloDropdown').css({ 'background-color': '#' + backColor });
    jQuery('.responsiveBackground').css({ 'background-color': '#' + backColor });
    jQuery('li.resourceListItem').css({ 'background-color': '#' + backColor });
    jQuery('.wrapper-dropdown-3 .dropdown li a').css({ 'color': '#' + fontColor });
    jQuery('div.triangle').css({ 'border-top': '41px solid #' + highlightBackColor });
    jQuery('div.triangle').css({ 'border-right': '40px solid #' + backColor });
    jQuery('div#resource').css({ 'max-height': '41px' });
}

function enableCustomCssGroupMenu(settings) {
    loadColors(settings);
    jQuery('div#group').css({ 'color': '#' + fontColor, 'background-color': '#' + backColor });
    jQuery('li.groupListItem').css({ 'background-color': '#' + backColor });
    jQuery('.wrapper-dropdown-3 .dropdown li a').css({ 'color': '#' + fontColor });
}

function enableCustomCssTimeMenu(settings) {
    loadColors(settings);
    jQuery('div#time').css({ 'color': '#' + fontColor, 'background-color': '#' + backColor });
    jQuery('li.dateListItem').css({ 'background-color': '#' + backColor });
    jQuery('.wrapper-dropdown-3 .dropdown li a').css({ 'color': '#' + fontColor });
}

function enableCustomCssDataTable(settings) {
    loadColors(settings);
    jQuery('th').css({ 'color': '#' + fontColor, 'background-color': '#' + highlightBackColor });
    jQuery('tbody th').css({ 'color': '#' + fontColor });
    jQuery('tbody td').css({ 'color': '#' + fontColor });

    var evenRowColor = colorLuminance("#" + highlightFontColor, -0.15);
    if (evenRowColor == "#000000") {
        evenRowColor = "#696969";
    }

    jQuery('table.display tbody tr:nth-child(even) td').css({ 'background-color': evenRowColor });
    jQuery('table.display tbody tr:nth-child(odd) td').css({ 'background-color': '#' + highlightFontColor });
    jQuery('table#dataTable tbody > tr > td').css({ 'color': '#' + fontColor });
}