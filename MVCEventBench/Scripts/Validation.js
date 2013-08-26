//**********************************************************************************************************************//
// Main Validation for Event Creation page.
// 03/14/2012 (Randall)
// 03/18/2012 (Randall) Simplying the MainValidation to call the globals to see if they are validated (per the individual validation functions)
//
//**********************************************************************************************************************//
function MainValidation() {


    var isState = document.getElementById("strEventState").value;
    
    if (getPhone() && getDate() && getWeb() && getLoc() && getTitle() && getImage() && isState)
        document.getElementById("submitEvent").style.display = "block";
    else
        document.getElementById("submitEvent").style.display = "none";


}
