(function () {
    window.initializeDeleteConfirm = function initializeDeleteConfirm(rootElementId) {
        var rootElement = document.getElementById(rootElementId);
        if (!rootElement || !window.Vue) {
            return;
        }

        Vue.createApp({
            data: function () {
                return {
                    isDeleteConfirmOpen: false,
                    deleteIssueName: "",
                    pendingDeleteForm: null
                };
            },
            methods: {
                openDeleteConfirm: function (event) {
                    var button = event.currentTarget;
                    var form = button ? button.closest("form") : null;

                    if (!form) {
                        return;
                    }

                    this.pendingDeleteForm = form;
                    this.deleteIssueName = button.dataset.issueName || "this issue";
                    this.isDeleteConfirmOpen = true;
                },
                closeDeleteConfirm: function () {
                    this.pendingDeleteForm = null;
                    this.deleteIssueName = "";
                    this.isDeleteConfirmOpen = false;
                },
                submitDelete: function () {
                    if (!this.pendingDeleteForm) {
                        return;
                    }

                    this.pendingDeleteForm.submit();
                }
            }
        }).mount(rootElement);
    };
})();
