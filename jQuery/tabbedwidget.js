$(document).ready(function(){
	var tabContainers = $('div.tabs > div.sub-div > div');
    tabContainers.hide().filter(':first').show();
    $('div.tabs ul.tabNavigation a').click(function () {
    tabContainers.hide();
    tabContainers.filter(this.hash).show(); 
    return false;})
    $(".tab").click(function(){
		$(".tab").removeClass("active-tab");
		$(this).addClass("active-tab");
	});
	$(".tab:first").click();

});
