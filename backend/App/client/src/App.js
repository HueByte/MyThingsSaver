import "./App.css";
import { BrowserRouter } from "react-router-dom";
import { createBrowserHistory } from "history";
import { ClientRouter } from "./routes/Routes";
import { AuthProvider } from "./auth/AuthContext";

// notifications
import ReactNotifications from "react-notifications-component";
import "react-notifications-component/dist/theme.css";
import { store } from "react-notifications-component";
import "animate.css";

// modals
import Modal from "react-modal";
import { CategoryProvider } from "./contexts/CategoryContext";

Modal.setAppElement("#root");

function App() {
  const history = createBrowserHistory();
  return (
    <BrowserRouter history={history}>
      <AuthProvider>
        <CategoryProvider>
          <ReactNotifications isMobile={true} />
          <ClientRouter />
        </CategoryProvider>
      </AuthProvider>
    </BrowserRouter>
  );
}

export default App;
