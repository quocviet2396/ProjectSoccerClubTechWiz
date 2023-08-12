jQuery(document).ready(function ($) {
	const urlPath = window.location.pathname;
	$(".site-navigation ul.site-menu li").each(function (index, element) {
		if (!urlPath.includes($(element).data("name"))) {
			if ($(element).data("name") === "Home") {
				$(element).addClass("active");
			}
		} else {
			$(".site-navigation ul.site-menu li").removeClass("active");
			$(element).addClass("active");
		}
	});
});