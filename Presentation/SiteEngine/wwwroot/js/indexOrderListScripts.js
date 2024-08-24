document.addEventListener("DOMContentLoaded", startVoid());

function startVoid() {
    var checkboxesStatus = document.querySelectorAll('input[name="statusId"]');
    var checkboxesPriority = document.querySelectorAll('input[name="priorityId"]');

    checkboxesStatus.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            var selectedStatuses = [];
            var checkedCheckboxesStatuses = document.querySelectorAll('input[name="statusId"]:checked');

            checkedCheckboxesStatuses.forEach(function (checkedCheckboxStatus) {
                var statusId = checkedCheckboxStatus.id;
                if (!selectedStatuses.includes(statusId)) {
                    selectedStatuses.push(statusId);
                }
            });

            var allCheckboxesStatuses = document.querySelectorAll('input[name="statusId"]');
            allCheckboxesStatuses.forEach(function (checkbox) {
                var statusId = checkbox.id;
                if (!checkbox.checked) {
                    var index = selectedStatuses.lastIndexOf(statusId);
                    if (index > -1) {
                        selectedStatuses.splice(index, 1)
                    }
                }
            });

            document.getElementById('statusesId').value = selectedStatuses.join(',');
        });
    });

    checkboxesPriority.forEach(function (checkbox) {
        checkbox.addEventListener('change', function () {
            var selectedPriorities = [];
            var checkedCheckboxesPriorities = document.querySelectorAll('input[name="priorityId"]:checked');

            checkedCheckboxesPriorities.forEach(function (checkedCheckboxPriority) {
                var priorityId = checkedCheckboxPriority.id;
                if (!selectedPriorities.includes(priorityId)) {
                    selectedPriorities.push(priorityId);
                }
            });

            var allCheckboxesPriorities = document.querySelectorAll('input[name="priorityId"]');
            allCheckboxesPriorities.forEach(function (checkbox) {
                var priorityId = checkbox.id;
                if (!checkbox.checked) {
                    var index = selectedPriorities.lastIndexOf(priorityId);
                    if (index > -1) {
                        selectedPriorities.splice(index, 1)
                    }
                }
            });

            document.getElementById('prioritiesId').value = selectedPriorities.join(',');
            console.log(document.getElementById('prioritiesId').value)
        });
    });
};