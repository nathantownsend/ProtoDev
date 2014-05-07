/* CONFIRMATION DIALOG
----------------------------------------------------------------*/
// opens an auto-destroying modal dialog that confirm an action wtih callbacks for confirm and cancel events
// title - the dialog title
// message - the message displayed to the user
// confirmCallback - the function to call if they confirm with parameters (confirmationDialog, dataObject)
// cancelCallback - the function to call if they cancel with parameters (confirmationDialog, dataObject)
// dataObject - an object that is passed to the callback functions
function ConfirmationDialog(title, message, isError, confirmCallback, cancelCallback, dataObject) {

    var div;
    if (isError) {
        div = $("<div><strong class='error'>" + message + "</strong></div>");
    } else {
        div = $("<div><strong>" + message + "</strong></div>");
    }
    div.dialog({
        title: title,
        modal: true,
        close: function (event, ui) {
            $(this).dialog("destroy");
        },
        buttons: [
                {
                    text: "Ok", click: function () {
                        if (confirmCallback)
                            confirmCallback(dataObject);
                        $(this).dialog("close");
                    }
                },
                {
                    text: "Cancel", click: function () {
                        if (cancelCallback)
                            cancelCallback(dataObject);
                        $(this).dialog("close");
                    }
                }
        ]
    });

}
/* END CONFIRMATION DIALOG
----------------------------------------------------------------*/





/* FILTER TABLE
----------------------------------------------------------------*/
// uses text from a text box to filter the rows of a table that show based
// created [4/25/2014]

(function ($) {
    jQuery.fn.filterTable = function (options) {

        var table = $(this);

        // default settings
        var settings = {
            input: null,                     // the input text box to use for filtering the table
            trigger: null,                   // the element that gets clicked to trigger the search
            colIndex: 0,                     // the column that the filter will apply to
            hiddenClass: "hide"
        };


        // METHOD DEFINITIOINS
        var methods = {

            // INITIALIZE THE PLUGIN
            init: function (options) {
                // extendable settings
                settings = $.extend(settings, options);

                // define elements
                var input = $(settings.input);
                var trigger = $(settings.trigger);
                var rows = table.find("tbody tr")

                input.keyup(function () {
                    var text = input.val().trim().toLowerCase();
                    rows.each(function () {
                        var row = $(this);
                        var cell = row.find("td").eq(settings.colIndex);
                        if (cell.text().toLowerCase().startsWith(text) || text.length == 0)
                            row.removeClass(settings.hiddenClass);
                        else
                            row.addClass(settings.hiddenClass);
                    });
                });
            },

            // resets the table when the text changes
            reset: function () {
                table.find("tr." + settings.hiddenClass).removeClass(settings.hiddenClass);
            }
        }

        // call the init method with options by default, or whatever method is specified
        if (methods[options]) {
            return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof options === 'object' || !options) {
            // Default to "init"
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + options + ' does not exist on jQuery.filterTable');
        }
    }

}(jQuery));
// END FILTER TABLE





/* MESSAGING
----------------------------------------------------------------*/
var Message = new function () {
    this.Show = function (text, boolSuccess) {

        if (boolSuccess == null)
            boolSuccess = true;

        Notify({
            content: text,
            status: boolSuccess ? NotificationStatus.Success : NotificationStatus.Error
        });
    }
    this.OldShow = function (text, boolSuccess) {

        // defaults to true
        if (boolSuccess == null)
            boolSuccess = true;

        // get a message container 
        var container = $("#NotificationContainer");
        if (container.length == 0) {
            container = $('<div id="NotificationContainer"/>');
            $("body").append(container);
        }

        // setup the message object
        var message = $("<div class='notification'><span class='content'/></div>");
        if (boolSuccess)
            message.addClass("success");
        else
            message.addClass("error");

        // set the text
        message.find(".content").text(text);

        // display the message
        container.append(message);

        // show and hide the message
        message.slideDown(500, function () {
            setTimeout(function () { message.slideUp(500, function () { $(this).remove() }); }, 3000);
        });

    }

    // notifies of success
    this.OK = function (text) {
        Notify({
            content: text
        });
    }

    // notifies of an error
    this.Error = function (text) {
        Notify({
            content: text,
            status: NotificationStatus.Error
        });
    }

    // confirms an action
    this.Confirm = function (text, confirm, cancel, data) {
        Notify({
            content: text,
            status: NotificationStatus.Confirm,
            autoClose: false,
            modal: true,
            data: data,
            confirmCallback: confirm,
            cancel: cancel
        });
    }
}

// Defines the types of messages and the class names that will be applied to their display
var NotificationStatus = {
    Success: "success",
    Error: "error",
    Confirm: "confirm"
}


