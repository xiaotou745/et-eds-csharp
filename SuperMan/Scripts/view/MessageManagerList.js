﻿var adminjs = new adminglass(); //实例化后台类
$(document).ready(function () {
    $('#txtOrderPubStart').datepicker();
    $('#txtOrderPubEnd').datepicker();
    window.location.hash = '';
});