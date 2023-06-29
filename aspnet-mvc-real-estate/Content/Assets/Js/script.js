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
    prevArrow: "<button type='button' class='slick-prev slick-arrow-sanpham'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow: "<button type='button' class='slick-next slick-arrow-sanpham'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [{
        breakpoint: 1024,
        settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
            infinite: true,
        }
    },
    {
        breakpoint: 600,
        settings: {
            slidesToShow: 2,
            slidesToScroll: 2
        }
    },
    {
        breakpoint: 480,
        settings: {
            slidesToShow: 1,
            slidesToScroll: 1
        }
    }
    ]
});
$('.slide-product-2').slick({
    slidesToShow: 4,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 5000,
    dots: true,
    prevArrow: "<button type='button' class='slick-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow: "<button type='button' class='slick-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [{
        breakpoint: 1024,
        settings: {
            slidesToShow: 3,
            slidesToScroll: 3,
            infinite: true,
        }
    },
    {
        breakpoint: 600,
        settings: {
            slidesToShow: 2,
            slidesToScroll: 2
        }
    },
    {
        breakpoint: 480,
        settings: {
            slidesToShow: 1,
            slidesToScroll: 1
        }
    }
    ]
});
$('.slide-brand').slick({
    slidesToShow: 6,
    slidesToScroll: 1,
    arrow: false,
    responsive: [{
        breakpoint: 1024,
        settings: {
            slidesToShow: 5,
            slidesToScroll: 3,
            infinite: true,
        }
    },
    {
        breakpoint: 600,
        settings: {
            slidesToShow: 4,
            slidesToScroll: 2
        }
    },
    {
        breakpoint: 480,
        settings: {
            slidesToShow: 3,
            slidesToScroll: 1
        }
    }
    ]
});
$('.collection-galery').slick({
    centerMode: true,
    centerPadding: '60px',
    slidesToShow: 3,
    responsive: [
        {
            breakpoint: 768,
            settings: {
                arrows: false,
                centerMode: true,
                centerPadding: '40px',
                slidesToShow: 3
            }
        },
        {
            breakpoint: 480,
            settings: {
                arrows: false,
                centerMode: true,
                centerPadding: '40px',
                slidesToShow: 1
            }
        }
    ]
});

$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();
});