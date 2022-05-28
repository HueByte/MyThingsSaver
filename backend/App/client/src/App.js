// import "./styles/_variables.scss";
import "./App.css";
import { BrowserRouter } from "react-router-dom";
import { createBrowserHistory } from "history";
import { ClientRouter } from "./routes/Routes";
import { AuthProvider } from "./auth/AuthContext";
import Particles from "react-tsparticles";
import { loadFull } from "tsparticles";

// notifications
import { ReactNotifications } from "react-notifications-component";
import "react-notifications-component/dist/theme.css";
import "animate.css";

// modals
import Modal from "react-modal";
import { Suspense } from "react";
import Loader from "./components/Loaders/Loader";

Modal.setAppElement("#root");

function App() {
  const history = createBrowserHistory();

  const init = async (main) => {
    await loadFull(main);
  };

  return (
    <BrowserRouter history={history}>
      <AuthProvider>
        <Suspense fallback={<Loader />}>
          <ErrorBoundary>
            <ReactNotifications isMobile={true} />
            <ClientRouter />
          </ErrorBoundary>
        </Suspense>
      </AuthProvider>
      <Particles id="tsparticles" options={ParticlesOptions} init={init} />
    </BrowserRouter>
  );
}

const ErrorBoundary = ({ children }) => {
  return <>{children}</>;
};

export default App;

const ParticlesOptions = {
  fpsLimit: 45,
  particles: {
    links: {
      distance: 120,
      enable: true,
      triangles: {
        enable: true,
        opacity: 0.4,
      },
    },
    move: {
      enable: true,
      speed: 1,
    },
    size: {
      value: 0.7,
    },
    shape: {
      type: "circle",
    },
    number: {
      density: {
        enable: true,
        area: 1000,
      },
      value: 32,
    },
  },
};
