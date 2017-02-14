var del = {
    init: function () {
        del.resEvents();
    },
    resEvents: function () {
        $("a#btnDelete").click(function (e) {
            e.preventDefault();
            var result = confirm("Are you sure?");
            if (result) {
                var id = $(this).data("id");
                del.deleteStudent(id);
            }
        })
    },
    deleteStudent: function (id) {
        $.ajax({
            url: "/Students/Delete",
            type: "POST",
            dataType: "json",
            data: {
                id: id
            },
            success: function (res) {
                if (res.status) {
                    alert("Xóa thành công");
                    window.location.reload();
                }
                else {
                    alert("Xóa không thành công");
                }
            }
        })
    }
};
del.init();