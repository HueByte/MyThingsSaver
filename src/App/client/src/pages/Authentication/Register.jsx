import React, { useContext, useEffect, useRef, useState } from "react";
import { NavLink, Navigate } from "react-router-dom";
import "./Auth.scss";
import "../../core/BasicLayout/BasicLayoutStyles.scss";
import AuthTemplate from "./AuthTemplate";
// import { AuthRegister } from "../../auth/Auth";
import { AuthContext } from "../../auth/AuthContext";
import {
  errorModal,
  infoModal,
  successModal,
  warningModal,
} from "../../core/Modals";
import { AuthService } from "../../api/services/AuthService";

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
    var result = await AuthService.postApiAuthRegister({
      requestBody: {
        email: email.current.value,
        password: password.current.value,
        userName: username.current.value,
      },
    });

    if (result.isSuccess) {
      successModal(
        `You can now log in. User ${username.current.value} created!`,
        10000
      );
    } else {
      errorModal(result.errors.join(".\n"), 20000);
    }

    setIsWorking(false);
  };

  const handleEnter = async (event) => {
    if (event.key === "Enter") await register();
  };

  if (authContext.isAuthenticated()) return <Navigate to="/" />;
  return (
    <AuthTemplate isWorking={isWorking}>
      <div className="welcome">Sign up</div>
      <div className="input-container">
        <label>Email</label>
        <input
          type="text"
          id="email-input"
          className="mts-input"
          placeholder="email@domain.com"
        />
        <label>Username</label>
        <input
          type="text"
          id="username-input"
          className="mts-input"
          placeholder="username"
        />
        <label>Password</label>
        <input
          type="password"
          id="password-input"
          className="mts-input"
          placeholder="password"
        />
      </div>
      <div
        className="mts-button gradient-background-r full-button"
        onKeyDown={handleEnter}
        onClick={() => setIsWorking(true)}
      >
        Create Account
      </div>
      <NavLink
        to="/auth/login"
        className="mts-button gradient-background-r full-button"
      >
        Sign in
      </NavLink>
    </AuthTemplate>
  );
};

export default Register;
