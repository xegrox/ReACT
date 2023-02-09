document.addEventListener('alpine:init', () => {
    const jsonHeader = { 'Content-Type': 'application/json' }
    const resErr = (url, method) => Promise.reject(new Error(`Error response ${method}: ${url}`))
    const fetchRes = (url, method, body, bodyIsJson) => fetch(url, {method: method ?? 'GET', body, headers: bodyIsJson && jsonHeader})
    const fetchThrow = (url, method, body, bodyIsJson) => fetchRes(url, method, body, bodyIsJson).then((response) => response.ok ? Promise.resolve(response) : resErr(url, method))
    Alpine.magic('fetch', () => fetchThrow)
    Alpine.magic('fetchjson', () => (url, method, body, bodyIsJson) => fetchThrow(url, method, body, bodyIsJson).then((response) => response.json()))
    Alpine.magic('fetchok', () => (url, method, body, bodyIsJson) => fetchRes(url, method, body, bodyIsJson).then((response) => response.ok))
    Alpine.magic('elemById', () => (id) => document.getElementById(id))
    Alpine.magic('page', (el) => getPageController(el, Alpine))
    Alpine.directive('loading', (el, {expression}) => {
        const getLoading = Alpine.evaluateLater(el, expression)
        const spinner = document.createElement("div")
        spinner.innerHTML = '<svg class="animate-spin -ml-1 mr-3 h-5 w-5" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">\n' +
            '        <circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4"></circle>\n' +
            '        <path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>\n' +
            '      </svg>'
        Alpine.effect(() => {
            getLoading(loading => {
                el.disabled = loading
                loading ? el.prepend(spinner) : spinner.remove()
            })
        })
    })
    Alpine.directive('page', (el, { expression }) => {
        const controller = getPageController(el, Alpine);
        const pageNumber = Alpine.evaluate(el, expression)
        if (!Number.isInteger(pageNumber)) return;
        controller.registerPage(el, pageNumber)
    })
    Alpine.magic('addUrlParam', () => (param, value) => {
        const url = new URL(window.location.href)
        url.searchParams.set(param, value)
        return url.toString()
    })
    Alpine.magic('transEnd', (el) => (cb) => {
        el.addEventListener('transitionend', cb, {once: true})
    })
})

const pageControllers = new WeakMap()
const getPageController = (el, Alpine) => {
    const root = Alpine.closestRoot(el)
    if (!pageControllers.has(root)) pageControllers.set(root, createPageController(Alpine))
    return pageControllers.get(root)
}

const createPageController = (Alpine) => {
    return Alpine.reactive({
        size: 0,
        currentIndex: 1,
        animate: true,
        registerPage(el, pageNumber) {
            this.size++
            el.classList.add("absolute", "opacity-100")
            const transition = "transition-[transform,opacity]"
            const translateLeft = ["-translate-x-full", "!opacity-0"]
            const translateRight = ["translate-x-full", "!opacity-0"]
            Alpine.effect(() => {
                if (this.animate) el.classList.add(transition)
                else el.classList.remove(transition)
            })
            Alpine.effect(() => {
                if (this.currentIndex === pageNumber) {
                    el.classList.remove(...translateLeft)
                    el.classList.remove(...translateRight)
                } else if (this.currentIndex < pageNumber) {
                    el.classList.remove(...translateLeft)
                    el.classList.add(...translateRight)
                } else {
                    el.classList.remove(...translateRight)
                    el.classList.add(...translateLeft)
                }
            })
        },
        next() { if (this.currentIndex < this.size) { this.currentIndex++ } },
        prev() { if (this.currentIndex > 0) this.currentIndex-- },
        reset() { this.animate = false; this.currentIndex = 1; this.animate = true }
    })
}