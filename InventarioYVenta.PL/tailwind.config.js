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
            }
        },
    },
    plugins: [],
}

