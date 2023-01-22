import { useContext } from "react";
import { Navigate, useNavigate } from "react-router";
import { AuthContext } from "../contexts/AuthContext";

export const PrivateRoute = ({ roles, source, children }) => {
  const navigate = useNavigate();
  const authContext = useContext(AuthContext);

  if (!authContext.isAuthenticated()) {
    console.log("q");
    return <Navigate to="/auth/login" replace />;
  }

  if (!authContext.isInRole(roles)) {
    return <Navigate to="/" replace />;
  }

  return children;
};

export default PrivateRoute;
