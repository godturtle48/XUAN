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
    // Start print invoice
    $(".printResultSurvey").click(function () {
        let content = $(".result-survey").html();
        printResultSurvey(content);
    });

    function printResultSurvey(content, script) {
        var printWindow = window.open('', '', 'height=800,width=1200');
        printWindow.document.write('<html><head><title>Print Result </title>');
        printWindow.document.write('</head><body >');
        printWindow.document.write(content);
        printWindow.document.write('</body></html>');
        printWindow.document.close();
        printWindow.print();
    }

    // End print invoice

});




