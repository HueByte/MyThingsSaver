import './App.css';
import { Router } from 'react-router-dom';
import { createBrowserHistory } from 'history';
import { Routes } from './routes/Routes';

function App() {
  const history = createBrowserHistory();
  return (
    <Router history={history}>
      <Routes />
    </Router>
  );
}

export default App;
