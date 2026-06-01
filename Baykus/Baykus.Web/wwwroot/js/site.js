// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {
    "use strict";

    var storageKey = "baykus-theme";

    function getCurrentTheme() {
        var theme = document.documentElement.dataset.theme;

        if (theme === "dark" || theme === "light") {
            return theme;
        }

        return "light";
    }

    function setTheme(theme) {
        var safeTheme = theme === "dark" ? "dark" : "light";

        document.documentElement.dataset.theme = safeTheme;

        try {
            localStorage.setItem(storageKey, safeTheme);
        } catch {
            // Si localStorage no está disponible, el tema igual se aplica en la sesión actual.
        }

        updateThemeButton(safeTheme);
    }

    function updateThemeButton(theme) {
        var button = document.getElementById("themeToggle");

        if (!button) {
            return;
        }

        var icon = button.querySelector(".theme-toggle-icon");
        var text = button.querySelector(".theme-toggle-text");

        if (theme === "dark") {
            if (icon) icon.textContent = "☀️";
            if (text) text.textContent = "Claro";

            button.setAttribute("aria-label", "Cambiar a tema claro");
            button.setAttribute("title", "Cambiar a tema claro");
        } else {
            if (icon) icon.textContent = "🌙";
            if (text) text.textContent = "Oscuro";

            button.setAttribute("aria-label", "Cambiar a tema oscuro");
            button.setAttribute("title", "Cambiar a tema oscuro");
        }
    }

    document.addEventListener("DOMContentLoaded", function () {
        var button = document.getElementById("themeToggle");
        var currentTheme = getCurrentTheme();

        updateThemeButton(currentTheme);

        if (!button) {
            return;
        }

        button.addEventListener("click", function () {
            var nextTheme = getCurrentTheme() === "dark" ? "light" : "dark";
            setTheme(nextTheme);
        });
    });
})();