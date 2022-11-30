import React, { useContext, useEffect, useRef, useState } from "react";
import { NavLink, Navigate } from "react-router-dom";
// import { AuthLogin } from "../../auth/Auth";
import { AuthContext } from "../../auth/AuthContext";
import "./Auth.css";
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

    authContext.setAuthState(result.data);

    // await AuthLogin(username.current.value, password.current.value)
    //   .then((result) => {
    //     if (result.isSuccess) authContext.setAuthState(result.data);
    //     else errorModal(result?.errors.join("\n"), 10000);
    //   })
    //   .catch((errors) => {
    //     console.log(errors);
    //   });

    setIsWorking(false);
  };

  const handleEnter = async (event) => {
    if (event.key === "Enter") await authenticate();
  };

  if (authContext.isAuthenticated()) return <Navigate to="/" />;
  return (
    <AuthTemplate isWorking={isWorking}>
      <div className="auth-welcome">Welcome to My things saver!</div>
      <div className="auth-input-container">
        <input
          id="username-input"
          type="text"
          className="basic-input auth-input"
          placeholder="Username"
          autoComplete="username"
        />
        <input
          id="password-input"
          type="password"
          className="basic-input auth-input"
          placeholder="Password"
          autoComplete="current-password"
        />
      </div>
      <div className="auth-menu">
        <div className="auth-menu-side">
          <NavLink to="/HelpMe" className="auth-button-help">
            Can't log in?
          </NavLink>
        </div>
        <div className="auth-menu-side">
          <div
            onKeyDown={handleEnter}
            onClick={() => setIsWorking(true)}
            className="basic-button auth-button"
          >
            Log in
          </div>
          <NavLink to="/auth/register" className="basic-button auth-button">
            Register
          </NavLink>
        </div>
      </div>
    </AuthTemplate>
  );
};

export default Login;
