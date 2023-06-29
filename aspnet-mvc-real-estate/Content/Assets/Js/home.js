//Show/hide navigation
var bars = document.getElementById("bars-wrapper");
var close = document.getElementById("btn-close");
var dark_bg = document.getElementById("dark-background");
var navbar = document.getElementById("nav-bar-menu");
bars.addEventListener("click", function () {
    navbar.style.left = "0";
    close.style.display = "block";
    dark_bg.style.display = "block"
});
dark_bg.addEventListener("click", function () {
    navbar.style.left = "-100%";
    close.style.display = "none";
    dark_bg.style.display = "none"
});
close.addEventListener("click", function () {
    navbar.style.left = "-100%";
    close.style.display = "none";
    dark_bg.style.display = "none"
});
$('.slide-product').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 5000,
    dots: true,
});