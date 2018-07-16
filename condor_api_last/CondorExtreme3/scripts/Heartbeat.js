var keepSessionAlive = false;
var keepSessionAliveUrl = null;

function SetupSessionUpdater(actionUrl) {
    keepSessionAliveUrl = actionUrl;
    keepSessionAlive = true;
    CheckToKeepSessionAlive();
}

function CheckToKeepSessionAlive() {
    setTimeout("KeepSessionAlive()", 10000);
}

function KeepSessionAlive() {
    if (keepSessionAlive && keepSessionAliveUrl != null) {
        $.ajax({
            type: "POST",
            url: keepSessionAliveUrl,
            success: function () { keepSessionAlive = false; }
        });
    }
    CheckToKeepSessionAlive();
}
