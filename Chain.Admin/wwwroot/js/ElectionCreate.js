
$(document).ready(function () {
    //Add button click event
    $('#add').click(function () {
        //validation and add order items
        var isAllValid = true;


        if (!($('#candidateAddess').val().trim() != '')) {
            isAllValid = false;
            $('#candidateAddess').siblings('span.error').css('visibility', 'visible');
        }
        else {
            $('#candidateAddess').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var $newRow = $('#mainrow').clone().removeAttr('id');


            //Replace add button with remove button
            $('#add', $newRow).addClass('remove').val('Remove').removeClass('btn-light').addClass('btn-danger');

            //remove id attribute from new clone row
            $('#candidateAddess,#add', $newRow).removeAttr('id');
            $('span.error', $newRow).remove();
            //append clone row
            $('#orderdetailsItems').append($newRow);

            //clear select data
            $('#candidateAddess').val('');
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
            if ($('.candidateAddess', this).val() == "") {
                errorItemCount++;
                $(this).addClass('error');
            } else {
                var candidateItem = {
                    Candidate: $('.candidateAddess', this).val()
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



        if ($('#name').val().trim() == '') {
            $('#name').siblings('span.error').css('visibility', 'visible');
            isAllValid = false;
        }
        else {
            $('#name').siblings('span.error').css('visibility', 'hidden');
        }

        if (isAllValid) {
            var model = {
                Name: $('#name').val().trim(),
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
                    $('#name').val('');
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

