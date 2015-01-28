usa = {};
window.usa = usa;
usa.ui = {};
usa.ui.view = {};


(function () {
    window.usa.ui.view.SupplierEditJS = SupplierEditClass;

    function SupplierEditClass() {

        var formId = "#SupplierEditForm";
        var btnSubmitId = ".btn-edit-submit";
        var provinceId = "#province_add";
        var geographyKey = "#Geographykey";

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
            //submit form
            $(btnSubmitId).click(function () {
                
                var isvalid = $(formId).valid();
                
                if (isvalid) {
                    $(formId).submit();
                }
            });
        }

        function initDomElement(){
            // init area 
            yg.data.usa.initAreaSelect($(provinceId), $(geographyKey));

            $(".timepicker").datetimepicker();
        }

        function initValidation() {
            var options = {
                onkeyup: false,
                ignore: [],
                rules: {
                    Geographykey: { required: true, min: 1 },
                    Address: { required: true },
                    ContactPerson: { required: true, minlength: 2, maxlength: 50 },
                    ContactSex: { required: true },
                    DeliverMinAmount: { required: true },
                    DeliverDistanceMiles: { required: true },
                    DeliverDistanceMetre: { required: true },
                    PackagingAmount: { required: true },
                    DeliveryTime: { required: true },
                    ContractNO: { required: true },
                    HealthPermitNo: { required: true },
                    opentimestar: { required: true },
                    opentimeend: { required: true }
                },
                messages: {
                    ContactPerson: { required: "请填写餐厅联系人" },
                    ContactSex: { required: "性别不能空" },
                    DeliverMinAmount: { required: "起送价格不能空！" },
                    DeliverDistanceMiles: {required:"配送范围不能空"}

                }
            };

            $(formId).validate(options);
        }

        _init();
    }
})();