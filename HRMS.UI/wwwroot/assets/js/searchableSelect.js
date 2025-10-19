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