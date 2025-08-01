(function () {
    const setTheme = (theme) => {
        document.documentElement.setAttribute("data-bs-theme", theme);
        localStorage.setItem("theme", theme);
        updateToggleText(theme);
        applyHighlightRows(theme);
        syncTable(theme);
    };

    const updateToggleText = (theme) => {
        const btn = document.getElementById("theme-toggle");
        if (!btn) return;
        btn.innerText = theme === "dark" ? "☀️ Light Mode" : "🌙 Dark Mode";
    };

    const applyHighlightRows = (theme) => {
        const isDark = theme === "dark";
        document.querySelectorAll(".highlight-row").forEach(row => {
            if (isDark) {
                row.classList.add("highlight-user-dark");
                row.classList.remove("table-info");
            } else {
                row.classList.remove("highlight-user-dark");
                row.classList.add("table-info");
            }
        });
    };

    const syncTable = (theme) => {
        const table = document.getElementById("users-table");
        if (!table) return;
        if (theme === "dark") {
            table.classList.add("table-dark");
        } else {
            table.classList.remove("table-dark");
        }
    };

    document.addEventListener("DOMContentLoaded", () => {
        const saved = localStorage.getItem("theme");
        const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
        const theme = saved || (prefersDark ? "dark" : "light");
        setTheme(theme);

        const btn = document.getElementById("theme-toggle");
        if (btn) {
            btn.addEventListener("click", () => {
                const current = document.documentElement.getAttribute("data-bs-theme");
                setTheme(current === "dark" ? "light" : "dark");
            });
        }
    });
})();