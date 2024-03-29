/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {},
    colors: {
      transparent: "transparent",
      element: "#000c14",
      elementLight: "#001829",
      accent: "#00fa9a",
      accent2: "#c62368",
      accent3: "#fa7268",
      accent4: "#da3f67",
      accent5: "#861657",
      accent6: "#7300ff",
      accent7: "#FFA69E",
      textColor: "#e1e0e0",
      textColorLight: "#FFF",
      textPassive: "#4b4b4b",
      buttonText: "#fff",
      linkColor: "#FFA69E",
      linkColorHover: "#00fa9a",
      primary: "#c62368",
      primaryLight: "#00fa9a",
      primaryDark: "#861657",
      secondary: "#fa7268",
      secondaryLight: "#FFA69E",
      secondaryDark: "#da3f67",
      neutralDarker: "#7300ff",
      backgroundColorLight: "#011a2e",
      backgroundColor: "#001220",
      altBackgroundColor: "#000c14",
      altBackgroundColorLight: "#051929",
    },
    screens: {
      xl: { max: "1279px" },
      // => @media (max-width: 1279px) { ... }

      lg: { max: "1023px" },
      // => @media (max-width: 1023px) { ... }

      md: { max: "767px" },
      // => @media (max-width: 767px) { ... }

      sm: { max: "639px" },
      // => @media (max-width: 639px) { ... }
    },
  },
  plugins: [
    "tailwindcss",
    "autoprefixer",
    "postcss-import",
    "tailwindcss/nesting",
  ],
};
