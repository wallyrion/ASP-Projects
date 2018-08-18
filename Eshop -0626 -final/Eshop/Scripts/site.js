function LoadData(category) {
    alert(category);
    $("#tblJobAreas tbody tr").remove();

    $.ajax({

        type: 'GET',
        url: '@Url.Action("Goods","Home")',
        dataType: 'json',
        data: { categoryName: category },

        success: function (data) {
            console.log("success", data);
            var items = '';
            $.each(data, function (i, item) {
                var rows = "<tr>"
                    + "<td class='jobAreaTd'>" + item.Name + "</td>" + "<td><button onclick='deleteJobArea(" + item.Id + ")' class='btn btn-warning btn-xs' type='button'>delete</button></td>"
                    + "</tr>";
                $('#tblJobAreas tbody').append(rows);
            });
        },
        error: function (ex) {
            console.log('error ');
        }

    });
    return false;
}

