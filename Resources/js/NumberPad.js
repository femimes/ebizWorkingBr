var nonChar = "";
window.onload = init;
function init() {
  var char = "0123456789";
  keyValue = "";
  for (i = 0; keyValue.length < 10; i++) {
    rand = Math.round(Math.random() * 10);
    if (keyValue.indexOf(rand) > -1) continue;
    keyValue += char.substring(rand, rand + 1);
  }
  console.log(keyValue);
  counter = -1;
  while (counter < 10) {
    counter++;
    document.querySelector("#num" + counter).value = keyValue[counter];
  }
}
function showClickedButton(btn) {
  clickedButton((nonChar += btn));
}

function clearPassword() {
  clickedButton((nonChar = ""));
}
function deleteChar() {
  clickedButton((nonChar = nonChar.substring(0, nonChar.length - 1)));
}

function clickedButton(btn) {
  psword.value = btn;
}
function displayPassword() {
  box = document.querySelector("#plainPsWord");
  if (box.checked == false) {
    psword.type = "password";
  } else {
    psword.type = "text";
  }
 }


function changePasswordWithEye() {
    const passwordInputField = document.querySelector("#psword");
    const theEyeWithoutCross = document.querySelector("#myEye");
    const theEyeWithCross = document.querySelector("#myEyeOff");
    const displayingMyEyeWithoutCross = theEyeWithoutCross.style.display;
    if (displayingMyEyeWithoutCross === "none") {
        theEyeWithCross.style.display = "none";
        theEyeWithoutCross.style.display = "";
        passwordInputField.type = "text";
    } else {
        theEyeWithCross.style.display = "";
        theEyeWithoutCross.style.display = "none";
        passwordInputField.type = "password";
    }
}