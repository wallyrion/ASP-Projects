#
(document).on('click',
    '.btnExport',
    function(e) {
        e.preventDefault();

        var link = $(this);
        $(this).hide();
        var url = link.attr('data-src');
        $.ajax(
            {
                url: url,
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',

                data: {
                },
                type: "GET",
                success: function() {
                    link.show();
                    window.location = url;
                }

            })
    });