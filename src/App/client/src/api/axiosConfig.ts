import axios, { AxiosRequestConfig, AxiosResponse } from 'axios'
import { responseInterceptor } from 'http-proxy-middleware';
import { errorModal } from '../core/Modals';
import { AuthService } from './services/AuthService';

const axiosApiInstance = axios.create();
axios.interceptors.response.use(
    (Response) => responseHandler(Response),
    (Error) => errorHandler(Error)
)

function responseHandler(response: AxiosResponse) {
    let result: ApiResponse = response.data;

    if (response.status == 200 && !result.isSuccess) {
        errorModal(result?.errors.join("\n"), 10000);
        return response;
    }

    return response;
}

function errorHandler(error: { response: { status: any; data: any; }; config: AxiosRequestConfig<any>; }): Promise<any> {
    if (axios.isAxiosError(error)) {
        const status = error.response?.status;

        if (status == 401) {
            let result: ApiResponse = error.response.data;

            if (result.errors?.length > 0) {

                redirectToLogout();
                return Promise.resolve(error.response.data);
            }

            return AuthService.postApiAuthRefreshToken().then((result) => {
                // retry request if refresh token succeeded
                if (result?.isSuccess) return axios.request(error.config);

                // logout if refresh token failed
                redirectToLogout();
                return;
            });
        }
        else if (status == 400) {
            let result: ApiResponse = error.response?.data;

            if (result) {
                errorModal(result.errors.join("\n"), 10000);
                return Promise.resolve(error.response.data);
            }
        }
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