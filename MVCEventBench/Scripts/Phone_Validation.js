//**********************************************************************************************************************//
// Function used to validate the phone number field before inputting it into the database from Create Event page.
// 03/1/2012 (Randall)
// 03/18/2012 Functionality change - this func now sets global var and then calls MainValidation()
//**********************************************************************************************************************//
function validatePhone() {

    var input = document.getElementById("strPhoneNumber").value;
    var len = input.length;         //Gets the length of argument to test the size of the string.
    var isNum = 0;                  //Boolean to test if the number is valid (is a phone number)    
    //Creating regular expressions to cover which format to test the number against based on the string length.
    var length7 = /\b\d{7}\b/;                                  //Format: #######
    var length8 = /\b\d{3}[-.\s]\d{4}\b/;                       // Format ###-#### or ###.#### or ### ####
    var length10 = /\b\d{10}\b/;                                // Format ########## 
    var length11 = /\b1\d{10}\b/;                               // Format 1########## 
    var length12 = /\b\d{3}[-.\s]\d{3}[-.\s]\d{4}\b/;           // Format ###-###-#### or ###.###.#### or ### ### ####
    var length14 = /\b1[.\-\s]\d{3}[-.\s]\d{3}[\-.\s]\d{4}\b/;  // Format 1-###-###-#### or 1.###.###.#### or 1 ### ### ####

    //Switch determines which string length then tests the possible formats, sets the isNum to true(1) or false(0).
    switch (len) {

        case 0:
        {
            isNum = 1;            
            break;
        }
        case 7:
        {
            if (length7.test(input)) {
                isNum = 1;
            }
            break;
        }
        case 8:
        {
            if (length8.test(input)) {
                isNum = 1;
            }
            break;
        }
        case 10:
        {
            if (length10.test(input)) {
                isNum = 1;
            }
            break;
        }
        case 11:
        {
            if (length11.test(input)) {
                isNum = 1;
            }
            break;
        }
        case 12:
        {
            if (length12.test(input)) {
                isNum = 1;
            }
            break;
        }
        case 14:
        {
            if (length14.test(input)) {
                isNum = 1;
            }
            break;
        }
        default:
        {
            isNum = 0;
            break;
        }
    }

    if (isNum) {
        //document.getElementById("phoneX").style.display = "none";
        document.getElementById("phoneCheck").style.display = "block";
        document.getElementById("strPhoneNumber").style.opacity = 1.0;
        setPhone(true); //Set's global phone validation value to true              
    }
    else {
        //document.getElementById("phoneX").style.display = "block";
        document.getElementById("phoneCheck").style.display = "none";
        document.getElementById("strPhoneNumber").style.opacity = 0.6;
        setPhone(false);   //Set's global phone validation value to false
    }
    MainValidation(); 
   
}