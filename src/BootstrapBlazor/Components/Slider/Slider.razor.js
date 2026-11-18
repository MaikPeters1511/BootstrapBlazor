import Data from "../../modules/data.js"
import bootstrap from "../../modules/bootstrap.js"

export function init(id, options) {
    const input = document.getElementById(id);
    const wrapper = input ? input.closest('.form-range') : null;
    if (wrapper === null) {
        return;
    }

    const range = bootstrap.Range.getOrCreateInstance(wrapper, options);
    Data.set(id, { range });
}

export function update(id) {
    const data = Data.get(id);
    if (data) {
        data.range.update();
    }
}

export function dispose(id) {
    const data = Data.get(id);
    Data.remove(id);

    if (data) {
        data.range.dispose();
    }
}
