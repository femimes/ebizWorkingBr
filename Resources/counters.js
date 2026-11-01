            const counters = document.querySelectorAll('.counter');
            const speed = 200;

            counters.forEach(counter => {
                const updateCount = () => {
                    const target = +counter.getAttribute('data-target');
                    const number = target;
                    const count = +counter.innerText;

                    const inc = target / speed

                    if (count < target) {
                        counter.innerText = count + inc;
                         //counter.innerText =  Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(number) + inc;
                        setTimeout(updateCount, 1)
                    } else {
                    }
                    //new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(number))
                    //counter.innerText = (new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(counter.innerText));
                   console.log(typeof target);
                    //console.log(typeof Number(parseFloat(target).toFixed(2)).toLocaleString('en', { minimumFractionDigits: 2 }));
                    //console.log(typeof new Intl.NumberFormat('de-DE', { style: 'currency', currency: 'EUR' }).format(target));
                }
                updateCount();

            }
                 

            )
            //Number(parseFloat(target).toFixed(2)).toLocaleString('en', {minimumFractionDigits: 2}); //


            // $(document).ready(function() {
            //     $(".counter").each(function() {
            //         $(this).format({format:"#,###", locale:"us"});
            //     });
            // });