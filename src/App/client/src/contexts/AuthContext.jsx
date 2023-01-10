import { object } from "prop-types";
import React, { createContext, useEffect, useState } from "react";
import { AuthService } from "../api";
import { infoModal } from "../core/Modals";
import "../api/axiosConfig";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const user = JSON.parse(localStorage.getItem("user"));
  const [authState, setAuthState] = useState(user);

  useEffect(() => {
    window.addEventListener("refreshUser", () => {
      setAuthState(JSON.parse(localStorage.getItem("user")));
    });

    return () => {
      window.removeEventListener("refreshUser", () => {
        setAuthState(JSON.parse(localStorage.getItem("user")));
      });
    };
  }, []);

  const setAuthInfo = (userData) => {
    localStorage.setItem("user", JSON.stringify(userData));
    setAuthState(userData);
  };

  const signout = async () => {
    try {
      await AuthService.postApiAuthLogout();
    } catch (ex) {}

    localStorage.clear();
    setAuthState(null);
    window.location.reload();
  };

  const isAuthenticated = () => {
    if (
      authState == null ||
      authState == undefined ||
      (Object.keys(authState).length === 0 && authState.constructor === object)
    )
      return false;

    return true;
  };

  const value = {
    authState,
    setAuthState: (authInfo) => setAuthInfo(authInfo),
    signout,
    isAuthenticated,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export { AuthContext, AuthProvider };
