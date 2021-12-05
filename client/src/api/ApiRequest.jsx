import React, { useContext } from "react";
import { AuthContext } from "../auth/AuthContext";

export const Api = () => {
  authContext = useContext(AuthContext);

  export const ApiGet = (endpoint, body) => {
    const requestOptons = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authContext.authState?.token}`,
      },
      body: JSON.stringify(body),
    };

    return await AuthFetch(endpoint, body);
  };

  export const ApiPost = () => {};
};
