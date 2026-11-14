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
            bootstrap.Dialog.getOrCreateInstance(dialog).hide()
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
