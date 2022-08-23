var StyleBackColor;
var StyleFontColor;
var StyleHighlightBackColor;
var StyleHighlightFontColor;
var StyleEvenRowColor;

function loadColors(eventData) {
    StyleBackColor = parseHtmlColor(eventData["styleBackColor"]).colorSubstring;
    StyleFontColor = parseHtmlColor(eventData["styleFontColor"]).colorSubstring;
    StyleHighlightBackColor = parseHtmlColor(eventData["styleHighlightBackColor"]).colorSubstring;
    StyleHighlightFontColor = parseHtmlColor(eventData["styleHighlightFontColor"]).colorSubstring;
    StyleEvenRowColor = colorLuminance("#" + StyleHighlightFontColor, -0.15);
    if (StyleEvenRowColor == "#000000") {
        StyleEvenRowColor = "#212121";
    }
    if (StyleHighlightFontColor = 'FFFFFF') {
        StyleHighlightFontColor = '333333';
        StyleEvenRowColor = colorLuminance("#" + StyleHighlightFontColor, 0.40);
    }
}

function parseHtmlColor(color) {
    var parsed = { opacity: color.substring(0, 2), colorSubstring: color.substring(2, 8) }
    return parsed;
}

function isTransparent(color) {
    return (parseHtmlColor(color).opacity == "00");
}

function initColors() {
    loadColors(eventData);
    if (isTransparent(eventData["styleBackColor"])) {
        $('body').css({ 'background-color': 'transparent'});
    } else {
        $('body').css({ 'background-color': '#' + StyleBackColor, 'background': 'url(LiveTiming/css/img/bg.png) center center repeat'});
    }
}

function initBranding(settings) {
    if (settings.Branding == 'BmiLeisure') {
        document.getElementById('logomobile').src = 'Common/images/logo-leisure.png';
        document.getElementById('logo').src = 'Common/images/logo-leisure.png';
    } else {
        document.getElementById('logomobile').src = 'Common/images/logo-smstiming.png';
        document.getElementById('logo').src = 'Common/images/logo-smstiming.png';
    }
}

function colorLuminance(hex, lum) {

    // validate hex string
    hex = String(hex).replace(/[^0-9a-f]/gi, '');
    if (hex.length < 6) {
        hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
    }
    lum = lum || 0;

    // convert to decimal and change luminosity
    var rgb = "#", c, i;
    for (i = 0; i < 3; i++) {
        c = parseInt(hex.substr(i * 2, 2), 16);
        c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
        rgb += ("00" + c).substr(c.length);
    }

    return rgb;
}