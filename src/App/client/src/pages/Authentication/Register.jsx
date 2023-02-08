import React, { useContext, useEffect, useRef, useState } from "react";
import { NavLink, Navigate } from "react-router-dom";
import "./Auth.scss";
import "../../core/BasicLayout/BasicLayoutStyles.scss";
import AuthTemplate from "./AuthTemplate";
import { AuthContext } from "../../contexts/AuthContext";
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
  const [agreed, setAgreed] = useState(false);
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

    if (!agreed) {
      warningModal("You must agree to the Legal Notice");
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
      return;
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
      <div className="legal">
        <label className="switch-s switch">
          <input
            type="checkbox"
            checked={agreed}
            onChange={() => setAgreed(!agreed)}
          />
          <span class="slider"></span>
        </label>
        <div>
          I have read and agree to the{" "}
          <NavLink to="/LegalNotice">Legal Notice</NavLink>
        </div>
      </div>
      <div
        className="mts-button mts-bg-gradient-r full-button"
        onKeyDown={handleEnter}
        onClick={() => setIsWorking(true)}
      >
        Create Account
      </div>
      <NavLink
        to="/auth/login"
        className="mts-button mts-bg-gradient-r full-button"
      >
        Sign in
      </NavLink>
    </AuthTemplate>
  );
};

export default Register;
