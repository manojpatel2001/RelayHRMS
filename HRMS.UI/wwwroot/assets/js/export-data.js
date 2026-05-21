// Sample data for demonstration
let sampleData = [];



let availableColumnsList = [];
let selectedColumnsList = [];
let draggedItem = null;
let selectedItem = null;
let firstDataRowHeader = null;
// Initialize the modal with default columns
function initializeModal() {
    if (!sampleData || sampleData.length === 0) return;

    // Get the column keys from the first data row
    
    availableColumnsList = Object.keys(firstDataRowHeader).map(key => ({
        key: key,
        displayName: firstDataRowHeader[key] || key // Use mapped name or fall back to key
    }));
    
    selectedItem = null;
    selectedColumnsList = [];
    updateAvailableColumns();
    updateSelectedColumns();
}

function showBtnExportLoder() {
   
    $('#btnExport').removeClass('d-flex').hide();
    $('#btnExporting').addClass('d-flex').show();
}
function hideBtnExportLoder() {
    $('#btnExporting').removeClass('d-flex').hide();
    $('#btnExport').addClass('d-flex').show();
}
function showBtnExportDataLoder() {
   
    $('#exportEmployee').hide();
    $('#exportingEmployee').show();
}
function hideBtnExportDataLoder() {
    $('#exportEmployee').show();
    $('#exportingEmployee').hide();
}

async function loadEmployeeMasterData(data) {
    try {
        const payload = {
            CompanyId: CompanyId, // Make sure to define CompanyId
            IsLeft: data
        };

        const response = await fetch(BaseUrlLayout + '/ExportDataAPI/GetAllEmployeeExportData', {
            method: 'POST',
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem("authToken"),
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(payload)
        });

        const responseData = await response.json();
        if (responseData.isSuccess) {
            firstDataRowHeader = responseData.data[0];
            sampleData = responseData.data.slice(1);
            initializeModal();
        }
    } catch (error) {
        console.error('Fetch error:', error);
    }
}

async function openExportModal(report) {

    showBtnExportLoder();
    if (report ==="Employee Master") {
        const selectedOption = $('input[name="options"]:checked').val();
        let data = null;

        if (selectedOption === "LeftEmployee") {
            data = true;
        } else if (selectedOption === "CurrentEmployee") {
            data = false;
        }
        await loadEmployeeMasterData(data);
    }
    hideBtnExportLoder();
    initializeModal();
    document.getElementById('exportModal').classList.add('active');
    document.body.style.overflow = 'hidden';
}


function closeExportModal() {
    document.getElementById('exportModal').classList.remove('active');
    document.body.style.overflow = 'auto';
}

function updateAvailableColumns() {
    const container = document.getElementById('availableColumns');
    container.innerHTML = '';

    availableColumnsList
        .filter(col => !selectedColumnsList.some(selected => selected.key === col.key))
        .forEach(column => {
            const item = createColumnItem(column, false);
            container.appendChild(item);
        });
}

function updateSelectedColumns() {
    const container = document.getElementById('selectedColumns');
    container.innerHTML = '';

    selectedColumnsList.forEach((column, index) => {
        const item = createColumnItem(column, true, index + 1);
        container.appendChild(item);
    });
}

function createColumnItem(columnInfo, isSelected, order = null) {
    const item = document.createElement('div');
    item.className = `column-item ${isSelected ? 'selected' : ''}`;
    item.draggable = true;
    item.dataset.column = columnInfo.key;
    item.innerHTML = `
        <div class="drag-handle">☰</div>
        <span>${columnInfo.displayName}</span>
        ${order ? `<div class="order-indicator">${order}</div>` : ''}
    `;

    item.addEventListener('dragstart', handleDragStart);
    item.addEventListener('dragend', handleDragEnd);

    if (isSelected) {
        item.addEventListener('click', (e) => {
            e.preventDefault();
            selectItem(columnInfo.key);
        });
        item.style.cursor = 'pointer';
    }
    return item;
}

