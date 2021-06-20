// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    var placeHolderElement = $('#placeHolderHere');
    $('button[data-toggle="ajax-modal"]').click(function (event) {

        var url = $(this).data('url');
        //var decodeUrl = decodeURIComponent(url);
        $.get(url).done(function (data) {
            placeHolderElement.html(data);
            placeHolderElement.find('.modal').modal('show');
        })
    })

    placeHolderElement.on('click', '[data-save="modal"]', function (event) {
        var form = $(this).parents('.modal').find('form');
        var actionUrl = form.attr('action');
        var sendData = form.serialize();
        $.post(actionUrl, sendData).done(function (data) {
            placeHolderElement.find('.modal').modal('hide');
        })
    })

})
//onload ajax call
var designationList;
//$.ajax({
//    type: 'GET',
//    url: '/Employees/GellAllDesignation',
//    dataType: 'JSON',
//    success: function (data) {
//        designationList = data;
//    },
//    error: function (err) {
//        console.log(err)
//    }
//})

///Show modal as a pop up
showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');


            $("#Designation").change(function () {
                var d = $("#Designation option:selected").val();
                $.ajax({
                    type: 'GET',
                    url: '/Employees/GetDesignationSalary/?id=' + d,
                    dataType: 'JSON',
                    success: function (data) {
                        console.log("salary", data)
                        var salary = document.getElementById("Salary");
                        salary.value = data;
                    },
                    error: function (err) {
                        console.log(err)
                    }
                })
            });
            $("#Designation").trigger("change");
        }
    })
}
showInPopup1 = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');

        }
    })
}
//for post data
jQueryAjaxPost = form => {
    try {
        $.ajax({
            type: 'POST',
            url: form.action,
            data: new FormData(form),
            contentType: false,
            processData: false,
            success: function (res) {
                console.log("view all", res)
                if (res.isValid) {

                    $('#view-all').html(res.html)
                    $('#form-modal .modal-body').html('');
                    $('#form-modal .modal-title').html('');
                    $('#form-modal').modal('hide');
                }
                else
                    $('#form-modal .modal-body').html(res.html);
            },
            error: function (err) {
                console.log('fgdfgfgfd', err)
            }
        })
        //to prevent default form submit event
        return false;
    } catch (ex) {
        console.log(ex)
    }
}

/*-- Product Quantity --*/
$('.qtybtn').on('click', function () {


    var d = $("#hiddenId").val();
    
    var $button = $(this);

    var oldValue = $button.parent().find('input').val();
    
    var newVal = parseFloat(oldValue);
    if ($button.hasClass('inc')) {
        $.ajax({
            type: 'GET',
            url: '/Home/GetData/?id=' + d,
            dataType: 'JSON',
            success: function (data) {

                newVal = parseFloat(oldValue) + 1;
                $button.parent().find('input').val(newVal);
                if (newVal > data) {
                    alert("Not Available!");
                    newVal = newVal - 1;
                    $button.parent().find('input').val(newVal);
                    console.log(newVal);
                    return;
                }

            },
            error: function (err) {
                console.log(err)
            }
        });


    } else {
        newVal = newVal - 1;
        // Don't allow decrementing below zero
        if (oldValue <= 0) {
            newVal = 0;
        }
    }
    $button.parent().find('input').val(newVal);
});






///for delete
Delete = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {

            if (res.isValid) {
                $('#view-all').html(res.html)
                toastr.success("Delete Success");
            } else {
                toastr.warning("Delete Wwrong");
            }
        },
        error: function (res) {
            console.log(res);
        }
    });
}

