﻿document.addEventListener('alpine:init', () => {
    Alpine.magic('fetchjson', () => (url) => fetch(url).then((response) => response.json()))
    Alpine.magic('fetchok', () => (url, method) => fetch(url, {method: method ?? 'GET'}).then((response) => response.ok))
    Alpine.magic('elemById', () => (id) => document.getElementById(id))
    Alpine.magic('page', (el) => getPageController(el, Alpine))
    Alpine.directive('page', (el, { expression }) => {
        const controller = getPageController(el, Alpine);
        const pageNumber = Alpine.evaluate(el, expression)
        if (!Number.isInteger(pageNumber)) return;
        controller.registerPage(el, pageNumber)
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