import Data from "../../modules/data.js"
import EventHandler from "../../modules/event-handler.js"
import bootstrap from "../../modules/bootstrap.js"

export function init(id, invoke, method, options) {
    const el = document.getElementById(id);
    if (el === null) {
        return;
    }

    const strengthEl = el.querySelector('.strength');
    const strength = bootstrap.Strength.getOrCreateInstance(strengthEl, options);
    Data.set(id, { strengthEl, strength });

    EventHandler.on(strengthEl, 'strengthChange.bs.strength', e => {
        invoke.invokeMethodAsync(method, e.strength ?? null);
    });
}

export function dispose(id) {
    const data = Data.get(id);
    Data.remove(id);

    if (data) {
        EventHandler.off(data.strengthEl, 'strengthChange.bs.strength');
        data.strength.dispose();
    }
}
