function parseHtmlColor(color) {
    var parsed = { opacity: color.substring(0, 2), colorSubstring: color.substring(2, 8) }
    return parsed;
}

function isTransparent(color) {
    return (parseHtmlColor(color).opacity == "00");
}

function enableDropDownMenu(elementName) {
    function DropDown(el) {
        this.dd = el;
        this.placeholder = this.dd.children('span');
        this.opts = this.dd.find('ul.dropdown > li');
        this.val = '';
        this.index = -1;
        this.initEvents();
    }
    DropDown.prototype = {
        initEvents: function () {
            var obj = this;

            obj.dd.on('click', function (event) {

                $('.wrapper-dropdown-3').removeClass('clicked');

                if ($(this).hasClass('active')) {
                    $('.wrapper-dropdown-3').removeClass('active');
                } else {
                    $('.wrapper-dropdown-3').removeClass('active');
                    if ($(this).hasClass('soloDropdown')) {
                        return;
                    } else {
                        $(this).toggleClass('active');
                        $(this).toggleClass('clicked');
                    }
                }
                return false;
            });

            changeSpanName(obj);

            obj.dd.on('hover', function() {
                
            });
        },
        setValue: function(active) {
            this.val = active;
        },
        getValue: function () {
            return this.val;
        },
        getIndex: function () {
            return this.index;
        }
    }

    $(function () {
        var dropDown = new DropDown($(elementName));
        
        $(document).click(function () {
            $(".wrapper-dropdown-3").removeClass('active');
            $(".wrapper-dropdown-3").removeClass('clicked');
        });
    });
}

function enableDataTable() {
    /*for additional settings go to www.datatables.net/*/
    var table = $('#dataTable').DataTable({
        "paging": false,
        "info": false,
        "searching": false,
        "bSort": false,
        "autoWidth": true,
        "columnDefs": [
            { "width": "10%", "targets": 0 }
        ],
        responsive: {
            details: false
        }
    });
}

function changeSpanName(obj) {
    obj.opts.on('click', function () {
        var opt = $(this);
        obj.val = opt.text();
        obj.index = opt.index();
        obj.placeholder.text(obj.val);
        opt.text = obj.val;
    });
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

function enableGradient(divId) {

    jQuery('div#' + divId).css({
        'background-image': '-moz-linear-gradient(left, rgba(0, 0, 0, 0), rgba(204, 204, 204, 0.4))',
        'background-image': '-ms-linear-gradient(left, rgba(0, 0, 0, 0), rgba(204, 204, 204, 0.4))',
        'background-image': '-webkit-gradient(linear, 150 30, 130% 30, from(rgba(0, 0, 0, 0)), to(rgba(204, 204, 204, 0.4)))',
        'background-image': '-webkit-linear-gradient(left, rgba(0, 0, 0, 0), rgba(204, 204, 204, 0.4))',
        'background-image': '-o-linear-gradient(left, rgba(0, 0, 0, 0), rgba(204, 204, 204, 0.4))',
        'background-image': 'linear-gradient(left, rgba(0, 0, 0, 0), rgba(204, 204, 204, 0.4));',
        'background-repeat': 'repeat-x',
        'filter': 'progid:DXImageTransform.Microsoft.gradient(startColorstr="rgba(0, 0, 0, 0)", endColorstr="rgba(204, 204, 204, 0.4)", GradientType=1)'
    });
}