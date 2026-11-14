import Data from "../../modules/data.js"
import EventHandler from "../../modules/event-handler.js"

export function init(id, invoke, shownCallback, closeCallback) {
    const el = document.getElementById(id)
    const modal = {
        el,
        invoke,
        shownCallback,
        closeCallback,
        pop: () => {
            modal.hide();
        }
    }
    Data.set(id, modal)

    EventHandler.on(el, 'shown.bs.dialog', () => {
        invoke.invokeMethodAsync(shownCallback)
    })
    EventHandler.on(el, 'hidden.bs.dialog', e => {
        e.stopPropagation();
        invoke.invokeMethodAsync(closeCallback)
    })
    EventHandler.on(window, 'popstate', modal.pop)

    // A native dialog closes itself on Escape and the browser does not let a page cancel a second Escape.
    // Take the key here: keydown with preventDefault stops the close request, the component decides
    modal.handlerEscape = e => {
        if (e.key !== 'Escape' || e.defaultPrevented || !modal.getShownDialog()) {
            return
        }
        e.preventDefault()
        if (el.getAttribute('data-bs-keyboard') === 'true') {
            invoke.invokeMethodAsync('RequestCloseCallback')
        }
    }
    EventHandler.on(document, 'keydown', modal.handlerEscape)

    // A click on the backdrop makes Bootstrap hide the dialog on its own: stop that and hand it over
    // to the component, so OnClosingAsync runs (and can prevent the close) like for the close button
    EventHandler.on(el, 'hide.bs.dialog', e => {
        if (modal.forceHide) {
            return
        }
        e.preventDefault()
        invoke.invokeMethodAsync('RequestCloseCallback')
    })

    // every ModalDialog is a native <dialog> element, the last one is the top most
    modal.getDialogs = () => [...el.children].filter(d => d.tagName === 'DIALOG')

    modal.getShownDialog = () => modal.getDialogs().filter(d => d.open).pop()

    modal.show = () => {
        const dialog = modal.getDialogs().pop()
        if (dialog && !dialog.open) {
            dialog.classList.toggle('dialog-instant', !el.classList.contains('fade'))

            const instance = bootstrap.Dialog.getOrCreateInstance(dialog)
            instance._config.keyboard = el.getAttribute('data-bs-keyboard') === 'true'
            instance._config.backdrop = el.getAttribute('data-bs-backdrop') === 'static' ? 'static' : true
            instance.show()
        }
    }

    modal.hide = () => {
        const dialog = modal.getShownDialog()
        if (dialog) {
            // the component already ran OnClosingAsync, do not ask again
            modal.forceHide = true
            try {
                bootstrap.Dialog.getOrCreateInstance(dialog).hide()
            }
            finally {
                modal.forceHide = false
            }
        }
    }

    modal.toggle = () => {
        if (modal.getShownDialog()) {
            modal.hide()
        }
        else {
            modal.show()
        }
    }
}

export function execute(id, method) {
    const modal = Data.get(id)
    if (method === 'show') {
        modal.show()
    }
    else if (method === 'hide') {
        modal.hide()
    }
    else if (method === 'toggle') {
        modal.toggle()
    }
}

export function dispose(id) {
    const modal = Data.get(id)
    Data.remove(id)

    if (modal) {
        EventHandler.off(modal.el, 'shown.bs.dialog')
        EventHandler.off(modal.el, 'hidden.bs.dialog')
        EventHandler.off(modal.el, 'hide.bs.dialog')
        EventHandler.off(document, 'keydown', modal.handlerEscape)
        EventHandler.off(window, 'popstate', modal.pop)

        modal.getDialogs().forEach(dialog => {
            const instance = bootstrap.Dialog.getInstance(dialog)
            if (instance) {
                // close without animation and release the scroll lock
                dialog.classList.add('dialog-instant')
                instance.dispose()
            }
        })
    }
}
