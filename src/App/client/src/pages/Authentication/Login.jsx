import { useContext, useEffect, useRef, useState } from "react";
import { NavLink, Navigate } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";
import "./Auth.scss";
import "../../core/BasicLayout/BasicLayoutStyles.scss";
import AuthTemplate from "./AuthTemplate";
import { errorModal, warningModal } from "../../core/Modals";
import { AuthService } from "../../api/services/AuthService";

const Login = () => {
  const authContext = useContext(AuthContext);
  const [isWorking, setIsWorking] = useState(false);
  const username = useRef();
  const password = useRef();

  useEffect(() => {
    username.current = document.getElementById("username-input");
    password.current = document.getElementById("password-input");
  }, []);

  useEffect(() => {
    (async () => {
      if (isWorking) await authenticate();
    })();
  }, [isWorking]);

  const authenticate = async () => {
    if (
      username.current.value.length === 0 ||
      password.current.value.length === 0
    ) {
      warningModal("Please fill all of the fields");
      setIsWorking(false);
      return;
    }

    var result = await AuthService.postApiAuthLogin({
      requestBody: {
        username: username.current.value,
        password: password.current.value,
      },
    });

    if (!result.isSuccess) errorModal(result.errors.join(".\n"), 20000);
    else authContext.setAuthState(result.data);

    setIsWorking(false);
  };

  const handleEnter = async (event) => {
    if (event.key === "Enter") await authenticate();
  };

  if (authContext.isAuthenticated()) return <Navigate to="/" />;
  return (
    <AuthTemplate isWorking={isWorking}>
      <div className="welcome">Sign in</div>
      <div className="input-container">
        <label>Username</label>
        <input
          id="username-input"
          type="text"
          placeholder="username"
          className="mts-input"
          autoComplete="on"
        />

        <label>Password</label>
        <input
          id="password-input"
          type="password"
          placeholder="password"
          className="mts-input"
          autoComplete="on"
        />
      </div>
      <div
        className="mts-button mts-bg-gradient-r full-button"
        onClick={() => setIsWorking(true)}
      >
        Continue
      </div>
      <div className="singup-text">
        Don't have account? <NavLink to="/auth/register">Sign up</NavLink>
      </div>
      <div>
        <NavLink to="/helpme">Forgot Password?</NavLink>
      </div>
    </AuthTemplate>
  );
};

export default Login;
