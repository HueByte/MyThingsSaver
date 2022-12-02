// import React from "react";
// import ReactDOM from "react-dom";
import { createRoot } from "react-dom/client";
import App from "./App";
import "./index.scss";
import * as serviceWorkerRegistration from "./serviceWorkerRegistration";
import reportWebVitals from "./reportWebVitals";
import bg from "./assets/Sprinkle.svg";

const container = document.getElementById("root");
container.style.backgroundImage = `url(${bg}`;
const root = createRoot(container);
root.render(<App />);

serviceWorkerRegistration.unregister();
reportWebVitals();
