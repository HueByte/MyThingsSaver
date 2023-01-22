import React, { useContext, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import { AuthContext } from "../../contexts/AuthContext";

const Logout = () => {
  const authContext = useContext(AuthContext);
  const [redirect, setRedirect] = useState(false);

  useEffect(() => {
    (async () => {
      await authContext.signout();
      setRedirect(true);
    })();
  }, []);

  return <>{redirect ? <Navigate to="/auth/login" /> : <>logging out...</>}</>;
};

export default Logout;
