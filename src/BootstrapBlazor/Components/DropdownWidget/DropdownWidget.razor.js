import EventHandler from "../../modules/event-handler.js"

export function init(id, invoke, options) {
    const el = document.getElementById(id);
    if (el === null) {
        return;
    }

    const invokeMethod = e => {
        const item = e.target;
        const items = [...el.querySelectorAll("[data-bs-toggle=\"menu\"]")];
        const index = items.indexOf(item);
        invoke.invokeMethodAsync(method, index, e.type === 'shown.bs.menu');
    }

    const { method } = options;
    EventHandler.on(el, 'shown.bs.menu', invokeMethod);
    EventHandler.on(el, 'hidden.bs.menu', invokeMethod);
}

export function dispose(id) {
    const el = document.getElementById(id);
    if (el) {
        EventHandler.off(el, 'shown.bs.menu');
        EventHandler.off(el, 'hidden.bs.menu');
    }
}