function selectItem(columnKey) {
    // Clear previous selection
    document.querySelectorAll('#selectedColumns .column-item').forEach(item => {
        item.classList.remove('highlight');
    });

    // Highlight selected item
    const selectedElement = document.querySelector(`#selectedColumns .column-item[data-column="${columnKey}"]`);
    if (selectedElement) {
        selectedElement.classList.add('highlight');
        selectedItem = columnKey;
    }
}

function moveUp() {
    if (!selectedItem) {
        alert('Please click on a column in the "Selected Columns" list first, then click Move Up.');
        return;
    }

    const currentIndex = selectedColumnsList.findIndex(col => col.key === selectedItem);
    if (currentIndex > 0) {
        const newList = [...selectedColumnsList];
        [newList[currentIndex], newList[currentIndex - 1]] = [newList[currentIndex - 1], newList[currentIndex]];
        selectedColumnsList = newList;
        updateSelectedColumns();

        setTimeout(() => {
            selectItem(selectedItem);
        }, 50);
    } else {
        alert('Cannot move up - item is already at the top.');
    }
}

function moveDown() {
    if (!selectedItem) {
        alert('Please click on a column in the "Selected Columns" list first, then click Move Down.');
        return;
    }

    const currentIndex = selectedColumnsList.findIndex(col => col.key === selectedItem);
    if (currentIndex < selectedColumnsList.length - 1 && currentIndex !== -1) {
        const newList = [...selectedColumnsList];
        [newList[currentIndex], newList[currentIndex + 1]] = [newList[currentIndex + 1], newList[currentIndex]];
        selectedColumnsList = newList;
        updateSelectedColumns();

        setTimeout(() => {
            selectItem(selectedItem);
        }, 50);
    } else {
        alert('Cannot move down - item is already at the bottom.');
    }
}

function handleDragStart(e) {
    draggedItem = e.target;
    e.target.classList.add('dragging');
    e.dataTransfer.effectAllowed = 'move';
    e.dataTransfer.setData('text/plain', e.target.dataset.column);
}

function handleDragEnd(e) {
    e.target.classList.remove('dragging');
    draggedItem = null;
}

// Add drop zones
document.addEventListener('DOMContentLoaded', function () {
    const dropZones = document.querySelectorAll('.ec-column-list');
    dropZones.forEach(zone => {
        zone.addEventListener('dragover', handleDragOver);
        zone.addEventListener('drop', handleDrop);
        zone.addEventListener('dragenter', handleDragEnter);
        zone.addEventListener('dragleave', handleDragLeave);
    });
});

function handleDragOver(e) {
    e.preventDefault();
    e.dataTransfer.dropEffect = 'move';
}

function handleDragEnter(e) {
    e.preventDefault();
    e.target.classList.add('drag-over');
}

function handleDragLeave(e) {
    e.target.classList.remove('drag-over');
}

function handleDrop(e) {
    e.preventDefault();
    e.target.classList.remove('drag-over');

    if (!draggedItem) return;

    const columnKey = draggedItem.dataset.column;
    const columnInfo = availableColumnsList.find(col => col.key === columnKey);
    const targetZone = e.target.closest('.ec-column-list');

    if (targetZone.id === 'selectedColumns' && !selectedColumnsList.some(col => col.key === columnKey)) {
        selectedColumnsList.push(columnInfo);
    } else if (targetZone.id === 'availableColumns') {
        selectedColumnsList = selectedColumnsList.filter(col => col.key !== columnKey);
        selectedItem = null;
    }

    updateAvailableColumns();
    updateSelectedColumns();
}

function moveToSelected() {
    availableColumnsList.forEach(col => {
        if (!selectedColumnsList.some(selected => selected.key === col.key)) {
            selectedColumnsList.push(col);
        }
    });
    updateAvailableColumns();
    updateSelectedColumns();
}

