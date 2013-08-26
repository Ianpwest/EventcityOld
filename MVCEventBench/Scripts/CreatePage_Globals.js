//**********************************************************************************************************************//
// Global variables for the Event Creation page to keep track of validation on the page.
// Initializes all validation to 0 (false) and sets the values to true if the set functions are called inside of the individual validation functions.
// Inside of the MainValidation(), the get funcs are called to return the values.    
// 3/17/2012 (Randall)
//**********************************************************************************************************************//

var isPhone = 0;
function setPhone(isTrue) {

    isPhone = isTrue;
}
function getPhone() {

    return isPhone;
}
/*************************/
var isDate = 0;
function setDate(isTrue) {

    isDate = isTrue;
}
function getDate() {

    return isDate;
}
/*************************/
var isWeb = 0;
function setWeb(isTrue) {

    isWeb = isTrue;
}

function getWeb() {

    return isWeb;
}
/*************************/
var isLoc = 0;
function setLoc(isTrue) {

    isLoc = isTrue;
}

function getLoc() {

    return isLoc;
}
/*************************/
var isTitle = 0;
function setTitle(isTrue) {

    isTitle = isTrue;
}

function getTitle() {

    return isTitle;
}
/*************************/
var isImage = 0;
function setImage(isTrue) {

    isImage = isTrue;
}

function getImage() {

    return isImage;
}
/*************************/
var flgEventState = 0;
function setEventState(isTrue) {

    flgEventState = isTrue;
}

function getEventState() {

    return flgEventState;
}

    