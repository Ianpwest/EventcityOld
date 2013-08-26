var geocoder;
var map;
function DrawMap(lat, long) {

    geocoder = new google.maps.Geocoder();
    var myOptions = {
        center: new google.maps.LatLng(lat, long),
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };    
    map = new google.maps.Map(document.getElementById("mapCanvas"), myOptions);


//    var marker = new google.maps.Marker({
//        position: new google.maps.LatLng(lat, long),
//        map: map,
//        title:"Dis the spot"
//    });    

    document.getElementById("lbl").innerHTML = lat + ", " + long;
}

function Geocode(address) {

    geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            map.setCenter(results[0].geometry.location);
            var marker = new google.maps.Marker({
                map: map,
                position: results[0].geometry.location,
                title:"Right hurr"
            });
        } else {
            alert("Geocode was not successful for the following reason: " + status);
        }
    });
}