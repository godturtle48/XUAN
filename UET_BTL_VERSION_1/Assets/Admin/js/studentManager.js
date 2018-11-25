$(document).ready(function () {

    $("#fileupload").change(function () {
        if ($(this).val() !== "") {
            $('#importdata').prop("disabled", false);
        } else {
            $('#importdata').prop("disabled", true);
        }
    });

    var rowCurrent = null;
    var idCurrent = null;
   
    //start  ajax delete student
   
    $(".delete-student").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Admin/StudentManager/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (student) {
                $("#DstudentName").val(student.Name);
                $("#DstudentEmail").val(student.Email);
                $("#DstudentUsername").val(student.UserName);
                $("#DstudentPassword").val(student.PassWord);
               
                $(".delete-student-form h5:eq(0)").text("Xóa sinh viên");
                $(".delete-student-form .bottom-button:eq(0)").html("<a class='btn btn-danger btn-delete'>Xóa</a>");
                $(".delete-student-form .bottom-button:eq(1)").html("<a class='btn btn-success btn-cancle'>Hủy bỏ</a>");
                $(".delete-student-form").show();
            }
        });
    });

    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-student-form").fadeOut(700);
        rowCurrent.hide(1000);
        urldetail = '/Admin/StudentManager/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-student-form").fadeOut(700);
    });
    //end  ajax delete student
    //end  ajax create and edit student
    $('.btn-close').click(function () {
        $('.create-student').hide();
        $('.input2').removeClass('border-red');
        $('[data-toggle="tooltip"]').tooltip("hide"); 
    });

    $('#add-button').click(function () {
        $('.input2').val("");
        $(".create-student h5:eq(0)").text("Thêm mới sinh  viên");
        $(".create-student a:eq(0)").text("Thêm mới");
        $('.create-student').show();
    });
    var rowEditCurrent = null;
    $('tbody').on("click", ".edit-student", function () {
        rowEditCurrent = $(this).parent().parent();
        $(".create-student h5:eq(0)").text("Chỉnh sửa sinh viên");
        $(".create-student a:eq(0)").text("Lưu");
        $("#idStudent").val($(this).attr("id"));

        $("#studentName").val($(this).parent().parent().children("td:eq(1)").text().trim());
        $("#studentId").val($(this).parent().parent().children("td:eq(4)").text().trim());
        $("#studentClass").val($(this).parent().parent().children("td:eq(2)").text().trim());
        $("#studentEmail").val($(this).parent().parent().children("td:eq(3)").text().trim());
        $("#studentUsername").val($(this).parent().parent().children("td:eq(4)").text().trim());
        $("#studentPassword").val($(this).parent().parent().children("td:eq(5)").text().trim());
        $('.create-student').show();
    });
    $('#studentName').blur(function () {
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
     // Kiểm tra email vnu
    function isStudentEmail(e) {
        var regex = /(^(\d{8}))((@vnu.edu.vn)$)/;
        if (regex.test(e)) return true;
        return false;
    }
    // Kiểm tra mã minh viên (8 chữ số)
    function isStudentId(e) {
        var regex = /(^(\d{8})$)/;
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
        if ($(".create-student a:eq(0)").text() === "Lưu") {
            checkEdit = true;
        }
        $('.input2').removeClass('border-red');
        $('.input2').each(function () {
            if ($(this).val() === "" || $(this).val().length < 5 || $(this).val().length > 200) {
                $(this).addClass('border-red');
                check = false;
                $(this).tooltip("show"); 
            }
        });
        if (!isStudentEmail($("#studentEmail").val())) {
             $('#studentEmail[data-toggle="tooltip"]').tooltip("show");  
            check = false;
            }
        if (!isStudentId($("#studentId").val())) {
            $('#studentId[data-toggle="tooltip"]').tooltip("show");  
            check = false;
        }
        if (check) {
            if (checkEdit) {
                PeformAjaxStudent("/Admin/StudentManager/Edit");
            } else {
                PeformAjaxStudent("/Admin/StudentManager/Create");
            }
        }
    });

    function PeformAjaxStudent(urlDetail) {
        var frm = $('#addStudent');
            $.ajax({
                type: "POST",
                url: urlDetail,
                data: frm.serialize(),
                success: function (data) {
                    if (data.status === 0) {
                        $('#studentUsername[data-toggle="tooltip"]').tooltip("show");
                    } else if (data.status === 1) {
                        $('.create-student').hide();
                        alert("create success");
                    } else {
                        rowEditCurrent.hide();
                        $('.create-student').hide();
                        rowEditCurrent.children("td:eq(1)").text($("#studentName").val());
                        rowEditCurrent.children("td:eq(2)").text($("#studentClass").val());
                        rowEditCurrent.children("td:eq(3)").text($("#studentEmail").val());
                        rowEditCurrent.children("td:eq(4)").text($("#studentUsername").val());
                        rowEditCurrent.children("td:eq(5)").text($("#studentPassword").val());
                        rowEditCurrent.fadeIn(700);
                    }
                }
            });
   
    }
    
    //end  ajax create student
});

