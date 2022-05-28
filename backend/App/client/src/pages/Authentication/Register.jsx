import React, { useContext, useEffect, useRef, useState } from "react";
import { NavLink, Navigate } from "react-router-dom";
import "./Auth.css";
import "../../core/BasicLayout/BasicLayoutStyles.scss";
import AuthTemplate from "./AuthTemplate";
import { AuthRegister } from "../../auth/Auth";
import { AuthContext } from "../../auth/AuthContext";
import { infoModal, successModal, warningModal } from "../../core/Modals";

const Register = () => {
  const authContext = useContext(AuthContext);
  const [isWorking, setIsWorking] = useState(false);
  const email = useRef();
  const username = useRef();
  const password = useRef();

  useEffect(() => {
    email.current = document.getElementById("email-input");
    username.current = document.getElementById("username-input");
    password.current = document.getElementById("password-input");
  }, []);

  useEffect(() => {
    (async () => {
      if (isWorking) await register();
    })();
  }, [isWorking]);

  const register = async () => {
    if (
      email.current.value.length === 0 ||
      username.current.value.length === 0 ||
      password.current.value.length === 0
    ) {
      warningModal("Please fill all of the fields");
      setIsWorking(false);
      return;
    }

    infoModal("Creating account...");
    await AuthRegister(
      email.current.value,
      username.current.value,
      password.current.value
    )
      .then((result) => {
        if (result.isSuccess)
          successModal(
            `You can now log in. User ${username.current.value} created!`,
            10000
          );
      })
      .catch((errors) => console.error(errors));

    setIsWorking(false);
  };

  const handleEnter = async (event) => {
    if (event.key === "Enter") await register();
  };

  if (authContext.isAuthenticated()) return <Navigate to="/" />;
  return (
    <AuthTemplate isWorking={isWorking}>
      <div className="auth-welcome">
        <span>Welcome to My things saver!</span>
        <br />
        <span>Register your account here</span>
      </div>
      <div className="auth-input-container">
        <input
          id="email-input"
          type="text"
          className="basic-input auth-input"
          placeholder="E-mail"
          autoComplete="email"
        />
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
        <div className="auth-menu-side"></div>
        <div className="auth-menu-side">
          <div
            onKeyDown={handleEnter}
            onClick={() => setIsWorking(true)}
            className="basic-button auth-button"
          >
            Register
          </div>
          <NavLink to="/auth/login" className="basic-button auth-button">
            Log in
          </NavLink>
        </div>
      </div>
    </AuthTemplate>
  );
};

export default Register;
