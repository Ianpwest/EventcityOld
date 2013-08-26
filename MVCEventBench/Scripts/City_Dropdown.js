//**********************************************************************************************************************//
// Populates the city combobox on the Event Creation page after the state has been selected.
// 03/18/2012 (Randall)
//**********************************************************************************************************************//

function findCities() {

    document.getElementById("testp").innerHTML = "Got here";
//    var whichState = document.getElementById("strEventState").value;
//    var strCities = "";

//    $.get("/Event/WhichCities", { state: whichState }, function (data) {
//        //This is the success function we should be removing or adding the new events. 
//        //$("#divRenderBody").html(data);
//        strCities = data.message;
    //    }
//    cities = new XMLHttpRequest();
//    cities.open("GET", "http://localhost:16939/Event/WhichCities", true);

//    $.getJSON('/Event/WhichCities', { strState: document.getElementById("strEventState").value },
//    function (json) 
//    {
//        document.getElementById("testp").innerHTML = "In da JSAWN";
    //    });
    $.ajax({
        type: "Post",
        datatype: 'json',
        data: "strState=" + document.getElementById("strEventState").value,
        url: "/Event/WhichCities",
        success: function (data) {
            //document.getElementById("testp").innerHTML = data.message;
            document.getElementById("strEventCity").style.display = "block";
            var array = data.message.split(',');
        }
    });

    for (var i = 0; array.length; i++) {

        document.getElementById("strEventCity").add(array[i], null);
    }

    //document.getElementById("lbl").innerHTML = ":(";
}