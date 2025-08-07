document.addEventListener("DOMContentLoaded", function () {
    const submenuIds = ["#operacionesSubmenu", "#sociosSubmenu"];

    document.querySelectorAll('.has-dropdown').forEach(link => {
        link.addEventListener("click", function (e) {
            const targetId = this.getAttribute("data-bs-target");
            const targetElement = document.querySelector(targetId);

            submenuIds.forEach(id => {
                if (id !== targetId) {
                    const submenu = document.querySelector(id);
                    if (submenu && submenu.classList.contains("show")) {
                        const bsCollapse = bootstrap.Collapse.getInstance(submenu) || new bootstrap.Collapse(submenu, { toggle: false });
                        bsCollapse.hide();
                    }
                }
            });
        });
    });
});
