/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: "class",
    content: ["./Views/**/*.cshtml"],
    theme: {
        extend: {
            maxWidth: {
                'nav': '1280px',
            },
            gridTemplateColumns: {
                'responsive': 'repeat(auto-fill, minmax(15rem, 1fr))'
            },
            colors:{
                'primary': '#f4f4f4',
                'secondary': '#fa4968',
                'aqua': '#8cf2ed',
                'skin': '#ffca8a'
            }
        }
    },
    plugins: [],
}

