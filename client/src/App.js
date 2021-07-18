import './App.css';
import { Router } from 'react-router-dom';
import { createBrowserHistory } from 'history';
import { Routes } from './routes/Routes';
import { AuthProvider } from './auth/AuthContext';

function App() {
  const history = createBrowserHistory();
  return (
    <Router history={history}>
      <AuthProvider>
        <Routes />
      </AuthProvider>
    </Router>
  );
}

export default App;
