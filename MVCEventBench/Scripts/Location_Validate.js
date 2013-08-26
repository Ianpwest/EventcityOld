//**********************************************************************************************************************//
// Function used to validate the location field before inputting it into the database from Create Event page.
// 3/17/2012 (Randall)
// 03/18/2012 Functionality change - this func now sets global var and then calls MainValidation()
//**********************************************************************************************************************//

function validateLocation() {

    var input = document.getElementById("strEventLocation").value;      // String value of the text box for the webpage

    if (input.length >= 4)     // Validation is: greater than 4 characters.
    {
        //document.getElementById("locX").style.display = "none";
        document.getElementById("locCheck").style.display = "block";
        document.getElementById("strEventLocation").style.opacity = 1.0;
        setLoc(true);
    }
    else {
        //document.getElementById("locX").style.display = "block";
        document.getElementById("locCheck").style.display = "none";
        document.getElementById("strEventLocation").style.opacity = 0.6;
        setLoc(false);
    }
    MainValidation();
}