// Bootstrap 6 ships ES modules only. The bundle (core/bootstrap/js/bootstrap.bundle.min.js) is converted to a classic
// script that exposes window.bootstrap, so Bootstrap's own data API and the component scripts share one set of instances.
// Every script reaches Bootstrap through this module: swapping the bundle (or moving to a real import) touches this file only.
// The lookup is lazy, the global is read when a class is used, not when the module is loaded.
const bootstrap = new Proxy({}, {
    get: (_, name) => window.bootstrap[name],
    set: (_, name, value) => {
        window.bootstrap[name] = value;
        return true;
    }
});

export default bootstrap;
