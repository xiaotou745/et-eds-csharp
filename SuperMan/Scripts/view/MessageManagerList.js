var adminjs = new adminglass(); //实例化后台类
$(document).ready(function () {
    $('#txtPubDateStart').datepicker();
    $('#txtPubDateEnd').datepicker();
    window.location.hash = '';
});