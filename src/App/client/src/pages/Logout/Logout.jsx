import React, { useContext, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import { AuthContext } from "../../auth/AuthContext";

const Logout = () => {
  const authContext = useContext(AuthContext);
  const [redirect, setRedirect] = useState(false);

  useEffect(() => {
    (async () => {
      await authContext.signout();
      setRedirect(true);
    })();
  }, []);

  const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

  return <>{redirect ? <Navigate to="/auth/login" /> : <>logging out...</>}</>;
};

export default Logout;
