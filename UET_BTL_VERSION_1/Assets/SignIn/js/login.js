$(document).ready(function () {

        //start  ajax create and edit teacher
        $('.btn-close').click(function () {
            $('.verify-email').hide();
            $('.input2').removeClass('border-red');
        });

        $('#forgot').click(function () {
            $('.input2').val("");
            $('.verify-email').show();
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
            $('.input2').removeClass('border-red');
            $('.input2').each(function () {
                if ($(this).val().trim() === "" ) {
                    $(this).addClass('border-red');
                    check = false;
                }
            });
            if (check) {
                $('.verify-email').hide();
                $(".load").show();
                PeformAjaxVerifyEmail("/SignIn/Home/GetPassWord");
            }
});

function PeformAjaxVerifyEmail(urlDetail) {
    var frm = $('#verfiry');
    $.ajax({
        type: "POST",
        url: urlDetail,
        data: frm.serialize(),
        success: function (data) {
            if (data.status === 1) {
                $(".load").hide();
                $('.input2').removeClass('border-red');
                alert("Vui lòng kiểm tra email để lấy lại mật khẩu");
            } else {
                $(".load").hide();
                $('.input2').removeClass('border-red');
                alert("Username hoặc email không hợp lệ");
            }
        }
    });

}

    //end  ajax 

});