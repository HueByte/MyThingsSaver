// import "./styles/_variables.scss";
import "./App.css";
import { BrowserRouter } from "react-router-dom";
import { createBrowserHistory } from "history";
import { ClientRouter } from "./routes/Routes";
import { AuthProvider } from "./auth/AuthContext";
import Particles from "react-tsparticles";

// notifications
import ReactNotifications from "react-notifications-component";
import "react-notifications-component/dist/theme.css";
import { store } from "react-notifications-component";
import "animate.css";

// modals
import Modal from "react-modal";
import { Suspense } from "react";
import Loader from "./components/Loaders/Loader";

Modal.setAppElement("#root");

function App() {
  const history = createBrowserHistory();

  const particlesLoaded = (container) => {
    console.log(container);
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
      <Particles
        id="tsparticles"
        loaded={particlesLoaded}
        options={ParticlesOptions}
      />
    </BrowserRouter>
  );
}

const ErrorBoundary = ({ children }) => {
  return <>{children}</>;
};

export default App;

const ParticlesOptions = {
  fpsLimit: 45,
  interactivity: {
    events: {
      onHover: {
        enable: true,
        mode: "grab",
      },
      resize: true,
    },
  },
  particles: {
    color: {
      value: "#ffffff",
    },
    links: {
      color: "#ffffff",
      distance: 150,
      enable: true,
      opacity: 0.5,
      width: 1,
    },
    collisions: {
      enable: false,
    },
    move: {
      direction: "none",
      enable: true,
      outMode: "out",
      random: false,
      speed: 0.5,
      straight: false,
    },
    number: {
      density: {
        enable: true,
        area: 1000,
      },
      value: 32,
    },
    opacity: {
      value: 0.5,
    },
    shape: {
      type: "circle",
    },
    size: {
      random: true,
      value: 5,
    },
  },
  detectRetina: true,
};
