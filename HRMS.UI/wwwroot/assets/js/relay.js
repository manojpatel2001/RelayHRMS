
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





//(function ($) {
//    $.fn.searchableSelect = function (options) {
//        const settings = $.extend({
//            data: [],
//            valueKey: 'id',
//            textKey: 'name',
//            placeholder: 'Select an option',
//            searchPlaceholder: 'Search...',
//            onChange: null
//        }, options);

//        return this.each(function () {
//            const $wrapper = $(this).closest('.searchable-select-wrapper');
//            const $input = $(this);
//            const $dropdown = $wrapper.find('.searchable-select-dropdown');
//            const $searchInput = $wrapper.find('.search-input');
//            const $optionsContainer = $wrapper.find('.searchable-select-options');

//            let selectedValue = null;
//            let allData = settings.data;
//            let currentHighlightIndex = -1;
//            let filteredData = [];

//            // Initialize
//            $searchInput.attr('placeholder', settings.searchPlaceholder);

//            // Update floating label state
//            function updateFloatingLabel() {
//                if (selectedValue) {
//                    $wrapper.addClass('has-value');
//                } else {
//                    $wrapper.removeClass('has-value');
//                }
//            }

//            // Scroll highlighted option into view
//            function scrollToHighlighted() {
//                const $highlighted = $optionsContainer.find('.searchable-select-option.highlighted');
//                if ($highlighted.length) {
//                    const containerHeight = $optionsContainer.height();
//                    const optionTop = $highlighted.position().top;
//                    const optionHeight = $highlighted.outerHeight();
//                    const scrollTop = $optionsContainer.scrollTop();

//                    if (optionTop < 0) {
//                        $optionsContainer.scrollTop(scrollTop + optionTop);
//                    } else if (optionTop + optionHeight > containerHeight) {
//                        $optionsContainer.scrollTop(scrollTop + optionTop + optionHeight - containerHeight);
//                    }
//                }
//            }

//            // Highlight option by index
//            function highlightOption(index) {
//                $optionsContainer.find('.searchable-select-option').removeClass('highlighted');

//                if (index >= 0 && index < filteredData.length) {
//                    currentHighlightIndex = index;
//                    $optionsContainer.find('.searchable-select-option').eq(index).addClass('highlighted');
//                    scrollToHighlighted();
//                }
//            }

//            // Select highlighted option
//            function selectHighlightedOption() {
//                if (currentHighlightIndex >= 0 && currentHighlightIndex < filteredData.length) {
//                    const item = filteredData[currentHighlightIndex];
//                    selectOption(item[settings.valueKey], item[settings.textKey]);
//                }
//            }

//            // Select option
//            function selectOption(value, text) {
//                selectedValue = allData.find(item => item[settings.valueKey] == value);

//                $input.val(text).removeClass('is-invalid');
//                $wrapper.removeClass('active').removeClass('is-invalid');
//                updateFloatingLabel();

//                // Clear error
//                $wrapper.closest('.form-floating-custom').find('.error-message').text('');

//                // Trigger onChange callback
//                if (settings.onChange && typeof settings.onChange === 'function') {
//                    settings.onChange(selectedValue);
//                }
//            }

//            // Render options
//            function renderOptions(data) {
//                filteredData = data;
//                currentHighlightIndex = -1;

//                if (data.length === 0) {
//                    $optionsContainer.html('<div class="searchable-select-no-results">No results found</div>');
//                    return;
//                }

//                const optionsHtml = data.map(item => {
//                    const isSelected = selectedValue && selectedValue[settings.valueKey] == item[settings.valueKey];
//                    return `<div class="searchable-select-option ${isSelected ? 'selected' : ''}"
//                                          data-value="${item[settings.valueKey]}"
//                                          data-text="${item[settings.textKey]}">
//                                          ${item[settings.textKey]}
//                                      </div>`;
//                }).join('');

//                $optionsContainer.html(optionsHtml);
//            }

