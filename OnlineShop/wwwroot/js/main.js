(function ($) {
    "use strict";

    new WOW().init();  

    /*---background image---*/
	function dataBackgroundImage() {
		$('[data-bgimg]').each(function () {
			var bgImgUrl = $(this).data('bgimg');
			$(this).css({
				'background-image': 'url(' + bgImgUrl + ')', // + meaning concat
			});
		});
    }
    
    $(window).on('load', function () {
        dataBackgroundImage();
    });
    
    /*---stickey menu---*/
    $(window).on('scroll',function() {    
           var scroll = $(window).scrollTop();
           if (scroll < 100) {
            $(".sticky-header").removeClass("sticky");
           }else{
            $(".sticky-header").addClass("sticky");
           }
    });
    
    /*---jQuery MeanMenu---*/

    $('.mobile-menu nav').meanmenu({
        meanScreenWidth: "9901",
        meanMenuContainer: ".mobile-menu",
        onePage: true,
    });

    /*---slider activation---*/
    $('.slider_area').owlCarousel({
        animateOut: 'fadeOut',
        autoplay: false,
		loop: true,
        nav: false,
        autoplayTimeout: 5000,
        items: 1,
        dots:true,
    });
    
    
    /*---category column4 activation---*/
       $('.category_column4').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: false,
        autoplayTimeout: 5000,
        items: 4,
        dots:false,
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            576:{
				items:2,
			},
            768:{
				items:3,
			},
            992:{
				items:4,
			},


		  }
    });
    

    /*---small product activation---*/
    $('.small_product_wrapper').slick({
        centerMode: true,
        centerPadding: '0',
        slidesToShow: 1,
        vertical:true,
        arrows:true,
        rows: 3,
        prevArrow:'<button class="prev_arrow"><i class="fa fa-angle-left"></i></button>',
        nextArrow:'<button class="next_arrow"><i class="fa fa-angle-right"></i></button>', 
        responsive:[
            {
              breakpoint: 480,
              settings: {
                slidesToShow: 1,
                  slidesToScroll: 1,
              }
            },
            {
              breakpoint: 768,
              settings: {
                slidesToShow: 1,
                  slidesToScroll: 1,
              }
            },
            {
              breakpoint: 991,
              settings: {
                slidesToShow: 2,
                  vertical:false,
              }
            },
        ]
    });
    
    
    /*---category column4 activation---*/
       $('.product_column4').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 4,
        margin: 24,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],   
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            576:{
				items:2,
			},
            768:{
				items:3,
			},
            992:{
				items:4,
			},


		  }
    });
    
    
    /*---category column4 activation---*/
       $('.product_column3').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 3,
        margin: 24,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],   
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            576:{
				items:2,
			},
            768:{
				items:3,
			},



		  }
    });
    
    
    /*---category column4 activation---*/
       $('.deals_product_column2').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 2,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],   
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            576:{
				items:1,
			},
            768:{
				items:1,
			},
            992:{
				items:2,
			},


		  }
    });
    
     /*---brand container activation---*/
     $('.brand_container').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: false,
        autoplayTimeout: 5000,
        items: 6,
        dots:false,
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            480:{
				items:2,
			},
            768:{
				items:4,
			},
            992:{
				items:5,
			},
            1200:{
				items:6,
			},

		  }
    });
    
    
    /*---brand column4 activation---*/
     $('.brand_column4').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 4,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],   
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            480:{
				items:2,
			},
            768:{
				items:4,
			},

		  }
    });
    
    
    /*---single product activation---*/
    $('.single-product-active').on('changed.owl.carousel initialized.owl.carousel', function (event) {
        $(event.target).find('.owl-item').removeClass('last').eq(event.item.index + event.page.size - 1).addClass('last')}).owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 4,
        margin: 15,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            320:{
				items:3,
			},
            500:{
				items:4,
			},
            768:{
				items:3,
			},
            992:{
				items:4,
			},
            1200:{
				items:4,
			},


		  }
    });
    
    
    
    /*---testimonial active activation---*/
    $('.testimonial_active').owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 1,
        dots:true,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
    })
 
     /*---sidebar blog carousel activation---*/
    $('.sidebar_blog_carousel').owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 1,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            768:{
				items:2,
                margin: 30,
			},
            992:{
				items:1,
			},

		  }
    })
    
     /*---blog carousel seven activation---*/
    $('.blog_carousel_seven').owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 3,
        margin: 24,
        dots:true,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            768:{
				items:2,
			},
            992:{
				items:3,
			},

		  }
    })
    
      /*---deals product column1 activation---*/
    $('.deals_product_column1').owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 1,
        dots:true,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            768:{
				items:2,
                margin: 30,
			},
            992:{
				items:1,
			},

		  }
    })
    
        /*---small product activation---*/
    $('.recent_product_container').slick({
        centerMode: true,
        centerPadding: '0',
        slidesToShow: 1,
        arrows:true,
        rows: 4,
        prevArrow:'<button class="prev_arrow"><i class="fa fa-angle-left"></i></button>',
        nextArrow:'<button class="next_arrow"><i class="fa fa-angle-right"></i></button>', 
        responsive:[
            {
              breakpoint: 480,
              settings: {
                slidesToShow: 1,
                  slidesToScroll: 1,
              }
            },
            {
              breakpoint: 768,
              settings: {
                slidesToShow: 1,
                  slidesToScroll: 1,
              }
            },
            {
              breakpoint: 991,
              settings: {
                slidesToShow: 2,
                  slidesToScroll: 2,
              }
            },
        ]
    });
    
    
    /*  blog thumb activation */
    $('.blog_thumb_active').owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 1,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        dots:true,
    });
    
    /*---product navactive activation---*/
    $('.product_navactive').owlCarousel({
        autoplay: false,
		loop: true,
        nav: true,
        autoplayTimeout: 5000,
        items: 4,
        dots:false,
        navText: ['<i class="fa fa-angle-left"></i>','<i class="fa fa-angle-right"></i>'],
        responsiveClass:true,
		responsive:{
				0:{
				items:1,
			},
            250:{
				items:2,
			},
            480:{
				items:3,
			},
            768:{
				items:4,
			},
		  
        }
    });

    $('.modal').on('shown.bs.modal', function (e) {
        $('.product_navactive').resize();
    })

    $('.product_navactive a').on('click',function(e){
      e.preventDefault();

      var $href = $(this).attr('href');

      $('.product_navactive a').removeClass('active');
      $(this).addClass('active');

      $('.product-details-large .tab-pane').removeClass('active show');
      $('.product-details-large '+ $href ).addClass('active show');

    })
       
    /*--- Magnific Popup Video---*/
    $('.video_popup').magnificPopup({
        type: 'iframe',
        removalDelay: 300,
        mainClass: 'mfp-fade'
    });
    
    /*--- Magnific Popup Video---*/
    $('.port_popup').magnificPopup({
        type: 'image',
        gallery: {
            enabled: true
        }
    });
 
    
    /*--- niceSelect---*/
     $('.select_option').niceSelect();
    
    /*---  Accordion---*/
    $(".faequently-accordion").collapse({
        accordion:true,
        open: function() {
        this.slideDown(300);
      },
      close: function() {
        this.slideUp(300);
      }		
    });	  

    /*--- counterup activation ---*/
    $('.counter_number').counterUp({
        delay: 10,
        time: 1000
    });

    /*---  ScrollUp Active ---*/
    $.scrollUp({
        scrollText: '<i class="fa fa-angle-double-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'fade'
    });   
    
    /*---countdown activation---*/
		
	 $('[data-countdown]').each(function() {
		var $this = $(this), finalDate = $(this).data('countdown');
		$this.countdown(finalDate, function(event) {
		$this.html(event.strftime('<div class="countdown_area"><div class="single_countdown"><div class="countdown_number">%D</div><div class="countdown_title">days</div></div><div class="single_countdown"><div class="countdown_number">%H</div><div class="countdown_title">hours</div></div><div class="single_countdown"><div class="countdown_number">%M</div><div class="countdown_title">mins</div></div><div class="single_countdown"><div class="countdown_number">%S</div><div class="countdown_title">secs</div></div></div>'));     
               
       });
	});	
    
    /*---slider-range here---*/
    $( "#slider-range" ).slider({
        range: true,
        min: 0,
        max: 500,
        values: [ 0, 500 ],
        slide: function( event, ui ) {
        $( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
       }
    });
    $( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) +
       " - $" + $( "#slider-range" ).slider( "values", 1 ) );
    
    /*---niceSelect---*/
     $('.niceselect_option').niceSelect();
    
    /*---elevateZoom---*/
    $("#zoom1").elevateZoom({
        gallery:'gallery_01', 
        responsive : true,
        cursor: 'crosshair',
        zoomType : 'inner'
    
    });  
    
    /*---portfolio Isotope activation---*/
      $('.portfolio_gallery').imagesLoaded( function() {

        var $grid = $('.portfolio_gallery').isotope({
           itemSelector: '.gird_item',
            percentPosition: true,
            masonry: {
                columnWidth: '.gird_item'
            }
        });

          /*---ilter items on button click---*/
        $('.portfolio_button').on( 'click', 'button', function() {
           var filterValue = $(this).attr('data-filter');
           $grid.isotope({ filter: filterValue });
            
           $(this).siblings('.active').removeClass('active');
           $(this).addClass('active');
        });
       
    });
    
    /*---slide toggle activation---*/
   $('.mini_cart_wrapper > a').on('click', function(event){
        if($(window).width() < 991){
            $('.mini_cart').slideToggle('medium');
        }
    });
    
    /*categories slideToggle*/
    $(".categories_title").on("click", function() {
        $(this).toggleClass('active');
        $('.categories_menu_toggle').slideToggle('medium');
    }); 

    /*----------  Category more toggle  ----------*/

	$(".categories_menu_toggle li.hidden").hide();
	   $("#more-btn").on('click', function (e) {

		e.preventDefault();
		$(".categories_menu_toggle li.hidden").toggle(500);
		var htmlAfter = '<i class="fa fa-minus" aria-hidden="true"></i> Less Categories';
		var htmlBefore = '<i class="fa fa-plus" aria-hidden="true"></i> More Categories';


		if ($(this).html() == htmlBefore) {
			$(this).html(htmlAfter);
		} else {
			$(this).html(htmlBefore);
		}
	});
    
    
    /* ---------------------
	 Category menu
	--------------------- */
    
   
    function categorySubMenuToggle(){
        $('.categories_menu_toggle li.menu_item_children > a').on('click', function(){
        if($(window).width() < 991){
            $(this).removeAttr('href');
            var element = $(this).parent('li');
            if (element.hasClass('open')) {
                element.removeClass('open');
                element.find('li').removeClass('open');
                element.find('ul').slideUp();
            }
            else {
                element.addClass('open');
                element.children('ul').slideDown();
                element.siblings('li').children('ul').slideUp();
                element.siblings('li').removeClass('open');
                element.siblings('li').find('li').removeClass('open');
                element.siblings('li').find('ul').slideUp();
            }
        }
        });
        $('.categories_menu_toggle li.menu_item_children > a').append('<span class="expand"></span>');
    }
    categorySubMenuToggle();


    /*---shop grid activation---*/
    $('.shop_toolbar_btn > button').on('click', function (e) {
        
		e.preventDefault();
        
        $('.shop_toolbar_btn > button').removeClass('active');
		$(this).addClass('active');
        
		var parentsDiv = $('.shop_wrapper');
		var viewMode = $(this).data('role');
        
        
		parentsDiv.removeClass('grid_3 grid_4 grid_5 grid_list').addClass(viewMode);

		if(viewMode == 'grid_3'){
			parentsDiv.children().addClass('col-lg-4 col-md-4 col-sm-6').removeClass('col-lg-3 col-cust-5 col-12');
            
		}

		if(viewMode == 'grid_4'){
			parentsDiv.children().addClass('col-lg-3 col-md-4 col-sm-6').removeClass('col-lg-4 col-cust-5 col-12');
		}
        
        if(viewMode == 'grid_list'){
			parentsDiv.children().addClass('col-12').removeClass('col-lg-3 col-lg-4 col-md-4 col-sm-6 col-cust-5');
		}
            
	});
  
    
   /*---Newsletter Popup activation---*/
   
       setTimeout(function() {
            if($.cookie('shownewsletter')==1) $('.newletter-popup').hide();
            $('#subscribe_pemail').keypress(function(e) {
                if(e.which == 13) {
                    e.preventDefault();
                    email_subscribepopup();
                }
                var name= $(this).val();
                  $('#subscribe_pname').val(name);
            });
            $('#subscribe_pemail').change(function() {
             var name= $(this).val();
                      $('#subscribe_pname').val(name);
            });
            //transition effect
            if($.cookie("shownewsletter") != 1){
                $('.newletter-popup').bPopup();
            }
            $('#newsletter_popup_dont_show_again').on('change', function(){
                if($.cookie("shownewsletter") != 1){   
                    $.cookie("shownewsletter",'1')
                }else{
                    $.cookie("shownewsletter",'0')
                }
            }); 
        }, 2500);
    
    /*---canvas menu activation---*/

    
    $('.canvas_open').on('click', function(){
        $('.Offcanvas_menu_wrapper,.off_canvars_overlay').addClass('active')
    });
    
    $('.canvas_close,.off_canvars_overlay').on('click', function(){
        $('.Offcanvas_menu_wrapper,.off_canvars_overlay').removeClass('active')
    });
    
    
    
     /*---Off Canvas Menu---*/

    /*Variables*/
    var $offcanvasNav = $('.offcanvas_main_menu'),
        $offcanvasNavSubMenu = $offcanvasNav.find('.sub-menu');
    
    /*Add Toggle Button With Off Canvas Sub Menu*/
    $offcanvasNavSubMenu.parent().prepend('<span class="menu-expand"><i class="fa fa-angle-down"></i></span>');
    
    /*Close Off Canvas Sub Menu*/
    $offcanvasNavSubMenu.slideUp();
    
    /*Category Sub Menu Toggle*/
    $offcanvasNav.on('click', 'li a, li .menu-expand', function(e) {
        var $this = $(this);
        if ( ($this.parent().attr('class').match(/\b(menu-item-has-children|has-children|has-sub-menu)\b/)) && ($this.attr('href') === '#' || $this.hasClass('menu-expand')) ) {
            e.preventDefault();
            if ($this.siblings('ul:visible').length){
                $this.siblings('ul').slideUp('slow');
            }else {
                $this.closest('li').siblings('li').find('ul:visible').slideUp('slow');
                $this.siblings('ul').slideDown('slow');
            }
        }
        if( $this.is('a') || $this.is('span') || $this.attr('clas').match(/\b(menu-expand)\b/) ){
        	$this.parent().toggleClass('menu-open');
        }else if( $this.is('li') && $this.attr('class').match(/\b('menu-item-has-children')\b/) ){
        	$this.toggleClass('menu-open');
        }
    });
    
    
})(jQuery);	
