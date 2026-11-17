import Data from "../../modules/data.js"
import EventHandler from "../../modules/event-handler.js"
import bootstrap from "../../modules/bootstrap.js"

export function init(id, invoke, method, options) {
    const el = document.getElementById(id);
    if (el === null) {
        return;
    }

    // null options would replace the defaults of Bootstrap
    const config = Object.fromEntries(Object.entries(options).filter(([, value]) => value !== null && value !== undefined));
    const spy = bootstrap.ScrollSpy.getOrCreateInstance(el, config);
    Data.set(id, { el, spy });

    EventHandler.on(el, 'activate.bs.scrollspy', e => {
        const link = e.relatedTarget;
        invoke.invokeMethodAsync(method, link && link.getAttribute ? link.getAttribute('href') : null);
    });
}

export function refresh(id) {
    const data = Data.get(id);
    if (data) {
        data.spy.refresh();
    }
}

export function dispose(id) {
    const data = Data.get(id);
    Data.remove(id);

    if (data) {
        EventHandler.off(data.el, 'activate.bs.scrollspy');
        data.spy.dispose();
    }
}
