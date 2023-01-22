import { object } from "prop-types";
import React, { createContext, useEffect, useState } from "react";
import { AuthService } from "../api";
import "../api/axiosConfig";
import DefaultAvatar from "../assets/DefaultAvatar.png";

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

  const getAvatar = () => {
    return authState.avatarUrl && !authState.avatarUrl.length >= 0
      ? authState.avatarUrl
      : DefaultAvatar;
  };

  const isInRole = (roles) => {
    if (roles === undefined || roles.length == 0) return true;

    let isInRole = roles.every((role) => authState.roles.includes(role));
    return isInRole;
  };

  const value = {
    authState,
    setAuthState: (authInfo) => setAuthInfo(authInfo),
    signout,
    isAuthenticated,
    isInRole,
    getAvatar,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export { AuthContext, AuthProvider };
