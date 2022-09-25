/*
* FlowType.JS v1.1
* Copyright 2013-2014, Simple Focus http://simplefocus.com/
*
* FlowType.JS by Simple Focus (http://simplefocus.com/)
* is licensed under the MIT License. Read a copy of the
* license in the LICENSE.txt file or at
* http://choosealicense.com/licenses/mit
*
* Thanks to Giovanni Difeterici (http://www.gdifeterici.com/)
*/

(function ($) {
    $.fn.flowtype = function (options) {

        // Calculate scale and change fontratio 
        var width = window.innerWidth;
        var height = window.innerHeight;
        var scale = width / height;
        var fontRatioVariable = 12.5;

        if (scale > 3.0) {
            fontRatioVariable = 19;
        } else if (scale > 2.9) {
            fontRatioVariable = 18;
        } else if (scale > 2.8) {
            fontRatioVariable = 17;
        } else if (scale > 2.7) {
            fontRatioVariable = 16;
        } else if (scale > 2.6) {
            fontRatioVariable = 15;
        }

        // Establish default settings/variables
        // ====================================
        var settings = $.extend({
            maximum: 9999,
            minimum: 1,
            maxFont: 9999,
            minFont: 1,
            fontRatio: fontRatioVariable
        }, options),

  // Do the magic math
  // =================
        changes = function (el) {
            var $el = $(el),
               elw = $el.width(),
               width = elw > settings.maximum ? settings.maximum : elw < settings.minimum ? settings.minimum : elw,
               fontBase = width / settings.fontRatio,
               fontSize = fontBase > settings.maxFont ? settings.maxFont : fontBase < settings.minFont ? settings.minFont : fontBase;
            $el.css('font-size', fontSize + 'px');
        };

        // Make the magic visible
        // ======================
        return this.each(function () {
            // Context for resize callback
            var that = this;
            // Make changes upon resize
            $(window).resize(function () { changes(that); });
            // Set changes on load
            changes(this);
        });
    };
}(jQuery));