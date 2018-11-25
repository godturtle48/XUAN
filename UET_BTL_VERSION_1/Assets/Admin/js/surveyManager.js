$(document).ready(function () {
   
    var rowCurrent = null;
    var idCurrent = null;
    //start  ajax delete ContentSurvey
    $("tbody").on("click",".delete-ContentSurvey",function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Admin/SurveyManager/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {
                $("#DsurveyName").val(data.Text);
                $(".delete-survey-form").show();
            }
        });
    });
    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-survey-form").fadeOut(700);
        rowCurrent.hide(1000);
        rowCurrent.remove();
        urldetail = '/Admin/SurveyManager/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-survey-form").fadeOut(700);
    });
    //end  ajax delete ContentSurvey
    //end  ajax create and edit survey -----------------------------------------------------------------------------------
    $('.btn-close').click(function () {
        $('.create-survey').hide();
        $('.input2').removeClass('border-red');
    });
    $('#add-button').click(function () {
        $('.input2').val("");
        $(".create-survey h5:eq(0)").text("Thêm mới tiêu chí");
        $(".create-survey a:eq(0)").text("Thêm mới");
        $(".create-survey td:eq(0)").text("(Lưu ý khi thêm tiêu chí mới sẽ xóa hết dữ liệu sinh viên đã đánh giá)");
        $('.create-survey').show();
    });
    var rowEditCurrent = null;
    $('tbody').on("click", ".edit-survey", function () {
        rowEditCurrent = $(this).parent().parent();
        $(".create-survey h5:eq(0)").text("Chỉnh sửa tiêu chí");
        $(".create-survey td:eq(0)").text("");
        $(".create-survey a:eq(0)").text("Lưu");
        $("#idContentSurvey").val($(this).attr("id"));
        $("#surveyName").val($(this).parent().parent().children("td:eq(1)").text().trim());
        $('.create-survey').show();
    });
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
        if ($(".create-survey a:eq(0)").text() === "Lưu") {
            checkEdit = true;
        }
        $('.input2').removeClass('border-red');
        $('.input2').each(function () {
            if ($(this).val().trim() === "") {
                $(this).addClass('border-red');
                check = false;
            }
        });
        if (check) {
            if (checkEdit) {
                PeformAjaxSurvey("/Admin/SurveyManager/Edit");
            } else {
                PeformAjaxSurvey("/Admin/SurveyManager/Create");
            }
        }
    });
    function appendRowSurvey(id, data, index) {
        $(".class-content-clone td:eq(2)").html("");
        $(".class-content-clone td:eq(2)").append("<a class='btn btn-warning edit-survey' id='" + id + "'>Sửa</a>");
        $(".class-content-clone td:eq(2)").append(" <a class='btn btn-danger delete-ContentSurvey' id='" + id + "'>Xóa</a>");
        $(".class-content-clone td:eq(0)").text(index);
        $(".class-content-clone td:eq(1)").text(data);
        var rownew = $(".class-content-clone tr:eq(0)").clone(true);
        $(".content-teacher tbody").append(rownew);
    }
    function PeformAjaxSurvey(urlDetail) {
        var frm = $('#addSurvey');
        $.ajax({
            type: "POST",
            url: urlDetail,
            data: frm.serialize(),
            success: function (data) {
                if (data.status === 0) {
                    $('#surveyName[data-toggle="tooltip"]').tooltip("show");
                } else if (data.status === 1) {
                    $('.create-survey').hide();
                    var index = parseInt($(".content-teacher .table-striped:last td:eq(0)").text()) + 1;
                    if (Number.isNaN(index)) {
                        index = 1;
                    }
                    appendRowSurvey(data.id, data.content.Text, index);
                }else{
                    rowEditCurrent.hide();
                    $('.create-survey').hide();
                    rowEditCurrent.children("td:eq(1)").text($("#surveyName").val());
                    rowEditCurrent.fadeIn(700);
                }
            }
        });

    }

    //end  ajax create survey

});

