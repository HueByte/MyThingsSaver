import axios from "axios";
import { createContext, Provider } from "react";
import { ApiEndpoint } from "./ApiEndpoints";
import { IApiResponse } from "./models/IApiResponse";

const HttpContext = createContext<any | null>(null);

const HttpProvider = ({ children }) => {
    const httpClient = axios.create({
        baseURL: ApiEndpoint,
        timeout: 10000,
    })

    httpClient.interceptors.request.use(
        Request => {

        },
        error => {

        }
    )

    const requestHandler = request => {
        return request;
    }

    const responseHandler = response => {

    }

    const errorHandler = error => {

    }

    return (
        <HttpContext.Provider value={{ httpClient }}>{children}</HttpContext.Provider>  
    );
}

export { HttpProvider, HttpContext };