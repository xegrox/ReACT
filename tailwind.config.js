module.exports = {
    content: [
        './Areas/*/Pages/**/*.cshtml',
        './Areas/*/Views/**/*.cshtml',
        './Pages/**/*.cshtml',
        './Views/**/*.cshtml'
    ],
    theme: {
        extend: {},
    },
    daisyui : {
        themes: [
            {
                default: {
                    "primary": "#0a7b79",
                    "primary-content": "#fafafa",
                    "secondary": "#7b0a0c",
                    "accent": "#4ade80",
                    "neutral": "#3D4451",
                    "base-100": "#FFFFFF"
                },
            },
        ],   
    },
    plugins: [require("daisyui")]
}