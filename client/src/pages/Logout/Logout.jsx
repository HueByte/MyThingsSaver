import React, { useContext, useEffect, useState } from "react";
import { AuthContext } from "../../auth/AuthContext";

const Logout = () => {
  const authContext = useContext(AuthContext);
  const [redirect, setRedirect] = useState(false);
  useEffect(() => {
    authContext.signout();
    setRedirect(true);
  }, []);

  return <>{redirect ? <Redirect to="/auth/login" /> : <>logging out...</>}</>;
};

export default Logout;
