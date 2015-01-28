/// <reference path="/_references.js" />
(function () {
    webExpress.utility.ajax = new AjaxClass();
    function AjaxClass() {
        var _self = this;

        function _init() {
            _self.request = request;
        }

        function request(url, data, successHandler, errorHandler, options) {
            var options = getAjaxOptions(url, data, successHandler, errorHandler, options);
            var jqXHR = $.ajax(options);
            return jqXHR;
        }

        function getAjaxOptions(url, data, successHandler, errorHandler, options) {
            var ajaxOptions = {
                type: "post",
                contentType: "application/json",
                data: data,
                url: url,
                success: successHandler,
                error: errorHandler
            };
            if (options !== undefined) {
                $.extend(ajaxOptions, options);
            }
            if (ajaxOptions.contentType == "application/json" && ajaxOptions.data != null && typeof data !== "function") {
                ajaxOptions.data = JSON.stringify(data);
            }
            return ajaxOptions;
        }

        _init();
    }
})();