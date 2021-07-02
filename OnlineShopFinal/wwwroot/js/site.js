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




$(document).ready(function () {

    document.getElementById("sbtotal").readOnly = true;
    document.getElementById("qt").readOnly = true;
    document.getElementById("sbtotal").style.color = "#black";

    totalIt();


    //1. Increment Decrement

    $('.quantitybtn').on('click', function () {
        var $row = $(this);
        var $row0 = $(this).closest("tr");

        var d = parseFloat($row0.find('.Id').val());
        var oldValue = $row.parent().find('.quantity').val();
        if ($row.hasClass('inc')) {

            var newVal = parseFloat(oldValue);

            var $row1 = $(this).closest("tr"),
                prce = parseFloat($row1.find('.price').val()),
                qnty = parseFloat($row1.find('.quantity').val()),
                productId = parseFloat($row1.find('.Id').val()),


                subTotal = (prce * qnty);


            $row1.find('.subtotal').val(isNaN(subTotal) ? 0 : subTotal);



            $.ajax({
                type: 'GET',
                url: '/Home/GetData/?id=' + d,
                dataType: 'JSON',
                success: function (data) {

                    newVal = parseFloat(oldValue) + 1;
                    $row.parent().find('.quantity').val(newVal);
                    if (newVal <= data) {



                        $row.parent().find('.quantity').val(newVal);
                        qnty = newVal;
                        subTotal = (prce * qnty);
                        $row1.find('.subtotal').val(subTotal);
                        console.log(qnty);
                        totalIt();
                    }
                    else {
                        alert("Not Available!");
                        newVal = newVal - 1;
                        $row.parent().find('.quantity').val(newVal);

                        return;
                    }
                },
                error: function (err) {
                    console.log(err)
                }
            });



            totalIt();
            $.ajax({
                type: "POST",
                url: "/Home/Cart",
                data: { number1: qnty, number2: subTotal, number3: productId },
                dataType: "text",
                success: function (msg) {
                    console.log();
                },
                error: function (req, status, error) {
                    console.log();
                }
            });


        }
        else if ($row.hasClass('dec')) {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
                $row.parent().find('.quantity').val(newVal);
                var $row1 = $(this).closest("tr"),
                    prce = parseFloat($row1.find('.price').val()),
                    qnty = parseFloat($row1.find('.quantity').val()),
                    productId = parseFloat($row1.find('.Id').val()),


                    subTotal = (prce * qnty);
                $row1.find('.subtotal').val(isNaN(subTotal) ? 0 : subTotal);
                $row1.find('.quantity').val(isNaN(qnty) ? 0 : qnty);

                $.ajax({
                    type: "POST",
                    url: "/Home/Cart",
                    data: { number1: qnty, number2: subTotal, number3: productId },
                    dataType: "text",
                    success: function (msg) {
                        console.log(msg);
                    },
                    error: function (req, status, error) {
                        console.log(msg);
                    }
                });




                totalIt();


            } else {
                newVal = 0;
            }
        }
        $row.parent().find('.quantity').val(newVal);
    });


    //2. calculation
    function totalIt() {
        var total = 0;
        $(".subtotal").each(function () {
            var val = this.value;
            total += val == "" || isNaN(val) ? 0 : parseFloat(val);
        });
        $('form').find("#Total").val(total);
    }




});


