var button_logout1 = document.getElementById('a_logout');

if (button_logout1 != undefined) {
    var slideout = document.getElementById('notif');
    var button_logout2 = document.getElementById('btn_logout')
    var button_cancel = document.getElementById('btn_cancel')

    button_logout1.onclick = function () {
        slideout.classList.toggle('visible');
    };
    button_logout2.onclick = function () {
        window.location.replace("/Account/Logout");
    };
    button_cancel.onclick = function () {
        //slideout.classList.toggle('invisible');
        slideout.classList.toggle('visible');
    };
}
