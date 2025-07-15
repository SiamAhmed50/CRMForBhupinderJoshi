$(document).ready(function () {
    // Initialize DataTable
    const userTable = $('#userTable').DataTable({
        ajax: {
            url: '/Users?handler=List',
            type: 'GET',
            dataSrc: 'data'
        },
        columns: [
            { data: 'name' },
            { data: 'email' },
            {
                data: 'menuIds',
                render: function (data) {
                    return Array.isArray(data) ? data.join(', ') : '';
                }
            },
            {
                data: null,
                render: function () {
                    return '<button class="btn btn-sm btn-secondary">Edit</button>';
                }
            }
        ]
    });

    // Register modal select2
    $('#addUserModal').on('shown.bs.modal', function () {
        $('#menus').select2({
            dropdownParent: $('#addUserModal'),
            width: '100%',
            placeholder: "Select menus",
            allowClear: true
        });
    });

    // Submit form
    $('#userForm').on('submit', function (e) {
        e.preventDefault();

        debugger

        const formData = new FormData();
        formData.append("UserName", $('#name').val());
        formData.append("Email", $('#email').val());
        formData.append("Password", $('#password').val());
        formData.append("ConfirmPassword", $('#confirmPassword').val());

        $('#menus').val().forEach(id => {
            formData.append("MenuIds", id); // Must match API DTO
        });

        $.ajax({
            url: '/Users?handler=Register',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                $('#addUserModal').modal('hide');
                $('#userTable').DataTable().ajax.reload();
                Swal.fire("Success", "User registered", "success");
            },
            error: function () {
                Swal.fire("Error", "Failed to register user", "error");
            }
        });
    });


});