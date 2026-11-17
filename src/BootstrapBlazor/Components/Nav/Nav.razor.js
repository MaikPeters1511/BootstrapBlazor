import Data from "../../modules/data.js"
import bootstrap from "../../modules/bootstrap.js"

export function init(id, options) {
    const el = document.getElementById(id);
    if (el === null) {
        return;
    }

    const overflow = bootstrap.NavOverflow.getOrCreateInstance(el, options);
    Data.set(id, { overflow });
}

export function dispose(id) {
    const data = Data.get(id);
    Data.remove(id);

    if (data) {
        data.overflow.dispose();
    }
}
