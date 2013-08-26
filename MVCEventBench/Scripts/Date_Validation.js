//**********************************************************************************************************************//
// Function used to validate the date field before inputting it into the database from Create Event page.
// 3/14/2012 (Randall)
// 03/18/2012 Functionality change - this func now sets global var and then calls MainValidation()
//**********************************************************************************************************************//

function validateDate() {

    // Grabs the string from the textbox input and parses it to a Date object.
    var input = document.getElementById("dEventDateStart").value;
    var testDate = new Date(Date.parse(input));
    var today = new Date();

    //Boolean logic to determine if the date has been validated.
    var isDate = 0;

    // Regular expressions to filter out dates that do parse but are not actually valid dates.
    var test1 = /\b[2 4 6 9].31/; //Covers the 31st of all months except November
    var test2 = /\b11.31/; //Covers the 31st of November
    var test3 = /\b2.30/; //Covers the 30th of Februrary
    var test4 = /\b2.29/; //Covers the 29th of Feburary - tho this will be bypassed of it's a leap year. (year % 4 =! 0)

    // String value of a parsed date that did not parse successfully is "Invalid Date"
    if (testDate != "Invalid Date")
        isDate = 1;
    else
        isDate = 0;

    // If the regular expressions match then the date is invalid. With the exception of test4 - it requires non leap years.
    if (test1.test(input) || test2.test(input) || test3.test(input) || (test4.test(input) && (testDate.getFullYear() % 4 != 0)))
        isDate = 0;

    // Checks to make sure the date they are putting into the text box isnt earlier than the current date.
    if (testDate > today)
        isDate = isDate;
    else if (testDate.getFullYear() < today.getFullYear())
        isDate = 0;
    else if (testDate.getMonth() < today.getMonth())
        isDate = 0;
    else if (testDate.getDate() < today.getDate())
        isDate = 0;
//    if (testDate < today)
//        isDate = 0;

    if (isDate) {
        //document.getElementById("dateX").style.display = "none";
        document.getElementById("dateCheck").style.display = "block";
        document.getElementById("dEventDateStart").style.opacity = 1.0;
        setDate(true);
    }
    else {
        //document.getElementById("dateX").style.display = "block";
        document.getElementById("dateCheck").style.display = "none";
        document.getElementById("dEventDateStart").style.opacity = 0.6;
        setDate(true);
    }
    MainValidation();

}