function Notify(options) {

    var _closed = false;

    // default settings
    var settings = {
        content: "no text specified",           // the content of the message
        status: NotificationStatus.Success,     // the status of the message to display
        title: null,                            // the notification title (null excludes the title element completely)
        autoClose: true,                        // if the message should close by itself or require the user to close it manually
        modal: false,                           // if the message should block the user from performing other actions
        data: {},                               // data that will be passed to the callbacks
        confirmCallback: function (data) { },   // called when the message is confirmed
        cancelCallback: function (data) { },    // called when the message is cancelled
        containerId: "NotificationContainer",   // the id of the container that will house the messages
        transitionSpeed: 500,                   // the speed of the transition in milliseconds
        displayDuration: 3000                   // the total amount of time the message shows
    };

    // merge the options
    settings = $.extend(settings, options);

    // get a message container 
    var container = $("#" + settings.containerId);
    if (container.length == 0) {
        container = $('<div id="' + settings.containerId + '"/>');
        $("body").append(container);
    }

    // setup the message object
    var message = $("<div class='notification'><div class='main'><h3 class='title'/><div class='content'/></div><div class='actions'><button class='ok primary'>OK</button><button class='cancel secondary'>Cancel</button></div>");
    message.addClass(settings.status);

    // the notification title (if present)
    var title = message.find(".title")
    if (settings.title == null)
        title.remove();
    else
        title.text(settings.title);

    // set the text
    message.find(".content").html(settings.content);

    // bind the ok button
    var ok = message.find(".ok");
    ok.click(function () {
        _closed = true;
        message.slideUp(settings.transitionSpeed, function () { message.remove() });
        settings.confirmCallback(settings.data);
    });

    // bind the cancel button
    var cancel = message.find(".cancel");
    cancel.click(function () {
        _closed = true;
        message.slideUp(settings.transitionSpeed, function () { message.remove() });
        settings.cancelCallback(settings.data);
    });

    // if auto close then don't show buttons
    if (settings.autoClose) {
        ok.remove();
        cancel.remove();
    }



    // display the message
    container.append(message);

    // show and hide the message
    message.slideDown(settings.transitionSpeed, function () {

        if (settings.autoClose) {
            setTimeout(function () {
                if (!_closed)
                    message.slideUp(settings.transitionSpeed, function () { $(this).remove() });
            }, settings.displayDuration);
        }


    });

}
// END MESSAGING









/* SELECTABLE TABLE
----------------------------------------------------------------*/
// selectable table plugin [4/7/2014 - Nathan Townsend]
// [4/22/2014] - added the refresh method
(function ($) {
    jQuery.fn.selectableTable = function (options) {

        var container = $(this);
        var body = container.find("tbody");

        // default settings
        var settings = {
            selectedClass: "rowSelected",
            hoverClass: "rowHover",
            rowSelector: "tr",
            onRowSelected: function (row) { },
            onRowHoverOver: function (row) { },
            onRowHoverOut: function (row) { }
        };

        // METHOD DEFINITIOINS
        var methods = {

            // INITIALIZE THE PLUGIN
            init: function (options) {
                // extendable settings
                settings = $.extend(settings, options);

                // row selection
                var rows = body.find(settings.rowSelector);

                rows.each(function () {

                    var row = $(this);

                    // add click functionality
                    row.click(function () {
                        container.find("." + settings.selectedClass).removeClass(settings.selectedClass); // unselect the current
                        row.addClass(settings.selectedClass);
                        container.trigger('BeforeRowSelected');
                        settings.onRowSelected(row);
                        container.trigger('AfterRowSelected');
                    });

                    // add hover functionality
                    row.hover(
                        function () {
                            // hover over
                            row.addClass(settings.hoverClass);
                            settings.onRowHoverOver(row);
                        },
                        function () {
                            // hover out
                            row.removeClass(settings.hoverClass);
                            settings.onRowHoverOut(row);
                        }
                    );
                });
            },

            // REFRESHES THE TABLE
            refresh: function () {
                var current = container.find("." + settings.selectedClass);
                current.trigger("click");
            }
        }

        // call the init method with options by default, or whatever method is specified
        if (methods[options]) {
            return methods[options].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof options === 'object' || !options) {
            // Default to "init"
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + options + ' does not exist on jQuery.selectableTable');
        }
    }
    
}(jQuery));
// END SELECTABLE TABLE





/* STARTS WITH
----------------------------------------------------------------*/
if (typeof String.prototype.startsWith != 'function') {
    String.prototype.startsWith = function (str) {
        return this.slice(0, str.length) == str;
    };
}
// END STARTS WITH






/* TABLE SORTING
----------------------------------------------------------------*/
(function ($) {
    $(document).ready(function () {

        // TABLE SORTER
        $(".tablesorter").each(function () {
            var table = $(this);
            if (table.find("tbody tr").length > 0)
                table.tablesorter();
            //table.tablesorter({ widgets: ['zebra'], widgets: ['staticRow'] });
        });

    });
})(jQuery);
// END TABLE SORTING

