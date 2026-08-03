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
        const selectedOption = $('input[name="employeeFilter"]:checked').val();
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

async function performExport() {
    const format = document.getElementById('exportFormat').value;
    if (selectedColumnsList.length === 0) {
        alert('Please select at least one column to export.');
        return;
    }
    showBtnExportDataLoder();

    try {
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
                await exportToExcel(exportData);
                break;
            case 'csv':
                exportToCSV(exportData);
                break;
            case 'json':
                exportToJSON(exportData);
                break;
        }
    } finally {
        hideBtnExportDataLoder();
    }
}

// Uses ExcelJS (already loaded globally) instead of the free SheetJS/XLSX
// build — SheetJS's community build silently drops every cell.s style
// object on write, so bold headers/borders/font size never actually showed
// up in the downloaded file no matter what was set here. ExcelJS applies
// styling for real.
async function exportToExcel(data) {
    try {
        const headers = selectedColumnsList.map(function (col) { return col.displayName; });

        const wb = new ExcelJS.Workbook();
        const ws = wb.addWorksheet('Employee Data');

        ws.addRow(headers);
        data.forEach(function (row) {
            ws.addRow(headers.map(function (h) { return row[h] != null ? row[h] : ''; }));
        });

        const thinBorder = {
            top: { style: 'thin' },
            bottom: { style: 'thin' },
            left: { style: 'thin' },
            right: { style: 'thin' }
        };

        ws.getRow(1).eachCell(function (cell) {
            cell.font = { bold: true, size: 10 };
            cell.alignment = { horizontal: 'center', vertical: 'middle' };
            cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFDDDDDD' } };
            cell.border = thinBorder;
        });

        for (var r = 2; r <= ws.rowCount; r++) {
            ws.getRow(r).eachCell({ includeEmpty: true }, function (cell) {
                cell.font = { size: 10 };
                cell.alignment = { horizontal: 'left', vertical: 'middle' };
                cell.border = thinBorder;
            });
        }

        // Auto-fit column widths based on the longest value in each column.
        // Using getColumn(n) by 1-based index rather than ws.columns.forEach —
        // ws.columns is only populated if it was explicitly assigned, so
        // iterating it directly here would silently do nothing.
        headers.forEach(function (header, idx) {
            var maxLen = (header || '').toString().length;
            data.forEach(function (row) {
                var v = row[header];
                if (v != null && v !== '') maxLen = Math.max(maxLen, v.toString().length);
            });
            ws.getColumn(idx + 1).width = Math.min(Math.max(maxLen + 2, 10), 50);
        });

        const buffer = await wb.xlsx.writeBuffer();
        const blob = new Blob([buffer], { type: 'application/octet-stream' });
        saveAs(blob, 'employee_export_' + new Date().toISOString().slice(0, 10) + '.xlsx');
    } catch (error) {
        console.error('Export error:', error);
        alert('Error exporting data: ' + error.message);
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
