import React, { createContext, useContext } from "react";
import { AuthContext } from "../auth/AuthContext";
import { AuthFetch } from "./ApiHandler";

export const ApiContext = createContext();

export const ApiProvider = ({ children }) => {
  const authContext = useContext(AuthContext);

  const Get = async (endpoint, params = null) => {
    const requestOptons = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authContext.authState?.token}`,
      },
    };

    let resultEndpoint = params ? ParamBuilder(endpoint, params) : endpoint;

    return await AuthFetch(resultEndpoint, requestOptons);
  };

  const Post = async (endpoint, body) => {
    const requestOptons = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authContext.authState?.token}`,
      },
      body: JSON.stringify(body),
    };

    return await AuthFetch(endpoint, requestOptons);
  };

  const value = {
    Get,
    Post,
  };

  return <ApiContext.Provider value={value}>{children}</ApiContext.Provider>;
};

const ParamBuilder = (endpoint, params) => {
  let result = endpoint;
  let isFirst = true;

  params?.foreach((item) => {
    if (isFirst) {
      result += `?${item.key}${item.value}`;
    } else {
      result += `&${item.key}${item.value}`;
    }
  });

  return result;
};
