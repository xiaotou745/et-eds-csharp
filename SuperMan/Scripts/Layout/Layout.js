
$('#menu-toggler').on("click", function () {
    $('#sidebar').toggleClass('display');
    $(this).toggleClass('display');
    return false;
});
//mini
var $minimized = $('.sidebar').hasClass('menu-min');
$('#sidebar-collapse').on("click", function () {
    $minimized = $(".sidebar").hasClass('menu-min');

    var collpase = !$minimized || false;

    var sidebar = document.getElementById('sidebar');
    var icon = document.getElementById('sidebar-collapse').querySelector('[class*="icon-"]');
    var $icon1 = icon.getAttribute('data-icon1');//the icon for expanded state
    var $icon2 = icon.getAttribute('data-icon2');//the icon for collapsed state

    if (collpase) {
        $(".sidebar").addClass("menu-min");
        $(icon).removeClass($icon1);
        $(icon).addClass($icon2);

        //ace.settings.set('sidebar', 'collapsed');
    } else {
        $(".sidebar").removeClass("menu-min");
        $(icon).removeClass($icon2);
        $(icon).addClass($icon1);

        //ace.settings.unset('sidebar', 'collapsed');
    }
});

//opening submenu
$('.nav-list').on("click", function (e) {
    //check to see if we have clicked on an element which is inside a .dropdown-toggle element?!
    //if so, it means we should toggle a submenu
    var link_element = $(e.target).closest('a');
    if (!link_element || link_element.length == 0) return; //if not clicked inside a link element

    $minimized = $('#sidebar').hasClass('menu-min');

    if (!link_element.hasClass('dropdown-toggle')) { //it doesn't have a submenu return  
        return;
    }
    //
    var sub = link_element.next().get(0);

    //if we are opening this submenu, close all other submenus except the ".active" one
    if (!$(sub).is(':visible')) { //if not open and visible, let's open it and make it visible
        var parent_ul = $(sub.parentNode).closest('ul');
        if ($minimized && parent_ul.hasClass('nav-list')) return;

        parent_ul.find('> .open > .submenu').each(function () {
            //close all other open submenus except for the active one
            if (this != sub && !$(this.parentNode).hasClass('active')) {
                $(this).slideUp(200).parent().removeClass('open');

                //uncomment the following line to close all submenus on deeper levels when closing a submenu
                //$(this).find('.open > .submenu').slideUp(0).parent().removeClass('open');
            }
        });
    } else {
        //uncomment the following line to close all submenus on deeper levels when closing a submenu
        //$(sub).find('.open > .submenu').slideUp(0).parent().removeClass('open');
    }

    if ($minimized && $(sub.parentNode.parentNode).hasClass('nav-list')) return false;

    $(sub).slideToggle(200).parent().toggleClass('open');
    return false;
});



function select(li) {
    var menu = $(li).closest("ul.submenu").closest("li");
    menu.addClass("open");
    menu.addClass("active");
    menu.show();

    $(li).eq(0).addClass("active");
}

function initLayout() {
    var pathname = location.pathname;
    var currentMenuItem = $("a[href='" + pathname + "']").closest("li");
    if (currentMenuItem.length == 0) {
        var curPathName = pathname.split('/')[1];
        currentMenuItem = $("a[data-url-def='" + curPathName + "']").closest("li");
        $("a[data-url-def='" + curPathName + "']").eq(0).addClass("cur");
    } else {
        $("a[href='" + location.pathname + "']").addClass("cur");
    }
    select(currentMenuItem);
    
}

initLayout();