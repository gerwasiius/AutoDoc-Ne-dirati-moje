function initTooltips() {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        var tooltip = new bootstrap.Tooltip(tooltipTriggerEl);

        tooltipTriggerEl.addEventListener('click', () => {
                tooltip.hide();
            }
        );

        return tooltip;
    })
}