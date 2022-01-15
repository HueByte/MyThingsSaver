import React, { Component, useContext, useEffect } from "react";
import { Navigate } from "react-router";
import { AuthContext } from "../auth/AuthContext";

export const PrivateRoute = ({ roles, children }) => {
  const authContext = useContext(AuthContext);
  if (!authContext.isAuthenticated()) return <Navigate to="/auth/login" />;

  if (roles && roles.indexOf(authContext.authState.roles) === -1)
    return <Navigate to="/" />;

  return children;
};

export default PrivateRoute;
