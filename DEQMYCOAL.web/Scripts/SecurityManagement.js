var activePanel = 0;

// initialize the Security / User Management screen
$(document).ready(function () {

    InitializeUserTable()

    InitializeMyInfoPanel();

    InitializeSecurityPanel();
});


// attaches javascript to run the user table
function InitializeUserTable() {
    
    $("#RegisteredUsersTable").selectableTable({
        onRowSelected: UserRowSelected
    });

    $("#RegisteredUsersTable").filterTable({
        input: $("#lookup"),
        trigger: $("#lookup-button")
    });
}

// loads user data when selected in the user table
function UserRowSelected(tr) {

    // remember the active panel
    activePanel = $("#AccountAccordion").accordion("option", "active");

    // reload the security panel info
    var userToken = tr.attr("id");
    var url = SecuritySettings.SecurityPanelUrl + "/?userToken=" + userToken;
    $("#SecurityPanel").load(url, function () {
        InitializeSecurityPanel(); // reattach events to newly loaded panel
    });
}





// binds events for managing my info
function InitializeMyInfoPanel() {
    // use ajax to submit the form
    $("#MyInfoForm").unbind("submit").bind("submit", SubmitMyInfo);

    // reveal the current employment type
    RevealEmploymentTypeDescription($("#EmploymentType"));

    // display the 
    $("#EmploymentType").unbind("change").bind("change", function () {
        RevealEmploymentTypeDescription(this);
    });
}

// submits changes to my info panel
function SubmitMyInfo(event) {
    event.preventDefault();

    AjaxRequest({
        url: $(this).attr("action"),
        data: $(this).serialize(),
        successCallback: function () { UpdateTableInfo(); RefreshMyInfoPanel(); },
        htmlCallback: function (html) {
            $("#myInfoPanel").html("");
            $("#myInfoPanel").append(html);
            InitializeMyInfoPanel();
        }
    });
}

// for owner, shows updates to the user's information in the user table table
function UpdateTableInfo() {
       
    var tr = $("tr.rowSelected");
    if (tr.length == 0)
        return; // the table is only available to owner

    var name = $("#Registration_FirstName").val() + " " + $("#Registration_LastName").val();
    var email = $("#Registration_Email").val();
    var company = $("#Registration_CompanyName").val();

    var tds = tr.find("td");
    tds.eq(0).text(name);
    tds.eq(1).text(email);
    tds.eq(2).text(company);

}


// reloads the my info panel from the server
function RefreshMyInfoPanel() {
    var id = $("#Registration_RegistrationID").val();
    var url = SecuritySettings.MyInfoPanel + "?RegistrationId=" + id;
    $("#myInfoPanel").load(url, InitializeMyInfoPanel);
}


// show the text box for Other Company Type in My Info panel
function RevealEmploymentTypeDescription(elem) {
    var desc = $("#Registration_RegistrationDescription");
    var value = $(elem).val();
    if (value == "Other")
        desc.removeClass("hide");
    else
        desc.val(value).addClass("hide");
}






// attaches javascript events to the security panel controls
function InitializeSecurityPanel() {
    // setup accordions
    $("#AccountAccordion").accordion({ heightStyle: "content", active: activePanel });
    $("#PermitAccordion").accordion({ heightStyle: "content", collapsible: true, active: false, beforeActivate: LoadPermitRegistration });

    // bind events
    $("#AddNewPermitRegistration").unbind('click.AddPermission').bind('click.AddPermission', AddPermissions);
    $("button[data-delete-permission]").unbind('click.DeletePermission').bind('click.DeletePermission', DeletePermission);
    $("#RequestPermitAccess").unbind("click").bind("click", RequestPermitAccess);
    $("#PermissionForm").unbind("submit").bind("submit", UpdateSystemRoles);

    InitializeMyInfoPanel();
}

// sends data to the server to create a new permit registration
function AddPermissions(event) {

    // prevent the button from posting the page
    event.preventDefault();

    // build data object
    var tr = $("#NewPermitRegistrationForm");
    var data = {
        RegistrationID: tr.find("#RegistrationID").val(),
        PermitKey: tr.find("#PermitKey").val(),
        Read: tr.find("#Read").prop("checked"),
        Edit: tr.find("#Edit").prop("checked"),
        Coordinator: tr.find("#Coordinator").prop("checked")
    };

    // permit key is required
    if (data.PermitKey == "") {
        Message.Show("You must choose a site name", false);
        return;
    }

    // post the data
    AjaxRequest({
        url: SecuritySettings.NewPermitRegistrationUrl,
        data: data,
        successCallback: RefreshPermitsTable
    });
}

