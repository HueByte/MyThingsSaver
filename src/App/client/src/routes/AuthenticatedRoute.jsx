import to from "kute.js/src/interface/to";
import { useEffect, useContext } from "react";
import { Navigate, useNavigate } from "react-router";
import { AuthContext } from "../contexts/AuthContext";

export const PrivateRoute = ({ roles, source, children }) => {
  const navigate = useNavigate();
  const authContext = useContext(AuthContext);

  if (!authContext.isAuthenticated()) {
    return <Navigate to="/auth/login" replace />;
  }

  if (!authContext.isInRole(roles)) {
    return <Navigate to={-1} replace />;
  }

  return children;
};

export default PrivateRoute;
