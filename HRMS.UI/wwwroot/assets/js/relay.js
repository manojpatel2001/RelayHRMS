
        // Your consolidated JavaScript here
    $(document).ready(function () {
        let currentUrl = window.location.href.toLowerCase();
    let $activeLink;

    // Loop through all links in navbar
    $('.main-navbar .nav-link, .main-navbar .dropdown-item').each(function () {
        let linkUrl = this.href.toLowerCase();

    if (currentUrl === linkUrl || currentUrl.startsWith(linkUrl)) {
        $('.main-navbar .nav-link').removeClass('active');
    $('.main-navbar .dropdown-item').removeClass('active');

    $(this).addClass('active');
                    $(this).closest('.nav-item').find('> .nav-link').addClass('active');
    $activeLink = $(this); // Store active link
    return false;
                }
            });

    // Handle hover to temporarily remove active background
    $('.main-navbar .nav-link').hover(
    function () {
        $('.main-navbar .nav-link.active').addClass('temp-active').removeClass('active');
                },
    function () {
        $('.main-navbar .nav-link.temp-active').addClass('active').removeClass('temp-active');
                }
    );

    // Prevent submenu from showing by default
    $('.submenu').hide();

    // Toggle submenu on click
    $('.submenu-toggle').on('click', function (e) {
        e.preventDefault();
    let $submenu = $(this).next('.submenu');

    // Close other submenus
    $('.submenu').not($submenu).slideUp('fast');

    // Toggle this submenu
    $submenu.stop(true, true).slideToggle('fast');
            });

    // Optional: hover support (works with .hover() OR CSS)
    $('.dropdown-submenu').hover(
    function () {
        $(this).find('.submenu').stop(true, true).slideDown(200);
                },
    function () {
        $(this).find('.submenu').stop(true, true).slideUp(200);
                }
    );

    // Close on outside click
    $(document).click(function (e) {
                if (!$(e.target).closest('.dropdown-submenu').length) {
        $('.submenu').slideUp('fast');
                }
            });
        });

    // Toggle user dropdown
    function toggleUserDropdown() {
            const dropdown = document.getElementById('userDropdown');
    const chevron = document.getElementById('userChevron');

    dropdown.classList.toggle('active');
    chevron.style.transform = dropdown.classList.contains('active') ? 'rotate(180deg)' : 'rotate(0deg)';
        }

    // Close user dropdown when clicking outside
    document.addEventListener('click', function(event) {
            const userSection = document.querySelector('.user-section');
    const dropdown = document.getElementById('userDropdown');
    const chevron = document.getElementById('userChevron');

    if (!userSection.contains(event.target)) {
        dropdown.classList.remove('active');
    chevron.style.transform = 'rotate(0deg)';
            }
        });

    // Toggle sidebar
    function toggleSidebar() {
            const sidebar = document.getElementById('mobileSidebar');
    const overlay = document.getElementById('overlay');
    sidebar.classList.toggle('active');
    overlay.classList.toggle('active');
        }

        // Close sidebar when clicking on a mobile nav link without submenu
        document.querySelectorAll('.mobile-nav-link').forEach(link => {
            if (!link.querySelector('.mobile-toggle-icon')) {
        link.addEventListener('click', () => {
            toggleSidebar();
        });
            }
        });

        // Close sidebar when clicking on submenu items
        document.querySelectorAll('.mobile-submenu-item, .mobile-subsubmenu-item, .mobile-subsubsubmenu-item').forEach(item => {
        item.addEventListener('click', () => {
            toggleSidebar();
        });
        });

    // Toggle mobile submenu
    function toggleMobileSubmenu(event, submenuId) {
        event.preventDefault();
    event.stopPropagation();

    const submenu = document.getElementById(submenuId);
    const icon = event.currentTarget.querySelector('.mobile-toggle-icon');

            // Close all other main submenus and their nested children
            document.querySelectorAll('.mobile-submenu').forEach(menu => {
                if (menu.id !== submenuId) {
        menu.classList.remove('active');
                    // Close all nested submenus
                    menu.querySelectorAll('.mobile-subsubmenu, .mobile-subsubsubmenu').forEach(nestedMenu => {
        nestedMenu.classList.remove('active');
                    });
                    // Reset all nested icons
                    menu.querySelectorAll('.submenu-toggle-icon, .subsubmenu-toggle-icon').forEach(nestedIcon => {
        nestedIcon.classList.remove('rotated');
                    });
                }
            });

            // Reset all other main menu icons
            document.querySelectorAll('.mobile-toggle-icon').forEach(otherIcon => {
                if (otherIcon !== icon) {
        otherIcon.classList.remove('rotated');
                }
            });

    // Toggle current submenu and icon
    submenu.classList.toggle('active');
    icon.classList.toggle('rotated');

    // If closing the main submenu, also close all its nested submenus
    if (!submenu.classList.contains('active')) {
        submenu.querySelectorAll('.mobile-subsubmenu, .mobile-subsubsubmenu').forEach(nestedMenu => {
            nestedMenu.classList.remove('active');
        });
                submenu.querySelectorAll('.submenu-toggle-icon, .subsubmenu-toggle-icon').forEach(nestedIcon => {
        nestedIcon.classList.remove('rotated');
                });
            }
        }

    // Toggle sub-submenu
    function toggleSubSubMenu(event, subsubmenuId) {
        event.preventDefault();
    event.stopPropagation();

    const subsubmenu = document.getElementById(subsubmenuId);
    const icon = event.currentTarget.querySelector('.submenu-toggle-icon');

    // Find the parent submenu to only close siblings within the same parent
    const parentSubmenu = subsubmenu.closest('.mobile-submenu');

            // Close other level-3 submenus in the same parent submenu
            parentSubmenu.querySelectorAll('.mobile-subsubmenu').forEach(menu => {
                if (menu.id !== subsubmenuId) {
        menu.classList.remove('active');
                    // Close all level-4 submenus within this level-3 menu
                    menu.querySelectorAll('.mobile-subsubsubmenu').forEach(level4Menu => {
        level4Menu.classList.remove('active');
                    });
                    // Reset level-4 icons
                    menu.querySelectorAll('.subsubmenu-toggle-icon').forEach(level4Icon => {
        level4Icon.classList.remove('rotated');
                    });
                }
            });

            // Reset all other level-3 submenu icons in the same parent
            parentSubmenu.querySelectorAll('.submenu-toggle-icon').forEach(otherIcon => {
                if (otherIcon !== icon) {
        otherIcon.classList.remove('rotated');
                }
            });

    // Toggle current level-3 submenu and icon rotation
    subsubmenu.classList.toggle('active');
    icon.classList.toggle('rotated');

    // If closing the level-3 submenu, also close all its level-4 submenus
    if (!subsubmenu.classList.contains('active')) {
        subsubmenu.querySelectorAll('.mobile-subsubsubmenu').forEach(level4Menu => {
            level4Menu.classList.remove('active');
        });
                subsubmenu.querySelectorAll('.subsubmenu-toggle-icon').forEach(level4Icon => {
        level4Icon.classList.remove('rotated');
                });
            }
        }

    // Toggle sub-sub-submenu
    function toggleSubSubSubMenu(event, subsubsubmenuId) {
        event.preventDefault();
    event.stopPropagation();

    const subsubsubmenu = document.getElementById(subsubsubmenuId);
    const icon = event.currentTarget.querySelector('.subsubmenu-toggle-icon');

    // Find the parent level-3 submenu to only close siblings within the same parent
    const parentSubsubmenu = subsubsubmenu.closest('.mobile-subsubmenu');

            // Close other level-4 submenus in the same parent level-3 submenu
            parentSubsubmenu.querySelectorAll('.mobile-subsubsubmenu').forEach(menu => {
                if (menu.id !== subsubsubmenuId) {
        menu.classList.remove('active');
                }
            });

            // Reset all other level-4 submenu icons in the same parent
            parentSubsubmenu.querySelectorAll('.subsubmenu-toggle-icon').forEach(otherIcon => {
                if (otherIcon !== icon) {
        otherIcon.classList.remove('rotated');
                }
            });

    // Toggle current level-4 submenu and icon rotation
    subsubsubmenu.classList.toggle('active');
    icon.classList.toggle('rotated');
        }

    // Close all menus when clicking outside
    document.addEventListener('click', function(event) {
            if (!event.target.closest('.mobile-sidebar')) {
        document.querySelectorAll('.mobile-submenu, .mobile-subsubmenu, .mobile-subsubsubmenu').forEach(menu => {
            menu.classList.remove('active');
        });
                document.querySelectorAll('.mobile-toggle-icon, .submenu-toggle-icon, .subsubmenu-toggle-icon').forEach(icon => {
        icon.classList.remove('rotated');
                });
            }
        });

    // Auto-hide mobile sidebar on full screen
    window.addEventListener('resize', function() {
            const sidebar = document.getElementById('mobileSidebar');
    const overlay = document.getElementById('overlay');

            if (window.innerWidth > 768) {
        sidebar.classList.remove('active');
    overlay.classList.remove('active');
            }
    });








