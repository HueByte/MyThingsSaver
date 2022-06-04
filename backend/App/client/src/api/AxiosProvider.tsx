import axios from "axios";
import { createContext, Provider } from "react";
import { SilentRefresh } from "../auth/Auth";
import { ApiEndpoint } from "./ApiEndpoints";
import { IApiResponse } from "./models/IApiResponse";

const HttpContext = createContext<any | null>(null);

const HttpProvider = ({ children }) => {
  const httpClient = axios.create({
    baseURL: ApiEndpoint,
    timeout: 10000,
  });

  const requestHandler = (request) => {
    return request;
  };

  const responseHandler = async (response): Promise<IApiResponse> => {
    if (response.status == 200) {
      let result: IApiResponse = await response.json();
      return result;
    }

    return Promise.reject("Something went wrong");
  };

  const errorHandler = async (error): Promise<IApiResponse> => {
    if (error.status == 401) {
      let silentResponse: IApiResponse = await SilentRefresh().then(
        (response) => response.json()
      );

      if (silentResponse.IsSuccess) {
        return httpClient.request(error.config);
      }
    }

    return Promise.reject(error);
  };

  httpClient.interceptors.request.use(
    (Request) => requestHandler(Request),
    (Error) => errorHandler(Error)
  );

  httpClient.interceptors.response.use(
    (Response) => responseHandler(Response),
    (error) => errorHandler(Response)
  );

  return (
    <HttpContext.Provider value={{ httpClient }}>
      {children}
    </HttpContext.Provider>
  );
};

export { HttpProvider, HttpContext };
