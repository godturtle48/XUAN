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
    //start  ajax delete subject

    $(".delete-subject").on("click", function () {
        rowCurrent = $(this).parent().parent();
        idCurrent = parseInt($(this).attr("id"));
        urldetail = '/Admin/SubjectManager/Delete';
        $.ajax({
            type: "GET",
            url: urldetail,
            data: { id: idCurrent },
            success: function (subject) {
                $("#DsubjectName").val(subject.Name);
                $("#DsubjectCode").val(subject.SubjectCode);
                $("#DsubjectCredit").val(subject.CreditNumber);
                $("#DsubjectRoom").val(subject.ClassRoom);
                $("#DsubjectTime").val(subject.TimeTeach);
                $(".delete-subject-form").show();
            }
        });
    });

    $(".bottom-button").on("click", ".btn-delete", function () {
        $(".delete-subject-form").fadeOut(700);
        rowCurrent.hide(1000);
        urldetail = '/Admin/SubjectManager/Delete';
        $.ajax({
            type: "POST",
            url: urldetail,
            data: { id: idCurrent },
            success: function (data) {

            }
        });
    });
    $(".bottom-button").on("click", ".btn-cancle", function () {
        $(".delete-subject-form").fadeOut(700);
    });
    //end  ajax delete subject

});

