document.addEventListener("DOMContentLoaded", startVoid());

function startVoid() {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');

    checkboxes.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            var selectedServices = [];
            var checkedCheckboxes = document.querySelectorAll('input[type="checkbox"]:checked');

            checkedCheckboxes.forEach(function (checkedCheckbox) {
                var serviceId = checkedCheckbox.id;
                if (!selectedServices.includes(serviceId)) {
                    selectedServices.push(serviceId);
                }
            });

            var allCheckboxes = document.querySelectorAll('input[type="checkbox"]');
            allCheckboxes.forEach(function (checkbox) {
                var serviceId = checkbox.id;
                if (!checkbox.checked) {
                    var index = selectedServices.indexOf(serviceId);
                    if (index > -1) {
                        selectedServices.splice(index, 1);
                    }
                }
            });

            document.getElementById('selectedServices').value = selectedServices.join(',');
        });
    });

    var input = document.querySelector('.searchServices')
    var services = document.querySelectorAll('.serviceItem')
    var servicesState = []

    services.forEach(function(service) {
        servicesState.push(service.classList.add('show'));
    })

    input.addEventListener('input', function () {
        searchServices();
    });

    input.addEventListener('keyup', function () {
        searchServices();
    });

    function searchServices() {
        var searchText = input.value.trim().toLowerCase();

        services.forEach(function (service, index) {
            var serviceName = service.innerText.toLowerCase();
            if (serviceName.includes(searchText)) {
                service.classList.remove('hidden')
                service.classList.add('show');
            } else {
                service.classList.remove('show')
                service.classList.add('hidden');
            }
        });

        // Проверяем, отображается ли какая-либо услуга, и восстанавливаем список при необходимости
        var anyServiceDisplayed = Array.from(services).some(service => {
            service.classList.contains('hidden')
        })
        if (!anyServiceDisplayed && searchText === '') {
            services.forEach(function (service, index) {
                service.classList.remove('hidden')
                service.classList.add('show')
            });
        }
    }
}
