usa = {};
window.usa = usa;
usa.ui = {};
usa.ui.view = {};

(function () {
    window.usa.ui.view.SupplierAccountAdd = SupplierAccountAddClass;

    function SupplierAccountAddClass() {

        var _self = this;
        function _init() {
            _self.init = init;
        }

        function init() {
            initDomEvent();
            initDomElement();
            initValidation();
        }

        function initDomEvent() {
            $("#btn_AddAccount").click(function () {
                var isvalid = $("#AddAccountForm").valid();
                if (isvalid) {
                    $("#AddAccountForm").submit();
                }
            });
        }

        function initDomElement() {
            yg.data.usa.initAreaSelect($("#province_add"), $("#Geographykey"));
        }

        function initValidation() {
            var options = {
                onkeyup: true,
                ignore: [],
                rules: {
                    SupplierName: {required:true,minlength:2,maxlength:50},
                    UserName: { userName: true, required: true, rangelength: [2, 20] },
                    Password: { required: true },
                    Geographykey: { required: true,min:1},
                    Address: { required: true },
                    Email: { email: true },
                    Lat: {required: true,number:true},
                    Long: { required: true ,number:true}
                },
                messages: {
                    Geographykey: { min: "请选择餐厅所在区域！" }
                }
            };

            $("#AddAccountForm").validate(options);
        }
        _init();
    }

})();