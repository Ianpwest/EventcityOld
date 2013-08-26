//**********************************************************************************************************************//
// Contains the functions used to dynamically change the Event Creation page for the user (title/img/etc.)
// Condensing into a file to make _Layout.cshtml look nicer.
// 3/18/2012 (Randall)
//**********************************************************************************************************************//

function readURL(input) {
    if (input.files && input.files[0]) {
        // Running setImage() to indicate that the image has been set into the browser for MainValidation()
        setImage(true);
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imgPreview').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
    MainValidation();   //Adding to run the validation check after setting the image.
}

function dynamicText(userUpdate, dynamicUpdate) {

    var userInput = document.getElementById(userUpdate).value;
    document.getElementById(dynamicUpdate).innerHTML = userInput;
}

function dynamicDate(userStart, dynamicUpdate) {

    var userStartDate = document.getElementById(userStart).value;
    // var userEndDate = document.getElementById(userEnd).value;
    // document.getElementById(dynamicUpdate).innerHTML = userStartDate + " - " + userEndDate;
    document.getElementById(dynamicUpdate).innerHTML = userStartDate;
}

function dynamicFont(userSelect, dynamicUpdate) {

    // var selectedFont = document.getElementById(userSelect).style.fontFamily;
    // var element = document.getElementById(dynamicUpdate);
    // element.style.fontFamily = selectedFont;
    var selectedElement = document.getElementById(userSelect);
    var textSelected = selectedElement.options[selectedElement.selectedIndex].text;
    document.getElementById(dynamicUpdate).style.fontFamily = textSelected;
}

function dynamicSize(userSelect, dynamicUpdate) {

    var selectedElement = document.getElementById(userSelect);
    var fontSelected = selectedElement.options[selectedElement.selectedIndex].text;
    //var fontSelected = document.getElementById(userSelect).getAttribute('value');
    document.getElementById(dynamicUpdate).style.fontSize = fontSelected + "px";
}

function dynamicColor(userSelect, dynamicUpdate) {
    
    var selectedColor = document.getElementById(userSelect).value;
    document.getElementById(dynamicUpdate).style.color = selectedColor;
}