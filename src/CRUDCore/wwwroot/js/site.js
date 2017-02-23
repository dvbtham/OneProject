$('#AlertBox').removeClass('hide');

//SET DEFAULT DATETIME INTO DATETIMEPICKER
var d = new Date();

var month = d.getMonth() + 1;
var day = d.getDate();

var output = d.getFullYear() + '-' +
(month < 10 ? '0' : '') + month + '-' +
(day < 10 ? '0' : '') + day;
var id = $("#myId").data('id');
if (id < 0)
    $("#FromDate, #DeadlineDate").val(output);

//$('#AlertBox').delay(3000).slideUp(500);
