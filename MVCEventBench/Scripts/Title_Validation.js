//**********************************************************************************************************************//
// Function used to validate the title field before inputting it into the database from Create Event page.
// 3/17/2012 (Randall)
// 03/18/2012 Functionality change - this func now sets global var and then calls MainValidation()
//**********************************************************************************************************************//

function validateTitle() {

    var input = document.getElementById("strEventName").value;      // String value of the text box for the webpage

    if (input.length >= 4)     // Validation is: greater than 4 characters.
    {
        //document.getElementById("titleX").style.display = "none";
        document.getElementById("titleCheck").style.display = "block";
        document.getElementById("strEventName").style.opacity = 1.0;
        setTitle(true);
    }
    else {
        //document.getElementById("titleX").style.display = "block";
        document.getElementById("titleCheck").style.display = "none";
        document.getElementById("strEventName").style.opacity = 0.6;
        setTitle(false);
    }

    MainValidation();

}