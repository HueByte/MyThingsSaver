import axios, { AxiosError, AxiosInstance, AxiosResponse } from "axios";
import { createContext, useState } from "react";
import { AuthLogout, SilentRefresh } from "../auth/Auth.jsx";
import { ApiEndpoint } from "./ApiEndpoints";
import { IApiResponse } from "./models/IApiResponse";

// repositories as hooks

interface IHttpProviderValue {
  client: AxiosInstance;
}

const HttpContext = createContext<any | null>(null);

const HttpProvider = ({ children }) => {
  const httpClient: AxiosInstance = axios.create({
    baseURL: ApiEndpoint,
    timeout: 10000,
    headers: {
      Accept: "application/json",
    },
  });

  const requestHandler = (request) => {
    return request;
  };

  // 200 status codes
  const responseHandler = async (
    response: AxiosResponse
  ): Promise<IApiResponse> => {
    let result: IApiResponse = await response.data;
    console.log(result);
    return result;
  };

  const errorHandler = async (error): Promise<IApiResponse> => {
    if (axios.isAxiosError(error)) {
      const { status } = error.response;

      if (error) {
        if (status == 401) {
          let silentResponse = await SilentRefresh();
          let silentResult: IApiResponse = await silentResponse.json();

          if (silentResult.IsSuccess) {
            return await httpClient.request(error.config); //retry
          }

          // logout
          window.location.replace(
            `${window.location.protocol}//${window.location.host}/logout`
          );
        } else if (status == 400) {
          console.log(error.response);
        }
      }
    }

    return Promise.reject(error);
  };

  httpClient.interceptors.request.use(
    (Request) => requestHandler(Request),
    (Error: Response) => errorHandler(Error)
  );

  httpClient.interceptors.response.use(
    (Response: AxiosResponse) => responseHandler(Response),
    (error: Response) => errorHandler(Response)
  );

  const value: IHttpProviderValue = {
    client: httpClient,
  };

  return <HttpContext.Provider value={value}>{children}</HttpContext.Provider>;
};

export { HttpProvider, HttpContext };
