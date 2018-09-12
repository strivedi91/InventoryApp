seqments = 40;
_mPreferMetric = true;
var address;
var map = new GMap2(document.getElementById("map"));


////pan and zoom to fit
var bounds = new GLatLngBounds();
function fit() {
    map.panTo(bounds.getCenter());
    map.setZoom(map.getBoundsZoomLevel(bounds));
}
$(document).ready(function () {
    loadMap();
});

///Geo

var geocoder = new GClientGeocoder();

function showAddress() {
    geocoder.getLatLng(address, function (point) {
        if (!point) {
            //document.getElementById('haku').style.color = 'red';
        }
        else {
            draw(point);
            map.panTo(point);
            //document.getElementById('haku').style.color = 'black';
        }
    })
    //return point;
}

// Esa's Auto ZoomOut

function esasZoomOut() {
    var paragraphs = map.getContainer().getElementsByTagName('p').length;
    //GLog.write(paragraphs);
    if (paragraphs > 6) {
        map.zoomOut();
    }
}
var interval = setInterval("esasZoomOut()", 500);

//calling circle drawing function
function draw(pnt) {
    map.clearOverlays();
    bounds = new GLatLngBounds();
    var givenRad = radius * 1;
    var givenQuality = seqments * 1;
    var centre = pnt || map.getCenter()
    drawCircle(centre, givenRad, givenQuality);
    fit();
}

////////////////////////// circle///////////////////////////////

function drawCircle(center, radius, nodes, liColor, liWidth, liOpa, fillColor, fillOpa) {
    // Esa 2006
    //calculating km/degree
    var latConv = center.distanceFrom(new GLatLng(center.lat() + 0.1, center.lng())) / 100;
    var lngConv = center.distanceFrom(new GLatLng(center.lat(), center.lng() + 0.1)) / 100;

    //Loop
    var points = [];
    var step = parseInt(360 / nodes) || 10;
    for (var i = 0; i <= 360; i += step) {
        var pint = new GLatLng(center.lat() + (radius / latConv * Math.cos(i * Math.PI / 180)), center.lng() +
        (radius / lngConv * Math.sin(i * Math.PI / 180)));
        points.push(pint);
        bounds.extend(pint); //this is for fit function
    }
    points.push(points[0]); // Closes the circle, thanks Martin
    fillColor = fillColor || liColor || "#0055ff";
    liWidth = liWidth || 2;
    var poly = new GPolygon(points, liColor, liWidth, liOpa, fillColor, fillOpa);
    map.addOverlay(poly);
}

function loadMap() {
    address = $("#txtZipCode").val();
    radius = $('#dropMiles').val();
    if (address == "" || radius == "")
    {
        $('#map').hide();
        return false;
    }
    radius = radius * 0.621371;// Convert miles to km
    $.ajax({
        url: "http://maps.googleapis.com/maps/api/geocode/json?components=postal_code:" + address + "&sensor=false",
        method: "POST",
        success: function (data) {
            if (data.results.length > 0) {
                $('#map').show();
                latitude = data.results[0].geometry.location.lat;
                longitude = data.results[0].geometry.location.lng;
                var start = new GLatLng(latitude, longitude);                
                map.setCenter(start, 10);
                new GKeyboardHandler(map);
                map.enableContinuousZoom();
                map.enableDoubleClickZoom();
                
                draw();

                if ($("#ServiceAreaRadiusMiles").val() == '5') {
                    map.setZoom(13);
                }

                if ($("#ServiceAreaRadiusMiles").val() == '10') {
                    map.setZoom(12);
                }
                
                if ($("#ServiceAreaRadiusMiles").val() == '25')
                    {                    
                    map.setZoom(10);
                }
                if ($("#ServiceAreaRadiusMiles").val() == '50') {
                    map.setZoom(9);
                }
                if ($("#ServiceAreaRadiusMiles").val() == '100') {
                    map.setZoom(8);
                }

                
               
                
            }
            else {
                $('#map').hide();
            }
        }
    });


   
    return false;
}

function getLatLong(zipcode) {

    $.ajax({
        url: "http://maps.googleapis.com/maps/api/geocode/json?address=santa+cruz&components=postal_code:" + zipcode + "&sensor=false",
        method: "POST",
        success: function (data) {
            latitude = data.results[0].geometry.location.lat;
            longitude = data.results[0].geometry.location.lng;
            lat = latitude;
            return data;
        }

    });
}