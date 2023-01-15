import React, { useContext } from "react";
import { Navigate } from "react-router";
import { AuthContext } from "../contexts/AuthContext";

export const PrivateRoute = ({ roles, children }) => {
  const authContext = useContext(AuthContext);
  if (!authContext.isAuthenticated()) return <Navigate to="/auth/login" />;

  if (roles && authContext.isInRole([roles])) return <Navigate to="/" />;

  return children;
};

export default PrivateRoute;