function moveToAvailable() {
    selectedColumnsList = [];
    selectedItem = null;
    updateAvailableColumns();
    updateSelectedColumns();
}

function selectAllColumns() {
    selectedColumnsList = [...availableColumnsList];
    selectedItem = null;
    updateAvailableColumns();
    updateSelectedColumns();
}

function clearAllColumns() {
    selectedColumnsList = [];
    selectedItem = null;
    updateAvailableColumns();
    updateSelectedColumns();
}

function performExport() {
    const format = document.getElementById('exportFormat').value;
    if (selectedColumnsList.length === 0) {
        alert('Please select at least one column to export.');
        return;
    }
    showBtnExportDataLoder();

    // Create export data with selected columns in the specified order
    const exportData = sampleData.map(row => {
        const newRow = {};
        selectedColumnsList.forEach(col => {
            // Use the original key to access data, but display name for headers
            newRow[col.displayName] = row[col.key];
        });
        return newRow;
    });

    switch (format) {
        case 'xlsx':
            exportToExcel(exportData);
            break;
        case 'csv':
            exportToCSV(exportData);
            break;
        case 'json':
            exportToJSON(exportData);
            break;
    }
    hideBtnExportDataLoder();
}

//function exportToExcel(data) {
//    try {
//        // Create worksheet with headers (using display names)
//        const ws = XLSX.utils.json_to_sheet(data);

//        // Create workbook
//        const wb = XLSX.utils.book_new();
//        XLSX.utils.book_append_sheet(wb, ws, "Employee Data");

//        // Style headers
//        selectedColumnsList.forEach((col, index) => {
//            const cellRef = XLSX.utils.encode_cell({ r: 0, c: index });
//            if (ws[cellRef]) {
//                ws[cellRef].s = {
//                    font: { bold: true },
//                    alignment: { horizontal: "center" }
//                };
//            }
//        });

//        // Set column widths
//        ws['!cols'] = selectedColumnsList.map(() => ({ wch: 25 }));

//        // Export
//        XLSX.writeFile(wb, "employee_export_" + new Date().toISOString().slice(0, 10) + ".xlsx");
//    } catch (error) {
//        console.error("Export error:", error);
//        alert("Error exporting data. Please check console.");
//    }
//}
function exportToExcel(data) {
    try {
        // Create worksheet with headers
        const ws = XLSX.utils.json_to_sheet(data);

        // Create workbook
        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Employee Data");

        // Get the range of the worksheet
        const range = XLSX.utils.decode_range(ws['!ref']);

        // Calculate column widths based on content
        const colWidths = [];
        for (let C = range.s.c; C <= range.e.c; ++C) {
            let maxWidth = 10; // minimum width

            // Check all rows for this column
            for (let R = range.s.r; R <= range.e.r; ++R) {
                const cellAddress = XLSX.utils.encode_cell({ r: R, c: C });
                const cell = ws[cellAddress];

                if (cell && cell.v) {
                    const cellLength = cell.v.toString().length;
                    maxWidth = Math.max(maxWidth, cellLength + 2); // +2 for padding
                }
            }

            // Cap maximum width at 50 characters
            colWidths.push({ wch: Math.min(maxWidth, 50) });
        }

        // Apply column widths
        ws['!cols'] = colWidths;

        // Apply styles to all cells (if styling is supported)
        if (range) {
            for (let R = range.s.r; R <= range.e.r; ++R) {
                for (let C = range.s.c; C <= range.e.c; ++C) {
                    const cellAddress = XLSX.utils.encode_cell({ r: R, c: C });

                    // Ensure cell exists
                    if (!ws[cellAddress]) {
                        ws[cellAddress] = { v: "", t: "s" };
                    }

                    // Initialize style object
                    if (!ws[cellAddress].s) {
                        ws[cellAddress].s = {};
                    }

                    // Apply borders to all cells
                    ws[cellAddress].s.border = {
                        top: { style: "thin" },
                        bottom: { style: "thin" },
                        left: { style: "thin" },
                        right: { style: "thin" }
                    };

                    // Style header row (first row)
                    if (R === range.s.r) {
                        ws[cellAddress].s.font = {
                            bold: true,
                            sz: 12
                        };
                        ws[cellAddress].s.fill = {
                            patternType: "solid",
                            fgColor: { rgb: "DDDDDD" }
                        };
                        ws[cellAddress].s.alignment = {
                            horizontal: "center",
                            vertical: "center"
                        };
                    } else {
                        // Style data rows
                        ws[cellAddress].s.alignment = {
                            horizontal: "left",
                            vertical: "center"
                        };
                    }
                }
            }
        }

        // Alternative approach if styling doesn't work - use writeFileXLSX
        try {
            XLSX.writeFileXLSX(wb, "employee_export_" + new Date().toISOString().slice(0, 10) + ".xlsx");
        } catch (xlsxError) {
            // Fallback to regular writeFile
            XLSX.writeFile(wb, "employee_export_" + new Date().toISOString().slice(0, 10) + ".xlsx");
        }


    } catch (error) {
        console.error("Export error:", error);
        alert("Error exporting data: " + error.message);
    }
}