// refreshes the permits table after a new registration has been added
function RefreshPermitsTable() {
    $("#RegisteredUsersTable").selectableTable("refresh");
}

// confirms permission deletion
function DeletePermission() {
    ConfirmationDialog("Confirm Delete", "Are you sure you want to delete this permission?", false, DeletePermissionConfirmed, null, $(this))
}

// deletes a permission
function DeletePermissionConfirmed(button) {
    var id = button.attr("data-delete-permission");
    var url =  SecuritySettings.DeletePermitRegistration + "/" + id;

    AjaxRequest({
        url: url,
        successCallback: function (result) {
            // permit coordinators will refresh their permit panel whereas owners will refresh the entire myPermit table
            if (button.hasClass("coordinator"))
                RefreshCoordinatorPanel(button);
            else
                RefreshPermitsTable();
        }
    });
}

// handles the callback from the permision delete operation
function PermissionsDeleted(response) {

    var result = new AjaxResult(response);

    if (result.Successful) {
        RefreshPermitsTable()
        // show confirmation
        Message.Show(result.Message);
    } else {
        Message.Show(result.Message, false);
    }
}

// handles the callback from the permision update operation
function PermissionsUpdated(response) {

    var result = new AjaxResult(response);

    if (result.Successful) {
        // show confirmation
        Message.Show(result.Message);

        // update the table row with the new data
        var data = result.Data.PermitRegistration;
        var id = data.PermitRegistrationID;
        var tr = $("tr[data-permit-registration-id='" + id + "']");
        tr.find("input[name='read']").prop('checked', data.Read);
        tr.find("input[name='edit']").prop('checked', data.Edit);
        // coordinators don't display a coordinator checkbox, only the owner can seteup a new coordinator
        var coordinator = tr.find("input[name='coordinator']")
        if(coordinator.length > 0)
            coordinator.prop('checked', data.Coordinator);
    } else {
        Message.Show(result.Message, false);
    }
}

// refreshes coordinator panel
function RefreshCoordinatorPanel(button) {
    var id = button.attr("data-delete-permission");
    var table = button.parents("table:first");
    var tr = table.find("tr[data-permit-registration-id='" + id + "']");
    tr.remove();
}

// triggered before a permit registration panel is expanded, used to load permit registrations for a coordinator
function LoadPermitRegistration(event, ui) {
    var div = $(ui.newPanel);
    var key = div.attr("data-permit-key");
    var url = SecuritySettings.MyPermitsCoordinatorPermitPanel + "?PermitKey=" + key;
    div.load(url, function () {
        $("button[data-delete-permission]").unbind('click.DeletePermission').bind('click.DeletePermission', DeletePermission);
        $("#UpdatePermitAccessCode").unbind('click').bind('click', UpdatePermitAccessCode);
    });
}

// updates the permit access code 
function UpdatePermitAccessCode() {
        
    var input = $(this).prev("input");
    var oldCode = input.attr("data-permit-access-code");
    var newCode = input.val().trim();
    if (newCode == "") {
        Message.Show("You  must enter an Access Code", false);
        return;
    }

    AjaxRequest({
        url: SecuritySettings.UpdatePermitAccessCode,
        data : {oldAccessCode: oldCode, newAccessCode: newCode}
    });
}

// sends a request to join a permit
function RequestPermitAccess() {
    AjaxRequest({
        url: SecuritySettings.RequestPermitAccess,
        data: { PermitAccessCode: $(this).prev("input").val() },
        successCallback: function (result) {
            var tr = $("<tr><td/><td><input type='checkbox' disabled='disabled'/></td><td><input type='checkbox' disabled='disabled'/></td></tr>");
            tr.find("td").eq(0).text(result.Data.SiteName);
            $("#myPermitsUser tbody").append(tr);
        }
    });
}




// updates the systm roles in the Permission and Access panel
function UpdateSystemRoles(event) {
    event.preventDefault();

    AjaxRequest({
        url: $(this).attr("action"),
        data: $(this).serialize(),
        successCallback: function (response) {
            var status = $("#Registration_Registration_RegistrationStatusID").val();
            $("#RegisteredUsersTable tr.rowSelected td").eq(3).text(status);
        },
        htmlCallback: function (html) {
            $("#PermissionPanel").html("");
            $("#PermissionPanel").append(html);
        }
    });
}
