$('#AlertBox').removeClass('hide');

var d = new Date();

var month = d.getMonth() + 1;
var day = d.getDate();

var output = d.getFullYear() + '-' +
(month < 10 ? '0' : '') + month + '-' +
(day < 10 ? '0' : '') + day;

$('#FromDate, #DeadlineDate').datepicker({
    language: 'en',
    todayButton: new Date(),
    clearButton: true
});

var id = $("#myId").data('id');
if (id < 0)
    $("input[type='date']").val(output);

