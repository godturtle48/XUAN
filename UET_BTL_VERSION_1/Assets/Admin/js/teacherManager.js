$(document).ready(function () {

    $("#fileupload").change(function () {
        if ($(this).val() !== "") {
            $('#importdata').prop("disabled", false);
        } else {
            $('#importdata').prop("disabled", true);
        }
    });

    //start  ajax delete teacher
    var rowCurrent = null;
    var idCurrent = null;
   
    $(".delete-teacher").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Admin/TeacherManager/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {
                $("#DteacherName").val(data.teacher.Name);
                $("#DteacherEmail").val(data.teacher.Email);
                $("#DteacherUsername").val(data.teacher.UserName);
                $("#DteacherPassword").val(data.teacher.PassWord);
                if (data.status === 0) {
                    $(".delete-teacher-form h5:eq(0)").text(" Không thể xóa ");
                    $(".delete-teacher-form tr:eq(0)").html("<td style='text-align: center;font-weight:bold; color: red; font-size: 17px; ' colspan='2'>GIẢNG VIÊN NÀY ĐANG DẠY MỘT HỌC PHẦN</td>");
                    $(".delete-teacher-form .bottom-button:eq(0)").html("");
                    $(".delete-teacher-form .bottom-button:eq(1)").html("<a class='btn btn-success btn-cancle'>Đóng</a>");
                } else {
                    $(".delete-teacher-form h5:eq(0)").text("Xóa giảng viên");
                    $(".delete-teacher-form tr:eq(0)").html("");
                    $(".delete-teacher-form .bottom-button:eq(0)").html("<a class='btn btn-danger btn-delete'>Xóa</a>");
                    $(".delete-teacher-form .bottom-button:eq(1)").html("<a class='btn btn-success btn-cancle'>Hủy bỏ</a>");
                }
                $(".delete-teacher-form").show();
            }
        });
    });

    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-teacher-form").fadeOut(700);
        rowCurrent.hide(1000);
        urldetail = '/Admin/TeacherManager/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });

   
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-teacher-form").fadeOut(700);
    });
    //end  ajax delete teacher
    //start  ajax create and edit teacher
    $('.btn-close').click(function () {
        $('.create-teacher').hide();
        $('.input2').removeClass('border-red');
        $('[data-toggle="tooltip"]').tooltip("hide");
    });

    $('#add-button').click(function () {
        $('.input2').val("");
        $(".create-teacher h5:eq(0)").text("Thêm mới giảng viên");
        $(".create-teacher a:eq(0)").text("Thêm mới");
        $('.create-teacher').show();
    });
    var rowEditCurrent = null;
    $('tbody').on("click", ".edit-teacher", function () {
        rowEditCurrent = $(this).parent().parent();
        $(".create-teacher h5:eq(0)").text("Chỉnh sửa giảng viên");
        $(".create-teacher a:eq(0)").text("Lưu");
        $("#teacherName").val($(this).parent().parent().children("td:eq(1)").text().trim());
        $("#idTeacher").val($(this).attr("id"));
        $("#teacherEmail").val($(this).parent().parent().children("td:eq(2)").text().trim());
        $("#teacherUsername").val($(this).parent().parent().children("td:eq(3)").text().trim());
        $("#teacherPassword").val($(this).parent().parent().children("td:eq(4)").text().trim());
        $('.create-teacher').show();
    });

    $('#teacherName').blur(function () {
        $(this).val(ChuanhoaTen($(this).val()));
    });

    function ChuanhoaTen(ten) {
        dname = ten;
        ss = dname.split(' ');
        dname = "";
        for (i = 0; i < ss.length; i++)
            if (ss[i].length > 0) {
                if (dname.length > 0) dname = dname + " ";
                dname = dname + ss[i].substring(0, 1).toUpperCase();
                dname = dname + ss[i].substring(1).toLowerCase();
            }
        return dname;
    }
    // kiểm tra email giáo viên (4-10 chữ cái + @vnu.edu)
    function isTeacherEmail(e) {
        var regex = /(^([a-zA-Z]{4,10}))((@vnu.edu.vn)$)/;
        if (regex.test(e)) return true;
        return false;
    }
   
    $('.input2').blur(function () {
        if ($(this).val() !== "") {
            $(this).removeClass('border-red');
        } else {
            $(this).addClass('border-red');
        }
    });
    $(".btn-submit").click(function (event) {
        let check = true;
        let checkEdit = false;
        if ($(".create-teacher a:eq(0)").text() === "Lưu") {
            checkEdit = true;
        }
        $('.input2').removeClass('border-red');
        $('.input2').each(function () {
            if ($(this).val().trim() === "" || $(this).val().length < 5 || $(this).val().length > 200) {
                $(this).addClass('border-red');
                check = false;
                $(this).tooltip("show");
            }
        });
        if (!isTeacherEmail($("#teacherEmail").val())) {
            $('#teacherEmail[data-toggle="tooltip"]').tooltip("show");
            check = false;
        }
        if (check) {
            if (checkEdit) {
                PeformAjaxTeacher("/Admin/TeacherManager/Edit");
            } else {
                PeformAjaxTeacher("/Admin/TeacherManager/Create");
            }
        }
    });

    function PeformAjaxTeacher(urlDetail) {
        var frm = $('#addTeacher');
        $.ajax({
            type: "POST",
            url: urlDetail,
            data: frm.serialize(),
            success: function (data) {
                if (data.status === 0) {
                    $('#teacherUsername[data-toggle="tooltip"]').tooltip("show");
                } else if (data.status === 1) {
                    $('.create-teacher').hide();
                    alert("create success");
                } else {
                    rowEditCurrent.hide();
                    $('.create-teacher').hide();
                    rowEditCurrent.children("td:eq(1)").text($("#teacherName").val());
                    rowEditCurrent.children("td:eq(2)").text($("#teacherEmail").val());
                    rowEditCurrent.children("td:eq(3)").text($("#teacherUsername").val());
                    rowEditCurrent.children("td:eq(4)").text($("#teacherPassword").val());
                    rowEditCurrent.fadeIn(700);
                }
            }
        });

    }

    //end  ajax create teacher
});

