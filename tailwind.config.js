module.exports = {
    content: ["./**/*.{razor,html,cshtml,razor.css}"],
    theme: {
        extend: {},
        screens: {
            'sm': '640px',
            // => @media (min-width: 640px) { ... }

            'md': '768px',
            // => @media (min-width: 768px) { ... }

            'lg': '1024px',
            // => @media (min-width: 1024px) { ... }

            'xl': '1280px',
            // => @media (min-width: 1280px) { ... }

            '2xl': '1536px',
            // => @media (min-width: 1536px) { ...

            '2xlx': { 'max': '1535px' },
            // => @media (max-width: 1535px) { ... }

            'xlx': { 'max': '1279px' },
            // => @media (max-width: 1279px) { ... }

            'lgx': { 'max': '1023px' },
            // => @media (max-width: 1023px) { ... }

            'mdx': { 'max': '767px' },
            // => @media (max-width: 767px) { ... }

            'smx': { 'max': '639px' },
            // => @media (max-width: 639px) { ... }
        }
    },
    plugins: [],
}
