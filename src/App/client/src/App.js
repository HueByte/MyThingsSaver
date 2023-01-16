import "./App.scss";
import { BrowserRouter } from "react-router-dom";
import { createBrowserHistory } from "history";
import { ClientRouter } from "./routes/Routes";
import { AuthProvider } from "./contexts/AuthContext";
import Loader from "./components/Loaders/Loader";
import { Suspense } from "react";

// notifications
import { ReactNotifications } from "react-notifications-component";
import "react-notifications-component/dist/theme.css";
import "animate.css";

// modals
import Modal from "react-modal";

Modal.setAppElement("#root");

function App() {
  const history = createBrowserHistory();

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
    </BrowserRouter>
  );
}

const ErrorBoundary = ({ children }) => {
  return <>{children}</>;
};

export default App;
