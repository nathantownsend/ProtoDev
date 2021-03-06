/*Nav Tabs*/
$(function () {
    $("#tabs").tabs();
});

/*Primary Nav Tabs*/
var strparentcategory = getparentcategory();
	
function getparentcategory() {
    var url = window.location;
    var urlstring = String(url);

    if (urlstring.indexOf("/ePermit")>0) {
        return "epermit";
    }
    else if (urlstring.indexOf("/ePermitAdmin/")>0) {
        return "epermit";
    }
    else if (urlstring.indexOf("/BondEstimates/")>0) {
        return "bond";
    }
    else if (urlstring.indexOf("/Inspections/")>0) {
        return "inspection";
    }
    else if (urlstring.indexOf("/CoalPC/")>0) {
        return "coalpc";
    }
}
function styleparentcategory() {
        var x = $("#tab" + strparentcategory)[0];
        var y = $("#tab" + strparentcategory + "a")[0];
        if (x && y){
        x.style.backgroundImage="none";
        x.style.backgroundColor="#B3C7CE";
        x.style.borderBottom="1px solid #B3C7CE";
        y.style.color = "#555555";
}
}

/*Sub Navigation*/
    var strsubnav = getsubnav();

     function getsubnav() {
        var url = window.location;
        var urlstring = String(url);

        if (urlstring.indexOf("/ePermitAdmin/") > 0) {
            return "admin";
        }
        else if (urlstring.indexOf("/ePermitBaseline/") > 0) {
            return "baseline";
        }
        else if (urlstring.indexOf("/ePermitOperations/") > 0) {
            return "operations";
        }
        else if (urlstring.indexOf("/ePermitReclamation/") > 0) {
            return "reclamation";
        }
        else if (urlstring.indexOf("/ePermitMaps/") > 0) {
            return "maps";
        }
        else if (urlstring.indexOf("/ePermitPerformanceStandards/") > 0) {
            return "performance";
        }
    }   
     function stylesubnav() {
         var z = $("ul.nav2." + strsubnav).parent().find("a").first().click();        
    }

$(document).ready(function () {
    $(".nav1 > li > a").on("click", function (event) {
        if ($(this).parent().has("ul")) {
            event.preventDefault();
        }

        if (!$(this).hasClass("open")) {
            // hide any open menus and remove all other classes
            $(".nav1 li ul").slideUp(500);
            $(".nav1 li a").removeClass("open");

            // open our new menu and add the open class
            $(this).next("ul").slideDown(500);
            $(this).addClass("open");
        }

        else if ($(this).hasClass("open")) {
            $(this).removeClass("open");
            $(this).next("ul").slideUp(500);
        } event.returnValue
    });
});

function styleactivemenu() {
    var activemenu = $("article > h2").data("submenu");
    $(".nav1 a.open").next("ul").find("a").get(activemenu).className="highlight";
}

$(document).ready(function () {
    styleparentcategory();
    stylesubnav();
    styleactivemenu();
});

/*datepicker*/
$(function () {
    $(".datepicker").datepicker({
        showOn: "button",
        buttonImage: "../Images/calendar.gif",
        buttonImageOnly: true
    });
});

/*accordion*/
/*
$(function () {
    $("#accordion").accordion({
        collapsible: true
        //active: false
       // autoHeight: true,
       // clearStyle: true
    });
});
*/
/*change accordion icons for Performance Standards*/
$(function () {
    $("h3.iconswitch span:first-of-type").css("right", ".5em").css("left", "auto");
});

/*multiple panels open*/
/*
$(function () {
    $("#notaccordion").addClass("ui-accordion ui-accordion-icons ui-widget ui-helper-reset")
       .find("h3")
         .addClass("ui-accordion-header ui-helper-reset ui-state-default ui-corner-top ui-corner-bottom")
         .prepend('<span class="ui-icon-triangle-1-e"/>')
         .click(function () {
             $(this)
             //.toggleClass("ui-accordion-header-active ui-state-active ui-state-default ui-corner-bottom")
             .find("> .ui-icon").toggleClass("ui-icon-triangle-1-e").end()
             .next().toggleClass("ui-accordion-content-active").slideToggle();
         })
         .next()
             .addClass("ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom")
             .hide();
});

*/

