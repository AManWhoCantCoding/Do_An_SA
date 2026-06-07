$(document).ready(function () {
    let patientsCache = [];

    $('#SelectedAppointment_StartTime, #NewAppointment_StartTime').datetimepicker({ format: 'YYYY-MM-DD HH:mm' });
    $('#SelectedAppointment_EndTime, #NewAppointment_EndTime').datetimepicker({ format: 'YYYY-MM-DD HH:mm' });

    loadPatientsIntoSelects().then(function () {
        initCalendar();
    });

    $('#add-appointment-form').on('submit', async function (e) {
        e.preventDefault();
        try {
            await DocDocGoApi.appointments.create(readAppointmentForm('#add-appointment-form'));
            $('#appointment-add').modal('hide');
            $('#calendar').fullCalendar('refetchEvents');
            $('#add-appointment-form')[0].reset();
        } catch (err) {
            alert('Failed to add appointment: ' + err.message);
        }
    });

    $('#edit-appointment-form').on('submit', async function (e) {
        e.preventDefault();
        const id = $('#SelectedAppointmentId').val();
        try {
            await DocDocGoApi.appointments.update(id, readAppointmentForm('#edit-appointment-form'));
            $('#appointment-edit').modal('hide');
            $('#calendar').fullCalendar('refetchEvents');
        } catch (err) {
            alert('Failed to update appointment: ' + err.message);
        }
    });

    $('#delete-appointment-btn').on('click', async function () {
        const id = $('#SelectedAppointmentId').val();
        if (!confirm('Delete this appointment?')) return;
        try {
            await DocDocGoApi.appointments.delete(id);
            $('#appointment-edit').modal('hide');
            $('#calendar').fullCalendar('refetchEvents');
        } catch (err) {
            alert('Failed to delete appointment: ' + err.message);
        }
    });

    $('#closeBtn, #maincloseBtn').click(function () { $('#appointment-add').modal('hide'); });
    $('#closeSelectedBtn, #maincloseSelectedBtn').click(function () { $('#appointment-edit').modal('hide'); });
    $('#appointment-add, #appointment-edit').on('hidden.bs.modal', function () { $(this).find('form')[0].reset(); });
});

async function loadPatientsIntoSelects() {
    patientsCache = await DocDocGoApi.patients.getAll();
    const options = patientsCache.map(function (p) {
        return '<option value="' + p.patientId + '">' + p.firstName + ' ' + p.lastName + '</option>';
    }).join('');
    $('#NewAppointment_PatientId, #SelectedAppointment_PatientId').html('<option value="">-- Select Patient --</option>' + options);
}

function initCalendar() {
    $('#calendar').fullCalendar({
        header: {
            left: 'prev, next, today',
            center: 'title',
            right: 'month,agendaWeek,agendaDay'
        },
        eventClick: async function (event) {
            try {
                const data = await DocDocGoApi.appointments.getById(event.id);
                $('#SelectedAppointment_Topic').val(data.topic);
                $('#SelectedAppointment_PatientId').val(data.patientId);
                $('#SelectedAppointment_StartTime').datetimepicker('date', moment(data.startTime));
                $('#SelectedAppointment_EndTime').datetimepicker('date', moment(data.endTime));
                $('#SelectedAppointment_Status').val(data.status);
                $('#SelectedAppointment_Notes').val(data.notes || '');
                $('#SelectedAppointmentId').val(event.id);
                $('#appointment-edit').modal('show');
            } catch (err) {
                alert('Failed to load appointment: ' + err.message);
            }
        },
        dayClick: function () {
            $('#appointment-add').modal('show');
        },
        firstDay: 1,
        events: async function (start, end, timezone, callback) {
            try {
                const appointments = await DocDocGoApi.appointments.getAll();
                const events = appointments.map(function (a) {
                    return {
                        id: a.appointmentId,
                        title: a.topic,
                        start: a.startTime,
                        end: a.endTime
                    };
                });
                callback(events);
            } catch (err) {
                callback([]);
                alert('Failed to load appointments: ' + err.message);
            }
        }
    });
}

function readAppointmentForm(formSelector) {
    const form = $(formSelector);
    const startVal = form.find('[name="startTime"]').val();
    const endVal = form.find('[name="endTime"]').val();
    return {
        patientId: parseInt(form.find('[name="patientId"]').val(), 10),
        topic: form.find('[name="topic"]').val(),
        startTime: moment(startVal, 'YYYY-MM-DD HH:mm').format(),
        endTime: moment(endVal, 'YYYY-MM-DD HH:mm').format(),
        notes: form.find('[name="notes"]').val(),
        status: form.find('[name="status"]').val()
    };
}
