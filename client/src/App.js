import './App.css';
import { Router } from 'react-router-dom';
import { createBrowserHistory } from 'history';
import { Routes } from './routes/Routes';
import { AuthProvider } from './auth/AuthContext';

// notifications
import ReactNotifications from 'react-notifications-component';
import 'react-notifications-component/dist/theme.css';
import { store } from 'react-notifications-component';
import 'animate.css';

function App() {
  const history = createBrowserHistory();
  return (
    <Router history={history}>
      <AuthProvider>
        <ReactNotifications isMobile={true} />
        <Routes />
      </AuthProvider>
    </Router>
  );
}

export default App;
