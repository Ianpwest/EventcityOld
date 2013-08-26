//**********************************************************************************************************************//
// Function used to validate the website field before inputting it into the database from Create Event page.
// 3/17/2012 (Randall)
// 03/18/2012 Functionality change - this func now sets global var and then calls MainValidation()
//**********************************************************************************************************************//

function validateWeb() {

    var input = document.getElementById("strWebpage").value;    // String value of the text box for the webpage

    var isWeb = 0;                                              // Boolean to see if we return a true or false for the web validate
    var webTest = /\b\w+[.]*/;                                   // Regex used to test against the validity of the webpage.

    if (webTest.test(input) || !input )
        isWeb = 1;
    else
        isWeb = 0;

    if (isWeb) {
        //document.getElementById("webX").style.display = "none";
        document.getElementById("webCheck").style.display = "block";
        document.getElementById("strWebpage").style.opacity = 1.0;
        setWeb(true);
    }
    else {
        //document.getElementById("webX").style.display = "block";
        document.getElementById("webCheck").style.display = "none";
        document.getElementById("strWebpage").style.opacity = 0.6;
        setWeb(false);
    }
    MainValidation();

}