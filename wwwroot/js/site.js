(function () {
    const path = location.pathname.toLowerCase();

    document.querySelectorAll('.ms-nav-link[data-page]').forEach(function (link) {
        const href = (link.getAttribute('href') || '').toLowerCase();
        if (href && path.indexOf(href.replace(/^\//, '')) !== -1) {
            link.classList.add('active');
        }
    });

    const toggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.ms-sidebar');
    const overlay = document.getElementById('sidebarOverlay');

    if (toggle && sidebar) {
        toggle.addEventListener('click', function () {
            sidebar.classList.toggle('open');
            overlay?.classList.toggle('show');
        });
    }

    if (overlay && sidebar) {
        overlay.addEventListener('click', function () {
            sidebar.classList.remove('open');
            overlay.classList.remove('show');
        });
    }

    function normalizePath(value) {
        const normalized = (value || '').toLowerCase().replace(/\/$/, '');
        return normalized || '/';
    }

    const currentPath = normalizePath(path);

    document.querySelectorAll('.ms-auth-tabs .ms-auth-tab').forEach(function (tab) {
        const href = tab.getAttribute('href');
        if (!href) {
            return;
        }

        let tabPath = href;
        try {
            tabPath = new URL(href, location.origin).pathname;
        } catch (e) {
            // Keep relative href fallback.
        }

        if (normalizePath(tabPath) === currentPath) {
            tab.classList.add('active');
        }
    });
})();
