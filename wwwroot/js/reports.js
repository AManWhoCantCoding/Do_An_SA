$(document).ready(function () {
    if ($('#reports-table-body').length) {
        loadReportsTable();
    }
});

async function loadReportsTable() {
    try {
        const reports = await MediSphereApi.reports.getAll();
        const tbody = $('#reports-table-body');
        tbody.empty();

        reports.forEach(function (r) {
            tbody.append(
                '<tr>' +
                '<td>' + r.reportId + '</td>' +
                '<td>' + r.patientId + '</td>' +
                '<td>' + formatDate(r.createdAt) + '</td>' +
                '<td>' + (r.reportDescription || '') + '</td>' +
                '<td>' + (r.reportTypeName || 'No Report type used') + '</td>' +
                '<td>' + (r.isReportPrinted ? 'Yes' : 'No') + '</td>' +
                '<td>' + r.status + '</td>' +
                '<td>' + r.initialStaffName + '</td>' +
                '<td>' + formatDate(r.lastUpdated) + '</td>' +
                '<td>' + (r.lastUpdatedBy || '') + '</td>' +
                '<td>' +
                '<a href="/Reports/ExportReport?id=' + r.reportId + '" class="btn btn-primary btn-sm" title="Export"><i class="bi bi-arrow-bar-up"></i></a> ' +
                '<a href="/Reports/UpdateReport?id=' + r.reportId + '" class="btn btn-warning btn-sm" title="Edit"><i class="bi bi-pencil-square"></i></a>' +
                '</td>' +
                '</tr>'
            );
        });

        $('#searchBox').off('keyup').on('keyup', function () {
            const searchText = $(this).val().toLowerCase();
            $('#reports-table-body tr').filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
            });
        });
    } catch (err) {
        alert('Failed to load reports: ' + err.message);
    }
}

function formatDate(value) {
    if (!value) return '';
    return value.split('T')[0];
}
