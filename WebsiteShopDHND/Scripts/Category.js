$(document).ready(function () {
    Load();
    XuLyNhapLieu();
    $("#btnAdd").click(function () {
        Add();
    });
    $(document).on("click", ".btnEdit", function () {
        var MaDanhMuc = $(this).closest("tr").find("td:eq(1)").text();
        GetbyID(MaDanhMuc);
    });
    $("#btnSaveEdit").click(function () {
        Update();
    });
    $(document).on("click", ".btnDelete", function () {
        var id = $(this).closest("tr").find("td:eq(1)").text();
        var name = $(this).closest("tr").find("td:eq(2)").text();

        Swal.fire({
            icon: 'warning',
            title: 'Bạn có chắc muốn xóa?',
            text: "Bạn đang cố gắng xóa danh mục '" + name + "'",
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                Delete(id);
            }
        });
    });

});

function Load() {
    $.ajax({
        url: '/Home/LoadCategory',
        type: 'GET',
        success: function (res) {
            var items = res.data;
            var html = "";
            if (items.length === 0) {
                html += "<tr><td colspan='9' class='text-center'>Không có dữ liệu</td></tr>";
            } else {
                for (let i = 0; i < items.length; i++) {
                    var NgayBatDau = unixTimestampToDate(items[i].NgayBatDau);
                    var CapNhatLanCuoi = unixTimestampToDate(items[i].CapNhatLanCuoi);
                    var index = i + 1;
                    html += "<tr>";
                    html += "<td>" + index + "</td>"
                    html += "<td>" + items[i].MaDanhMuc + "</td>";
                    html += "<td>" + items[i].TenDanhMuc + "</td>";
                    html += "<td>" + NgayBatDau + "</td>";
                    html += "<td>" + CapNhatLanCuoi + "</td>";
                    html += "<td><button class='btn btn-icon btn-hover btn-sm btn-rounded pull-right btnEdit' data-toggle='modal' data-target='#EditModal'> <i class='anticon anticon-edit'></i></button><button class='btn btn-icon btn-hover btn-sm btn-rounded pull-right btnDelete'> <i class='anticon anticon-delete'></i></button> </td> ";
                    html += "</tr>";
                }
            }
            $('#showdata').html(html);
        }
    });
};

function Add() {
    var name = $("#TenDanhMuc").val().trim();

    $.ajax({
        url: '/Home/AddCategory',
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify({
            name_cate: name
        }),
        success: function (res) {
            if (res.status === "Thêm thành công") {
                Swal.fire({
                    icon: 'success',
                    title: res.status,
                    showConfirmButton: false,
                    timer: 2000
                });
                Load();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: res.status,
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: jqXHR.responseText || 'Có lỗi xảy ra khi thêm mới dữ liệu!',
            });
        }
    });
}


function Update() {
    var cat = {
        id_cate: $('#EditMaDanhMuc').val(),
        name_cate: $('#EditTenDanhMuc').val(),
        time_Start: $("#EditTimeStart").val()
    };

    $.ajax({
        type: 'POST',
        url: '/Home/Update',
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(cat),
        success: function (response) {
            Swal.fire({
                icon: 'success',
                title: "Cập nhật thành công",
                showConfirmButton: false,
                timer: 2000
            });
            Load();
            $('#EditModal').modal('hide');
        },
        error: function () {
            Swal.fire({
                icon: 'error',
                title: 'Oops...',
                text: 'Có lỗi xảy ra khi cập nhật dữ liệu!'
            });
        }
    });
};

function Delete(id) {
    $.ajax({
        type: 'POST',
        url: '/Home/Delete',
        data: { id: id },
        success: function (response) {
            if (response.status === "Xóa thành công") {
                Swal.fire({
                    icon: 'success',
                    title: response.status,
                    showConfirmButton: false,
                    timer: 2000
                });
                Load();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Oops...',
                    text: response.status,
                });
            }
        },
    });
};

function GetbyID(MaDanhMuc) {
    $.ajax({
        url: '/Home/GetByID',
        type: 'GET',
        data: { id: MaDanhMuc },
        success: function (res) {
            $("#EditMaDanhMuc").val(res.data.MaDanhMuc);
            $("#EditTenDanhMuc").val(res.data.TenDanhMuc);
            $("#EditTimeStart").val(res.data.TimeStart);
        }
    });
};

function unixTimestampToDate(unixTimestamp) {
    var date = new Date(unixTimestamp * 1000);
    var weekdays = ['Chủ Nhật', 'Thứ 2', 'Thứ 3', 'Thứ 4', 'Thứ 5', 'Thứ 6', 'Thứ 7'];
    var dayOfWeek = weekdays[date.getDay()];
    var month = ("0" + (date.getMonth() + 1)).slice(-2);
    var day = ("0" + date.getDate()).slice(-2);
    var year = date.getFullYear();
    var hours = ("0" + date.getHours()).slice(-2);
    var minutes = ("0" + date.getMinutes()).slice(-2);
    var seconds = ("0" + date.getSeconds()).slice(-2);
    var formattedDate = dayOfWeek + ', ' + day + "-" + month + "-" + year + " " + ', ' + hours + ":" + minutes + ":" + seconds;
    return formattedDate;
};

// Validation Kiểm tra nhập liệu phía Client
function XuLyNhapLieu() {
    $('#TenDanhMuc').on('input', function () {
        $('#TenDanhMucError').text('');
    });

    $('#TenDanhMuc').on('blur', function () {
        var inputValue = $(this).val().trim();
        if (!inputValue) {
            $('#TenDanhMucError').text('Vui lòng nhập tên danh mục');
        }
        else {
            $('#TenDanhMucError').text('');
        }
    });
};