// Alternative simplified version if styling still doesn't work
function exportToExcelSimple(data) {
    try {
        // Create worksheet
        const ws = XLSX.utils.json_to_sheet(data);

        // Create workbook
        const wb = XLSX.utils.book_new();
        XLSX.utils.book_append_sheet(wb, ws, "Employee Data");

        // Auto-fit columns based on content
        const range = XLSX.utils.decode_range(ws['!ref']);
        const colWidths = [];

        for (let C = range.s.c; C <= range.e.c; ++C) {
            let maxWidth = 8;
            for (let R = range.s.r; R <= range.e.r; ++R) {
                const cell = ws[XLSX.utils.encode_cell({ r: R, c: C })];
                if (cell && cell.v) {
                    maxWidth = Math.max(maxWidth, cell.v.toString().length + 2);
                }
            }
            colWidths.push({ wch: Math.min(maxWidth, 50) });
        }

        ws['!cols'] = colWidths;

        // Export
        XLSX.writeFile(wb, "employee_export_" + new Date().toISOString().slice(0, 10) + ".xlsx");

    } catch (error) {
        console.error("Export error:", error);
        alert("Error exporting data: " + error.message);
    }
}
function exportToCSV(data) {
    const csvContent = [];

    // Add headers (display names)
    csvContent.push(selectedColumnsList.map(col => `"${col.displayName}"`).join(','));

    // Add data rows
    data.forEach(row => {
        const rowData = selectedColumnsList.map(col => {
            let value = row[col.displayName] || '';
            if (typeof value !== 'string') value = String(value);

            if (value.includes(',') || value.includes('"') || value.includes('\n')) {
                value = `"${value.replace(/"/g, '""')}"`;
            }
            return value;
        });
        csvContent.push(rowData.join(','));
    });

    downloadFile(csvContent.join('\n'), 'employee_export.csv', 'text/csv');
}

function exportToJSON(data) {
    // Convert back to original key structure for JSON export
    const jsonData = sampleData.map(row => {
        const newRow = {};
        selectedColumnsList.forEach(col => {
            newRow[col.key] = row[col.key];
        });
        return newRow;
    });

    downloadFile(JSON.stringify(jsonData, null, 2), 'employee_export.json', 'application/json');
}

function downloadFile(content, filename, contentType) {
    const blob = new Blob([content], { type: contentType });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
}

// Close modal on escape key
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        closeExportModal();
    }
});

// Close modal on overlay click
document.getElementById('exportModal').addEventListener('click', function (e) {
    if (e.target === this) {
        closeExportModal();
    }
}); 