/*
$(document).ready(function () {
    $("button.modal1").click(function () {
        $("div.dialogbox1").dialog({ height: "auto", maxHeight: 100, modal: "true", title: $(this).attr("title") });
    });

    $("button.modal2").click(function () {
        $("div.dialogbox2").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal3").click(function () {
        $("div.dialogbox3").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal4").click(function () {
        $("div.dialogbox4").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal5").click(function () {
        $("div.dialogbox5").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal6").click(function () {
        $("div.dialogbox6").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal7").click(function () {
        $("div.dialogbox7").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal8").click(function () {
        $("div.dialogbox8").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal9").click(function () {
        $("div.dialogbox9").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal10").click(function () {
        $("div.dialogbox10").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal11").click(function () {
        $("div.dialogbox11").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal12").click(function () {
        $("div.dialogbox12").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal13").click(function () {
        $("div.dialogbox13").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal14").click(function () {
        $("div.dialogbox14").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal15").click(function () {
        $("div.dialogbox15").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal16").click(function () {
        $("div.dialogbox16").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal17").click(function () {
        $("div.dialogbox17").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal18").click(function () {
        $("div.dialogbox18").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal19").click(function () {
        $("div.dialogbox19").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal20").click(function () {
        $("div.dialogbox20").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal21").click(function () {
        $("div.dialogbox21").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal22").click(function () {
        $("div.dialogbox22").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });

    $("button.modal23").click(function () {
        $("div.dialogbox23").dialog({ height: "auto", modal: "true", title: $(this).attr("title") });
    });
});

*/

/*dialog box can be moved, closed, resized*/
/*
$(function() {
    $( ".dialog" ).dialog({
        autoOpen: false        
    });
    $( ".opener" ).click(function() {
        $( ".dialog" ).dialog( "open" );
    });
});
*/
//$('a.opener').click(function (ev) {
//    window.open('/Shared/_helpwindow','width=500','height=600','resizable=Yes','scrollbars=Yes','menubar=no','toolbar=no','');
//    ev.preventDefault();
//    return false;
//})
//function winClick() {
//    var w = window.open();
//    var html = $(".dialog").html();
//    $(w.document.body).html(html);
//}
//$(function () {
//    $("a.opener").click(winClick);
//});
//$('a.opener').click(function () {
//    window.open('/Shared/_helpwindow', '_blank', 'width=500,height=600','resizable=Yes','scrollbars=Yes','menubar=no','toolbar=no');
//    return false;
//});

/*show/hide content*/
$(document).ready(function () {
    $("button#ShowComments").click(function () {
        $("#Comments").toggle();
    });
});
$(document).ready(function () {
    $("button.ShowComments").click(function () {
        $(".Comments").toggle();
    });
});
$(document).ready(function () {
    $("button.Open").click(function () {
        $(".Checklist").toggle();
    });
});

$(document).ready(function () {
    $("a.Show").click(function () {
        $(".content").toggle();
    });

    $(".selectOpt").change(function () {
        if ($(this).val() == "Other") {
            $(".describeEntity").show();
        }
        else
            $(".describeEntity").hide();
    });
});
/*Alternate BG color for table rows on Completeness*/
$(document).ready(function () {
    $("table.alternate tr:even").css("background-color", "#f3f3f3");
    
/*Show More Text...*/
$(".show-more a").on("click", function () {
    var $link = $(this);
    var $content = $link.parent().prev("div.text-content");
    var linkText = $link.text().toUpperCase();

    switchClasses($content);

    $link.text(getShowLinkText(linkText));

    return false;
});

function switchClasses($content) {
    if ($content.hasClass("short-text")) {
        $content.switchClass("short-text", "full-text", 400);
    } else {
        $content.switchClass("full-text", "short-text", 400);
    }
}

function getShowLinkText(currentText) {
    var newText = '';

    if (currentText === "SHOW MORE") {
        newText = "Show less";
    } else {
        newText = "Show more";
    }

    return newText;
}
});

function ToggleChildren(button, rule) {

    var text = $(button).text();
    if (text == "+")
        text = "-";
    else
        text = "+";
    $(button).text(text);

    $("tr[data-section='" + rule + "']").each(function () {
        if ($(this).hasClass("Collapsed")) {
            $(this).removeClass("Collapsed");
        } else {
            $(this).addClass("Collapsed"); // collapse the child rule
            $(this).next().addClass("Collapsed"); // collapse the description
        }
    });
}
(function ($) {
    $(document).ready(function () {
        $(".Expander").click(function (event) {
            // show or hide the the next row beneath the parent of this row
            var row = $(this).parents("tr:first");
            var next = row.next();
            if (next.hasClass("Collapsed"))
                next.removeClass("Collapsed");
            else
                next.addClass("Collapsed");
        });
    });
})(jQuery);




//$(".show-more a").on("click", function () {
//    var $link = $(this);
//    var $content = $link.parent().prev("div.text-content");
//    var linkText = $link.text().toUpperCase();

//    $content.toggleClass("short-text, full-text");

//    $link.text(getShowLinkText(linkText));

//    return false;
//});

//function getShowLinkText(currentText) {
//    var newText = '';

//    if (currentText === "SHOW MORE") {
//        newText = "Show less";
//    } else {
//        newText = "Show more";
//    }

//    return newText;
//}

//$(document).ready(function(){
//    $(".Comments").hide();
//    $('.CommentsButton').click(function(){
//        $(this).next().slideToggle();
//    });
//});

/*Tool Tips*/
//$(function () {
//    var tooltips = $("article input[title]").tooltip();
//    $("a#tips")
//      .click(function () {
//          tooltips.tooltip("open");
//      })
//});