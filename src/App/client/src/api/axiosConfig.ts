import axios, { AxiosRequestConfig } from 'axios'
import { errorModal } from '../core/Modals';
import { AuthService } from './services/AuthService';

const axiosApiInstance = axios.create();

axios.interceptors.response.use(
    (Request) => Request,
    (Error) => errorHandler(Error)
)

async function errorHandler(error: { response: { status: any; data: any; }; config: AxiosRequestConfig<any>; }): Promise<any> {
    if (axios.isAxiosError(error)) {
        const status = error.response?.status;

        if (status == 401) {
            let silentResult = await AuthService.postApiAuthRefreshToken();

            if (silentResult) {
                return await axios.request(error.config); // retry
            }

            // logout
            redirectToLogout();
            return;
        } else if (status == 400) {
            let result: ApiResponse = error.response?.data;

            if (result.errors.length > 0 && result.errors.find(err => err === "Token is invalid")) {
                redirectToLogout();
                return;
            }

            errorModal(result.errors.join("\n"), 10000);
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