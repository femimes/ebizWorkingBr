function Timer(fn, t) {
    var timerObj = setInterval(fn, t);

    this.stop = function () {
        if (timerObj) {
            clearInterval(timerObj);
            timerObj = null;
        }
        return this;
    };
    this.start = function () {
        if (!timerObj) {
            this.stop();
            timerObj = setInterval(fn, t);
        }
        return this;
    };
    this.reset = function (newT = t) {
        t = newT;
        return this.stop().start();
    };
}

const timer = new Timer(function () {
    window.location.replace(window.location.href = 'Expired.aspx');
}, 600000);    // Ten minutes

if ("serviceWorker" in navigator) {
    navigator.serviceWorker.addEventListener("message", (event) => {
        switch (event.data.type) {
            case "TIMER_RESTART":
                console.log("Timer has been reset");
                timer.reset();
                break;
            default:
                break;

        }
    });
}
const registerServiceWorker = function () {
    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("/sw.js").then(
            (registration) => {
                console.log("Service Worker registration successful");
            },
            (err) => {
                console.log("Registration failed");
            }
        );
    }
};

const unregisterServiceWorker = () => {
    if ("serviceWorker" in navigator) {
        navigator.serviceWorker
            .getRegistrations()
            .then(function (registrations) {
                for (let registration of registrations) {
                    registration.unregister();
                }
            })
            .catch(function (err) {
                console.log("Service Worker removal failed: ", err);
            });
    }
};


registerServiceWorker();
