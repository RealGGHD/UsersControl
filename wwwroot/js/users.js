// wwwroot/js/users.js
document.addEventListener("DOMContentLoaded", () => {
    const selectAllCheckbox = document.getElementById("selectAll");
    if (!selectAllCheckbox) return;

    selectAllCheckbox.addEventListener("change", function () {
        const checked = this.checked;
        const checkboxes = document.querySelectorAll("input[name='selectedIds']");
        checkboxes.forEach(cb => cb.checked = checked);
    });
});
