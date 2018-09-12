$(document).ready(function() {
  "use strict";
  //Masonry Script
  $(window).resize(function() {
    var $grid = $('.campaign-masonry').isotope ({
      itemSelector: '.masonry-item',
      layoutMode: 'packery',
      percentPosition: true,
      isFitWidth: true,
    });
    var filterFns = {
      ium: function() {
        var name = $(this).find('.name').text();
        return name.match(/ium$/);
      }
    };
    $('.masonry-filters').on('click', 'li a', function() {
      var filterValue = $(this).attr('data-filter');
      filterValue = filterFns[ filterValue ] || filterValue;
      $grid.isotope({
        filter: filterValue
      });
    });
    $('.masonry-filters').each(function(i, buttonGroup) {
      var $buttonGroup = $(buttonGroup);
      $buttonGroup.on('click', 'li a', function() {
        $buttonGroup.find('.active').removeClass('active');
        $(this).addClass('active');
      });
    });
  });

  //Custom Select Script
  $('select').niceSelect();

  //Add And Remove Class Script
  $('.add-card > .btn-block > .btn').click(function() {
    $('.add-card-form form').slideToggle('');
  });

  //Remove Modal Popup Scrolling Issue for IE Browser
  $('.campaign-image a').click(function() {
	  $('html').addClass('modal-open');
	});
});