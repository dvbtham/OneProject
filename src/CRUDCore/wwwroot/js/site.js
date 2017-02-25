$('#AlertBox').removeClass('hide');

//SET DEFAULT DATETIME INTO DATETIMEPICKER
var d = new Date();

var month = d.getMonth() + 1;
var day = d.getDate();

var output = d.getFullYear() + '-' +
(month < 10 ? '0' : '') + month + '-' +
(day < 10 ? '0' : '') + day;

$('#FromDate, #DeadlineDate').datepicker({
    language: 'en'
});

var id = $("#myId").data('id');
if (id < 0)
    $("input[type='date']").val(output);

$("#unitPer").off('keypress').on("keypress", function () {
    //validator();
});
$('html body').on('click', function () {
    //validator();
})
function validator() {
    var unitPer = $("#unitPer");
    if (unitPer.val().length == 0) {
        $(".input-icon-right > i").css('left', '10px');
    }
    else
        if (unitPer.val().length == 1) {
            $(".input-icon-right > i").css('left', '17px');
        } else
            if (unitPer.val().length == 2) {
                $(".input-icon-right > i").css('left', '25px');
            }

    if (unitPer.val() < 0 || unitPer.val() > 100) {
        alert('Vui lòng chỉ nhập tới số từ 0 đến 100');
        return;
    }
}

//$('#AlertBox').delay(3000).slideUp(500);
