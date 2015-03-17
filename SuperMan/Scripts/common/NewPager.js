/// <reference path="_references.views.js" />
/// <reference path="../webExpress/webExpress.ui.control.adapter.js" />

(function () {
    art.ui.control.NewPager = PagerClass;

    art.ui.control.NewPager.enablePaging = function (pagerContainer, redirectToPage) {
        $(pagerContainer).on("click", ".grid-pager a[pageIndex]", function () {
            var pageIndex = $(this).attr("pageIndex");
            redirectToPage(pageIndex);
        });

        $(pagerContainer).on("click", ".t-arrow-prev", function () {
            var pageIndex = parseInt(getCurrentPageIndex()) - 1;
            redirectToPage(pageIndex);
        });

        $(pagerContainer).on("click", ".t-arrow-next", function () {
            var pageIndex = parseInt(getCurrentPageIndex()) + 1;
            redirectToPage(pageIndex);
        });
        $(pagerContainer).on("click", ".t-arrow-last", function () {
            var pageIndex = parseInt($(this).attr("pageIndex"));
            redirectToPage(pageIndex);
        });
        $(pagerContainer).on("click", ".t-arrow-first", function () {
            redirectToPage(1);
        });
        $(pagerContainer).on("click", ".t-refresh", function () {
            redirectToPage();
        }); 
        $(pagerContainer).on("keyup", ".ipw_topage", function () {
            // var input = this;
            InitIpw_topage();
        });
        $(pagerContainer).on("click", "#_btn_topage", function () {
            var toPage = $(".ipw_topage").val();
            redirectToPage(toPage);
        });
        function getCurrentPageIndex() {
            return $(".t-state-active", pagerContainer).text();
        }
        function InitIpw_topage() {
            var input = $(".ipw_topage");
            $(".ipw_topage").val($(".ipw_topage").val().replace(/[^\d]/g, ''));
            var _pageIndex= parseInt(input.val());
            var _maxPage = parseInt($(input).attr("maxpage"));

            if (_pageIndex > _maxPage) {
                $(".ipw_topage").val(_maxPage);
            }
        }
    }

    function PagerClass(pagerContainer, redirectToPage) {
        var _self = this;

        function _init() {
            init(pagerContainer, redirectToPage);
        }

        function init() {
            $(pagerContainer).on("click", ".grid-pager a[pageIndex]", function () {
                var pageIndex = $(this).attr("pageIndex");
                refresh(pageIndex);
            });

            $(pagerContainer).on("click", ".grid-pager a[pageIndex]", function () {
                var pageIndex = $(this).attr("pageIndex");
                redirectToPage(pageIndex);
            });

            $(pagerContainer).on("click", ".t-arrow-prev", function () { 
                var pageIndex = parseInt(_searchCriteria.PagingRequest.PageIndex) - 1;
                redirectToPage(pageIndex);
            });

            $(pagerContainer).on("click", ".t-arrow-next", function () {
                var pageIndex = parseInt(_searchCriteria.PagingRequest.PageIndex) + 1;
                redirectToPage(pageIndex);
            });
            $(pagerContainer).on("click", ".t-arrow-last", function () {
                var pageIndex = parseInt($(this).attr("pageIndex"));
                redirectToPage(pageIndex);
            });
            $(pagerContainer).on("click", ".t-arrow-first", function () {
                redirectToPage(1);
            });
            $(pagerContainer).on("click", ".t-refresh", function () {
                redirectToPage();
            });

        }

        _init();
    }
})();