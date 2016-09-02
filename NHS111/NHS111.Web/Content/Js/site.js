//// JavaScript Document

$(document).ready(function () {

    //outcome page no-javascript support
    $(".noScript").hide();

    //hide select address on page load
    //$("#AddressList").hide();
    //$("#Next").hide();
    $("#AdditionalInfo").hide();
    $("#CurrentLocation").hide();
    $("#ManualEntryHome").hide();
    //$("#AddressList").hide();

    //show elements on page load
    $("#OutcomeFour #Next").show();


    //hide select address on page load
    //	$("#AddressList").hide();
    //	$("#Next").hide();


    //	$("input[type=submit]").attr('disabled', 'disabled');
    //	$("button[type=submit]").attr('disabled', 'disabled');
    //	$("a.submitButton").attr('disabled', 'disabled');


    // enable buttons where needed to  be active
    $("#OutcomeQuestion button[type=submit]").removeAttr('disabled');
    $("button.changeAddress[type=submit]").removeAttr('disabled');



    // enable button on OutcomeOne page
    //	$("#OutcomeQuestion button[type=submit]").removeAttr('disabled');

    //    //on postcode submit show address list
    //	$("#SearchPostcode").click(function () {
    //	    $("#AddressList").show();
    //	    $("#Next").show();
    //	});



    //    // check if value is entered before enabling button    
    //	$('#Postcode').bind('input', function () {
    //	    $("#SubmitAddress").removeAttr('disabled');
    //	    $("a.submitButton").removeAttr('disabled');
    //	    $("#SearchPostcode").removeAttr('disabled');
    //	});


    //    // check if value is selected from dropdown
    //	 $("#PickAddress").change(function(){
    //	     $("#Next").removeAttr('disabled');

    //	});


    //on postcode submit show address list
    //$("#SearchPostcode").click(function () {
    //    $("#AddressList").show();
    //    $("#Next").show();
    //});

    // at home yes/no click function
    //on postcode submit show address list
    $(".yesAtHome").click(function () {
        $("#AdditionalInfo").hide();
        $("#CurrentLocation").hide();
        //$("#PersonalDetails #Next").removeAttr('disabled');
    });

    $(".notAtHome").click(function () {
        $("#AdditionalInfo").show();
        $("#Postcode").focus();
        $("#CurrentLocation").show();
        //$("#PersonalDetails #Next").attr('disabled', 'disabled');

    });



    // show manual entry
    $(".manualEntry").click(function () {
        $("#AddressList").hide();
        $("#AddressDetails").show();
        $(".enterManually").hide();
        $("#PC").hide();


    });

    //$(".searchPostcode").click(function () {
    //    $("#AddressList").show();
    //});



    //for show & tell only
    $("#ManualEntryHome").show();


    //for disposition2 page
    $('.findEmergency .postcode').bind('input', function () {
        $(".searchPostcode").removeAttr('disabled');

    });

    $('.findEmergency input.postcode').blur(function () {
        if ($(this).val().length === 0) {
            $(".searchPostcode[type=submit]").attr('disabled', 'disabled');
        }
    });


    $('.findEmergency input.postcode').bind('keyup', function (e) {

        if (e.which == 8) {//backspace detected to disable the button

            $(".searchPostcode[type=submit]").attr('disabled', 'disabled');

        }
    });

});