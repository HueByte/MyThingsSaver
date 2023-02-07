import axios, { AxiosRequestConfig, AxiosResponse } from 'axios'
import { errorModal } from '../core/Modals';
import { AuthService } from './services/AuthService';

const axiosApiInstance = axios.create();

axios.interceptors.response.use(
    (Response) => responseHandler(Response),
    (Error) => errorHandler(Error)
)

let isRefreshing = false;
let failedQueue: any[] = [];

const processQueue = (error: any) => {
    failedQueue.forEach(prom => {
        if (error) prom.reject(error);
        else prom.resolve();
    });

    failedQueue = [];
};

function responseHandler(response: AxiosResponse) {
    let result: ApiResponse = response.data;

    if (response.status == 200 && !result.isSuccess) {
        errorModal(result?.errors.join("\n"), 10000);
        return response;
    }

    return response;
}

function errorHandler(error: { response: { status: any; data: any; }; config: AxiosRequestConfig<any>; }): Promise<any> {

    const originalRequest = error.config;

    if (error.response.status === 401) {

        // add to queue to resolve after refresh token succeeded
        if (isRefreshing) {
            return new Promise(
                (resolve, reject) => {
                    failedQueue.push({ resolve, reject });
                })
                .then(result => {
                    return axios(originalRequest);
                })
                .catch(err => {
                    return Promise.reject(err);
                });
        }

        isRefreshing = true;

        // attempt to refresh token
        return new Promise((resolve, reject) => {
            AuthService.postApiAuthRefreshToken()
                .then(result => {
                    if (result?.isSuccess) {
                        // retry request if refresh token succeeded
                        processQueue(null);
                        resolve(axios(error.config));
                    }
                    else {
                        // logout if refresh token failed
                        redirectToLogout();
                        reject(error);
                    }
                })
                .catch(err => {
                    processQueue(err);
                    return reject(err);
                })
                .then(() => {
                    isRefreshing = false;
                });
        });
    }

    return Promise.reject(error);
}

function redirectToLogout() {
    window.location.replace(
        `${window.location.protocol}//${window.location.host}/logout`
    );
}

interface ApiResponse {
    data: any;
    errors: string[];
    isSuccess: boolean;
}

