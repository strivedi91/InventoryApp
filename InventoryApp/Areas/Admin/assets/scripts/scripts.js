jQuery( document ).ready(function() {
    
	var bodyhight = jQuery(document).height();
	jQuery(".dashboard-menu").height(bodyhight - 58);
	jQuery(".charity-main").height(bodyhight);
	jQuery(".admin.login").height(bodyhight);
	var loginpadding = bodyhight;
	if(bodyhight < 450)
	{
		loginpadding = parseInt(bodyhight/10);
	}
	else
	{
		loginpadding = parseInt(bodyhight/4);
	}
	
	jQuery(".admin.login").css("padding-top", loginpadding+"px");
	var compleft = jQuery(".compaigns-form.left").height();
	var compright = jQuery(".compaigns-form.right").height();
	if(compleft > compright)
	{
		jQuery(".compaigns-form.right").height(compleft);
		jQuery(".compaigns-form.left").height(compleft);
	}
	else
	{
		jQuery(".compaigns-form.right").height(compright);
		jQuery(".compaigns-form.left").height(compright);
	}
	
	jQuery("#flcoupon, #txtcompanylogo").on('change', function() {
         
		 var filename = jQuery(this).val().replace(/.*(\/|\\)/, '');
		 if(filename != "")
		 {
			jQuery(".fileUpload span").html(filename);
		 }
	});
	
	jQuery( ".fileUpload span" ).click(function() {
	  jQuery( "#flcoupon" ).click();
	});

    jQuery(".fileUpload span").click(function () {
	    jQuery("#CouponCode").click();
	});
	
	jQuery( ".fileUpload span.companylogo" ).click(function() {
	    jQuery("#CompanyLogo").click();
	});

	jQuery(".fileUpload span.companylogo").click(function () {
	    jQuery("#CharityLogo").click();
	});
	
	jQuery("#flimage").on('change', function() {
		readURL(this);
	});
	
	jQuery( ".div-plus span" ).click(function() {
		jQuery(".charity-main").fadeIn("slow");
	});
	jQuery( ".charity-box .fa-times" ).click(function() {
		jQuery(".charity-main").fadeOut("slow");
	});
	jQuery('.squaredTwo input[type="checkbox"]').click(function(){
		if(jQuery(this).is(":checked")){
			jQuery(this).parent(".squaredTwo").css("background", "green");
		}
		else if(jQuery(this).is(":not(:checked)")){
			jQuery(this).parent(".squaredTwo").css("background", "#EE7A34");
		}
	});
	jQuery('.squaredFour #squaredsilver').click(function(){
		if(jQuery(this).is(":checked")){
			jQuery(this).parent(".squaredFour").css("background", "#219C64");
			jQuery(this).parent(".squaredFour").css("background", "-webkit-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "-o-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "-moz-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").children( "span" ).css("color", "#fff");
		}
		else if(jQuery(this).is(":not(:checked)")){
			jQuery(this).parent(".squaredFour").css("background", "#CBCBCB");
			jQuery(this).parent(".squaredFour").css("background", "-webkit-linear-gradient(#F6F6F6, #CBCBCB)");
			jQuery(this).parent(".squaredFour").css("background", "-o-linear-gradient(#F6F6F6, #CBCBCB)");
			jQuery(this).parent(".squaredFour").css("background", "-moz-linear-gradient(#F6F6F6, #CBCBCB)");
			jQuery(this).parent(".squaredFour").css("background", "linear-gradient(#F6F6F6, #CBCBCB)");
			jQuery(this).parent(".squaredFour").children( "span" ).css("color", "#282828");
		}
	});
	jQuery('.squaredFour #squaredgold').click(function(){
		if(jQuery(this).is(":checked")){
			jQuery(this).parent(".squaredFour").css("background", "#219C64");
			jQuery(this).parent(".squaredFour").css("background", "-webkit-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "-o-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "-moz-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").children( "span" ).css("color", "#fff");
		}
		else if(jQuery(this).is(":not(:checked)")){
			jQuery(this).parent(".squaredFour").css("background", "#DDA503");
			jQuery(this).parent(".squaredFour").css("background", "-webkit-linear-gradient(#FED254, #DDA503)");
			jQuery(this).parent(".squaredFour").css("background", "-o-linear-gradient(#FED254, #DDA503)");
			jQuery(this).parent(".squaredFour").css("background", "-moz-linear-gradient(#FED254, #DDA503)");
			jQuery(this).parent(".squaredFour").css("background", "linear-gradient(#FED254, #DDA503)");
			jQuery(this).parent(".squaredFour").children( "span" ).css("color", "#282828");
		}
	});
	jQuery('.squaredFour #squaredplatinum').click(function(){
		if(jQuery(this).is(":checked")){
			jQuery(this).parent(".squaredFour").css("background", "#219C64");
			jQuery(this).parent(".squaredFour").css("background", "-webkit-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "-o-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "-moz-linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").css("background", "linear-gradient(#1EDE86, #219C64)");
			jQuery(this).parent(".squaredFour").children( "span" ).css("color", "#fff");
		}
		else if(jQuery(this).is(":not(:checked)")){
			jQuery(this).parent(".squaredFour").css("background", "#AAAAAA");
			jQuery(this).parent(".squaredFour").css("background", "-webkit-linear-gradient(#ECECEC, #AAAAAA)");
			jQuery(this).parent(".squaredFour").css("background", "-o-linear-gradient(#ECECEC, #AAAAAA)");
			jQuery(this).parent(".squaredFour").css("background", "-moz-linear-gradient(#ECECEC, #AAAAAA)");
			jQuery(this).parent(".squaredFour").css("background", "linear-gradient(#ECECEC, #AAAAAA)");
			jQuery(this).parent(".squaredFour").children( "span" ).css("color", "#282828");
		}
	});
});
function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            jQuery('#upimg').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}