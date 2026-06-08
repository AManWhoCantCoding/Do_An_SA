$(document).ready(function () {
  if ($('#patients-table-body').length) {
    loadPatientsTable();
  }

  $('#add-patient-form').on('submit', async function (e) {
    e.preventDefault();
    const payload = {
      firstName: $('#firstName').val(),
      lastName: $('#lastName').val(),
      emailAddress: $('#emailAddress').val(),
      contactNumber: $('#contactNumber').val(),
      gender: $('#gender').val(),
      dateOfBirth: $('#dateOfBirth').val() || null,
      isPrivatePatient: $('#isPrivatePatient').is(':checked')
    };

    try {
      await MediSphereApi.patients.create(payload);
      window.location.href = '/Patient/Index';
    } catch (err) {
      alert('Failed to add patient: ' + err.message);
    }
  });

  const patientId = $('#patientId').val();
  if ($('#update-patient-form').length && patientId) {
    loadPatientForEdit(patientId);
  }

  $('#update-patient-form').on('submit', async function (e) {
    e.preventDefault();
    const id = $('#patientId').val();
    const payload = {
      firstName: $('#firstName').val(),
      lastName: $('#lastName').val(),
      emailAddress: $('#emailAddress').val(),
      contactNumber: $('#contactNumber').val(),
      gender: $('#gender').val(),
      dateOfBirth: $('#dateOfBirth').val() || null,
      isPrivatePatient: $('#isPrivatePatient').is(':checked')
    };

    try {
      await MediSphereApi.patients.update(id, payload);
      window.location.href = '/Patient/Index';
    } catch (err) {
      alert('Failed to update patient: ' + err.message);
    }
  });
});

async function loadPatientsTable() {
  try {
    const patients = await MediSphereApi.patients.getAll();
    const tbody = $('#patients-table-body');
    tbody.empty();

    patients.forEach(function (p) {
      const treatment = p.isPrivatePatient ? 'Private Healthcare' : 'NHS Treatment';
      const dob = p.dateOfBirth ? p.dateOfBirth.split('T')[0] : '';
      tbody.append(
        '<tr>' +
        '<td>' + p.patientId + '</td>' +
        '<td>' + escapeHtml(p.firstName) + '</td>' +
        '<td>' + escapeHtml(p.lastName) + '</td>' +
        '<td>' + escapeHtml(p.emailAddress) + '</td>' +
        '<td class="contact-number">' + escapeHtml(p.contactNumber) + '</td>' +
        '<td>' + escapeHtml(p.gender) + '</td>' +
        '<td>' + dob + '</td>' +
        '<td>' + treatment + '</td>' +
        '<td><a href="/Patient/UpdatePatient?id=' + p.patientId + '" class="btn btn-primary btn-sm"><i class="bi bi-pencil-square"></i></a></td>' +
        '</tr>'
      );
    });

    if ($('#searchBox').length) {
      $('#searchBox').off('keyup').on('keyup', function () {
        const searchText = $(this).val().toLowerCase();
        $('#patients-table-body tr').filter(function () {
          $(this).toggle($(this).text().toLowerCase().indexOf(searchText) > -1);
        });
      });
    }
  } catch (err) {
    alert('Failed to load patients: ' + err.message);
  }
}

async function loadPatientForEdit(id) {
  try {
    const p = await MediSphereApi.patients.getById(id);
    $('#firstName').val(p.firstName);
    $('#lastName').val(p.lastName);
    $('#emailAddress').val(p.emailAddress);
    $('#contactNumber').val(p.contactNumber);
    $('#gender').val(p.gender);
    if (p.dateOfBirth) {
      $('#dateOfBirth').val(p.dateOfBirth.split('T')[0]);
    }
    $('#isPrivatePatient').prop('checked', p.isPrivatePatient);
  } catch (err) {
    alert('Failed to load patient: ' + err.message);
  }
}

function escapeHtml(text) {
  if (!text) return '';
  return $('<div>').text(text).html();
}
