const mobileDeviceHeight = 768; // Change the mobile minimum size here.
const sideMenu = document.querySelector(".vertical-menu");
const width = window.innerWidth || document.documentElement.clientWidth ||
    document.body.clientWidth;
const height = window.innerHeight || document.documentElement.clientHeight ||
    document.body.clientHeight;
const hideMenu = () => {
    if ((window.innerWidth || document.documentElement.clientWidth ||
        document.body.clientWidth) <= mobileDeviceHeight) {
        sideMenu.style.display = "none";
    }
}

function showOrHideMenuMobile() {
    if((window.innerWidth || document.documentElement.clientWidth ||
        document.body.clientWidth) <= mobileDeviceHeight) {

        const currentDisplay = getComputedStyle(sideMenu, null).getPropertyValue("display");
        if (currentDisplay === "none") {
            sideMenu.style.display = "block";
        } else {
            sideMenu.style.display = "none";
        }

    }

}
function makeMenuVisible() {
    if ((window.innerWidth || document.documentElement.clientWidth ||
        document.body.clientWidth) >= mobileDeviceHeight) {
        sideMenu.style.display = "block";
    }
   
}

//window.onresize = makeMenuVisible;