module.exports = {
    content: [
        // './src/**/*.{html,js}',      
        './node_modules/preline/dist/*.js',
        './Views/**/*.cshtml',
    ],
    plugins: [
        // require('@tailwindcss/forms'),      
        require('preline/plugin'),
    ],
}