//            // Initial render
//            renderOptions(allData);
//            updateFloatingLabel();

//            // Toggle dropdown
//            $input.on('click', function (e) {
//                e.stopPropagation();
//                $('.searchable-select-wrapper').not($wrapper).removeClass('active');
//                $wrapper.toggleClass('active');
//                if ($wrapper.hasClass('active')) {
//                    $searchInput.val('').focus();
//                    renderOptions(allData);
//                }
//            });

//            // Search functionality
//            $searchInput.on('input', function () {
//                const searchTerm = $(this).val().toLowerCase();
//                const filtered = allData.filter(item =>
//                    item[settings.textKey].toLowerCase().includes(searchTerm)
//                );
//                renderOptions(filtered);
//            });

//            // Keyboard navigation for search input
//            $searchInput.on('keydown', function (e) {
//                const visibleOptions = $optionsContainer.find('.searchable-select-option').length;

//                switch (e.key) {
//                    case 'ArrowDown':
//                        e.preventDefault();
//                        if (visibleOptions > 0) {
//                            const nextIndex = currentHighlightIndex + 1;
//                            highlightOption(nextIndex >= visibleOptions ? 0 : nextIndex);
//                        }
//                        break;

//                    case 'ArrowUp':
//                        e.preventDefault();
//                        if (visibleOptions > 0) {
//                            const prevIndex = currentHighlightIndex - 1;
//                            highlightOption(prevIndex < 0 ? visibleOptions - 1 : prevIndex);
//                        }
//                        break;

//                    case 'Enter':
//                        e.preventDefault();
//                        if (currentHighlightIndex >= 0) {
//                            selectHighlightedOption();
//                        }
//                        break;

//                    case 'Escape':
//                        e.preventDefault();
//                        $wrapper.removeClass('active');
//                        $input.focus();
//                        break;

//                    case 'Tab':
//                        $wrapper.removeClass('active');
//                        break;
//                }
//            });

//            // Keyboard navigation for main input
//            $input.on('keydown', function (e) {
//                if (!$wrapper.hasClass('active') && (e.key === 'ArrowDown' || e.key === 'ArrowUp' || e.key === 'Enter' || e.key === ' ')) {
//                    e.preventDefault();
//                    $input.click();
//                }
//            });

//            // Option selection
//            $optionsContainer.on('click', '.searchable-select-option', function () {
//                const value = $(this).data('value');
//                const text = $(this).data('text');
//                selectOption(value, text);
//            });

//            // Hover effect for options
//            $optionsContainer.on('mouseenter', '.searchable-select-option', function () {
//                const index = $(this).index();
//                highlightOption(index);
//            });

//            // Click outside to close
//            $(document).on('click', function (e) {
//                if (!$wrapper.is(e.target) && $wrapper.has(e.target).length === 0) {
//                    $wrapper.removeClass('active');
//                }
//            });

//            // Prevent dropdown close on search input click
//            $searchInput.on('click', function (e) {
//                e.stopPropagation();
//            });

//            // Public methods
//            $input.data('searchableSelect', {
//                getValue: function () {
//                    return selectedValue ? selectedValue[settings.valueKey] : null;
//                },
//                getSelectedItem: function () {
//                    return selectedValue;
//                },
//                setValue: function (value) {
//                    const item = allData.find(d => d[settings.valueKey] == value);
//                    if (item) {
//                        selectedValue = item;
//                        $input.val(item[settings.textKey]);
//                        renderOptions(allData);
//                        updateFloatingLabel();
//                    }
//                },
//                clear: function () {
//                    selectedValue = null;
//                    $input.val('').removeClass('is-invalid');
//                    $wrapper.removeClass('is-invalid').removeClass('has-value');
//                    $wrapper.closest('.form-floating-custom').find('.error-message').text('');
//                    renderOptions(allData);
//                    updateFloatingLabel();
//                },
//                setData: function (data) {
//                    allData = data;
//                    renderOptions(allData);
//                }
//            });
//        });
//    };
//})(jQuery);


