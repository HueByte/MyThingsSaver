import { createRoot } from "react-dom/client";
import App from "./App";
import "./index.scss";
import "./styles/main.css";
import * as serviceWorkerRegistration from "./serviceWorkerRegistration";
import reportWebVitals from "./reportWebVitals";

const container = document.getElementById("root");
const root = createRoot(container);
root.render(<App />);

serviceWorkerRegistration.unregister();
reportWebVitals();
