var Categories = []
//fetch categories from database
function LoadCategory(element) {
    if (Categories.length == 0) {
        //ajax function for fetch data
        $.ajax({
            type: "GET",
            url: '/home/getProductCategories',
            success: function (data) {
                Categories = data;
                //render catagory
                renderCategory(element);
            }
        })
    }
    else {
        //render catagory to the element
        renderCategory(element);
    }
}

function renderCategory(element) {
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(Categories, function (i, val) {
        $ele.append($('<option/>').val(val.CategoryID).text(val.CategortyName));
    })
}

//fetch products
function LoadProduct(categoryDD) {
    $.ajax({
        type: "GET",
        url: "/home/getProducts",
        data: { 'categoryID': $(categoryDD).val() },
        success: function (data) {
            //render products to appropriate dropdown
            renderProduct($(categoryDD).parents('.mycontainer').find('select.product'), data);
        },
        error: function (error) {
            console.log(error);
        }
    })
}

function renderProduct(element, data) {
    //render product
    var $ele = $(element);
    $ele.empty();
    $ele.append($('<option/>').val('0').text('Select'));
    $.each(data, function (i, val) {
        $ele.append($('<option/>').val(val.ProductID).text(val.ProductName));
    })
}

$(document).ready(function () {
    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;


        if (!($('#rate').val().trim() != '')) {
            isAllValid = false;
            $('#rate').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#rate').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');


            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-success').addClass('btn-danger');

            //remove id attribute from new clone row
            $('#rate,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#orderdetailsItems').append($newRow);

            //clear select data
            $('#rate').val('');
            $('#orderItemError').empty();
        }

    })

    //remove button click event
    $('#orderdetailsItems').on('click', '.remove', function () {
        $(this).parents('tr').remove();
    });

    $('#submit').click(function () {
        var isAllValid = true;

        //validate order items
        $('#orderItemError').text('');
        var list = [];
        var errorItemCount = 0;
        console.log("inside outside : ");
        $('#orderdetailsItems tr').each(function (index, ele) {
            console.log("inside : ", index);
            if ($('.rate', this).val() == "") {
                errorItemCount++;
                $(this).addClass('error');
            } else {
                var candidateItem = {
                    Candidate: $('.rate', this).val()
                }
                list.push(candidateItem);
                console.log("list : ", list);
            }
        })

        if (errorItemCount > 0) {
            $('#orderItemError').text(errorItemCount + " invalid entry in order item list.");
            isAllValid = false;
        }

        if (list.length == 0) {
            $('#orderItemError').text('At least 1 order item required.');
            isAllValid = false;
        }



        if ($('#description').val().trim() == '') {
            $('#description').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#description').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var model = {
                Name: $('#description').val().trim(),
                Candidates: list
            }

            console.log("Data :", model);
            $(this).val('Please wait...');

            $.ajax({
                type: 'POST',
                url: '/api/Election/CreateElection',
                data: model,
                //contentType: 'application/json',
                success: function (data) {

                    alert('Successfully saved');
                    //here we will clear the form
                    list = [];
                    $('#description').val('');
                    $('#orderdetailsItems').empty();

                    $('#submit').val('Save');
                },
                error: function (jqxhr, textstatus, errorthrown) {
                    jsonValue = jQuery.parseJSON(jqxhr.responseText);
                    alert(jsonValue.Message);
                    console.log("ERROR : ", jsonValue);
                    $('#submit').val('Save');
                }
            });
        }

    });

});