(function ($) {
	$.fn.searchableSelect = function (options) {
		const settings = $.extend({
			data: [],
			valueKey: 'id',
			textKey: 'name',
			placeholder: 'Select an option',
			searchPlaceholder: 'Search...',
			onChange: null,
			multiple: false,
			maxSelection: null
		}, options);

		return this.each(function () {
			const $wrapper = $(this).closest('.searchable-select-wrapper');
			const $input = $(this);
			const $dropdown = $wrapper.find('.searchable-select-dropdown');
			const $searchInput = $wrapper.find('.search-input');
			const $optionsContainer = $wrapper.find('.searchable-select-options');
			const $selectAllBtn = $wrapper.find('.select-all-btn');
			const $clearAllBtn = $wrapper.find('.clear-all-btn');

			let selectedValues = settings.multiple ? [] : null;
			let allData = settings.data;
			let currentHighlightIndex = -1;
			let filteredData = [];

			// Add/remove multi-select class
			if (settings.multiple) {
				$wrapper.addClass('multi-select');
				const $tagsContainer = $input.find('.selected-tags');
				if ($tagsContainer.length === 0) {
					$input.html('<div class="selected-tags"><span class="selected-tags-placeholder"></span></div>');
				}
			} else {
				$wrapper.removeClass('multi-select');
			}

			// Initialize
			$searchInput.attr('placeholder', settings.searchPlaceholder);

			// Update floating label state
			function updateFloatingLabel() {
				if (settings.multiple) {
					if (selectedValues.length > 0) {
						$wrapper.addClass('has-value');
					} else {
						$wrapper.removeClass('has-value');
					}
				} else {
					if (selectedValues) {
						$wrapper.addClass('has-value');
					} else {
						$wrapper.removeClass('has-value');
					}
				}
			}

			// Render selected tags (multi-select)
			function renderTags() {
				if (!settings.multiple) return;

				const $tagsContainer = $input.find('.selected-tags');
				if (selectedValues.length === 0) {
					$tagsContainer.html(`<span class="selected-tags-placeholder">${settings.placeholder}</span>`);
				} else {
					const tagsHtml = selectedValues.map(item => {
						return `<span class="selected-tag" data-value="${item[settings.valueKey]}">
											${item[settings.textKey]}
											<span class="selected-tag-remove">×</span>
										</span>`;
					}).join('');
					$tagsContainer.html(tagsHtml);
				}
				updateFloatingLabel();
			}

			// Remove tag (multi-select)
			if (settings.multiple) {
				$input.on('click', '.selected-tag-remove', function (e) {
					e.stopPropagation();
					const value = $(this).parent().data('value');
					selectedValues = selectedValues.filter(item => item[settings.valueKey] != value);
					renderTags();
					renderOptions(filteredData);
					triggerChange();
				});
			}

			// Scroll highlighted option into view
			function scrollToHighlighted() {
				const $highlighted = $optionsContainer.find('.searchable-select-option.highlighted');
				if ($highlighted.length) {
					const containerHeight = $optionsContainer.height();
					const optionTop = $highlighted.position().top;
					const optionHeight = $highlighted.outerHeight();
					const scrollTop = $optionsContainer.scrollTop();

					if (optionTop < 0) {
						$optionsContainer.scrollTop(scrollTop + optionTop);
					} else if (optionTop + optionHeight > containerHeight) {
						$optionsContainer.scrollTop(scrollTop + optionTop + optionHeight - containerHeight);
					}
				}
			}

			// Highlight option by index
			function highlightOption(index) {
				$optionsContainer.find('.searchable-select-option').removeClass('highlighted');

				if (index >= 0 && index < filteredData.length) {
					currentHighlightIndex = index;
					$optionsContainer.find('.searchable-select-option').eq(index).addClass('highlighted');
					scrollToHighlighted();
				}
			}

			// Select option (single select)
			function selectOption(value, text) {
				if (settings.multiple) return;

				selectedValues = allData.find(item => item[settings.valueKey] == value);
				$input.val(text).removeClass('is-invalid');
				$wrapper.removeClass('active').removeClass('is-invalid');
				updateFloatingLabel();

				$wrapper.closest('.form-floating-custom').find('.error-message').text('');
				triggerChange();
			}

			// Toggle option (multi-select)
			function toggleOption(value) {
				if (!settings.multiple) return;

				const item = allData.find(d => d[settings.valueKey] == value);
				if (!item) return;

				const index = selectedValues.findIndex(v => v[settings.valueKey] == value);

				if (index > -1) {
					selectedValues.splice(index, 1);
				} else {
					if (settings.maxSelection && selectedValues.length >= settings.maxSelection) {
						alert(`You can only select up to ${settings.maxSelection} items`);
						return;
					}
					selectedValues.push(item);
				}

				$input.removeClass('is-invalid');
				$wrapper.removeClass('is-invalid');
				$wrapper.closest('.form-floating-custom').find('.error-message').text('');

				renderTags();
				renderOptions(filteredData);
				triggerChange();
			}

			// Trigger onChange callback
			function triggerChange() {
				if (settings.onChange && typeof settings.onChange === 'function') {
					settings.onChange(settings.multiple ? selectedValues : selectedValues);
				}
			}

			// Render options
			function renderOptions(data) {
				filteredData = data;
				currentHighlightIndex = -1;

				if (data.length === 0) {
					$optionsContainer.html('<div class="searchable-select-no-results">No results found</div>');
					return;
				}

				const optionsHtml = data.map(item => {
					let isSelected = false;
					if (settings.multiple) {
						isSelected = selectedValues.some(v => v[settings.valueKey] == item[settings.valueKey]);
					} else {
						isSelected = selectedValues && selectedValues[settings.valueKey] == item[settings.valueKey];
					}

					return `<div class="searchable-select-option ${isSelected ? 'selected' : ''}"
										data-value="${item[settings.valueKey]}"
										data-text="${item[settings.textKey]}">
										<div class="option-checkbox"></div>
										<span>${item[settings.textKey]}</span>
									</div>`;
				}).join('');

				$optionsContainer.html(optionsHtml);
			}

			// Initial render
			renderOptions(allData);
			if (settings.multiple) {
				renderTags();
			} else {
				updateFloatingLabel();
			}

			// Toggle dropdown
			$input.on('click', function (e) {
				if ($(e.target).closest('.selected-tag-remove').length) return;

				e.stopPropagation();
				$('.searchable-select-wrapper').not($wrapper).removeClass('active');
				$wrapper.toggleClass('active');
				if ($wrapper.hasClass('active')) {
					$searchInput.val('').focus();
					renderOptions(allData);
				}
			});

			// Search functionality
			$searchInput.on('input', function () {
				const searchTerm = $(this).val().toLowerCase();
				const filtered = allData.filter(item =>
					item[settings.textKey].toLowerCase().includes(searchTerm)
				);
				renderOptions(filtered);
			});

			// Keyboard navigation
			$searchInput.on('keydown', function (e) {
				const visibleOptions = $optionsContainer.find('.searchable-select-option').length;

				switch (e.key) {
					case 'ArrowDown':
						e.preventDefault();
						if (visibleOptions > 0) {
							const nextIndex = currentHighlightIndex + 1;
							highlightOption(nextIndex >= visibleOptions ? 0 : nextIndex);
						}
						break;

					case 'ArrowUp':
						e.preventDefault();
						if (visibleOptions > 0) {
							const prevIndex = currentHighlightIndex - 1;
							highlightOption(prevIndex < 0 ? visibleOptions - 1 : prevIndex);
						}
						break;

					case 'Enter':
						e.preventDefault();
						if (currentHighlightIndex >= 0 && currentHighlightIndex < filteredData.length) {
							const item = filteredData[currentHighlightIndex];
							if (settings.multiple) {
								toggleOption(item[settings.valueKey]);
							} else {
								selectOption(item[settings.valueKey], item[settings.textKey]);
							}
						}
						break;

					case 'Escape':
						e.preventDefault();
						$wrapper.removeClass('active');
						if (!settings.multiple) $input.focus();
						break;

					case 'Tab':
						$wrapper.removeClass('active');
						break;
				}
			});

			// Keyboard for main input (single select)
			if (!settings.multiple) {
				$input.on('keydown', function (e) {
					if (!$wrapper.hasClass('active') && (e.key === 'ArrowDown' || e.key === 'ArrowUp' || e.key === 'Enter' || e.key === ' ')) {
						e.preventDefault();
						$input.click();
					}
				});
			}

			// Option selection/toggle
			$optionsContainer.on('click', '.searchable-select-option', function (e) {
				e.stopPropagation();
				const value = $(this).data('value');
				const text = $(this).data('text');

				if (settings.multiple) {
					toggleOption(value);
				} else {
					selectOption(value, text);
				}
			});

			// Hover effect
			$optionsContainer.on('mouseenter', '.searchable-select-option', function () {
				const index = $(this).index();
				highlightOption(index);
			});

			// Select All (multi-select)
			$selectAllBtn.on('click', function (e) {
				e.stopPropagation();
				if (!settings.multiple) return;

				const currentFiltered = filteredData.length > 0 ? filteredData : allData;

				if (settings.maxSelection) {
					selectedValues = currentFiltered.slice(0, settings.maxSelection);
				} else {
					selectedValues = [...currentFiltered];
				}

				renderTags();
				renderOptions(filteredData);
				triggerChange();
			});

			// Clear All
			$clearAllBtn.on('click', function (e) {
				e.stopPropagation();
				if (settings.multiple) {
					selectedValues = [];
					renderTags();
				} else {
					selectedValues = null;
					$input.val('');
					updateFloatingLabel();
				}
				renderOptions(filteredData);
				triggerChange();
			});

			// Click outside to close
			$(document).on('click', function (e) {
				if (!$wrapper.is(e.target) && $wrapper.has(e.target).length === 0) {
					$wrapper.removeClass('active');
				}
			});

			// Prevent dropdown close on search input click
			$searchInput.on('click', function (e) {
				e.stopPropagation();
			});

			// Public methods
			const api = {
				getValue: function () {
					if (settings.multiple) {
						return selectedValues.map(item => item[settings.valueKey]);
					}
					return selectedValues ? selectedValues[settings.valueKey] : null;
				},
				getSelectedItem: function () {
					return selectedValues;
				},
				setValue: function (value) {
					if (settings.multiple) {
						selectedValues = allData.filter(item => value.includes(item[settings.valueKey]));
						renderTags();
					} else {
						const item = allData.find(d => d[settings.valueKey] == value);
						if (item) {
							selectedValues = item;
							$input.val(item[settings.textKey]);
							updateFloatingLabel();
						}
					}
					renderOptions(allData);
				},
				clear: function () {
					if (settings.multiple) {
						selectedValues = [];
						renderTags();
					} else {
						selectedValues = null;
						$input.val('');
						updateFloatingLabel();
					}
					$input.removeClass('is-invalid');
					$wrapper.removeClass('is-invalid').removeClass('has-value');
					$wrapper.closest('.form-floating-custom').find('.error-message').text('');
					renderOptions(allData);
				},
				setData: function (data) {
					allData = data;
					if (settings.multiple) {
						selectedValues = [];
						renderTags();
					} else {
						selectedValues = null;
						$input.val('');
						updateFloatingLabel();
					}
					renderOptions(allData);
				}
			};

			$input.data('searchableSelect', api);
		});
	};
})(jQuery);