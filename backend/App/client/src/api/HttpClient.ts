import axios, { AxiosInstance, AxiosResponse } from "axios";
import { SilentRefresh } from "../auth/Auth";
import { errorModal } from "../core/Modals";
import { ApiEndpoint } from "./ApiEndpoints";
import { IApiResponse } from "./models/IApiResponse";

class HttpClient {
    client: AxiosInstance = axios.create({
        baseURL: ApiEndpoint,
        timeout: 10000,
        headers: {
            Accept: "application/json",
        }
    })

    constructor() {
        this.client.interceptors.request.use(
            (Request) => this.requestHandler(Request),
            (Error: Response) => this.errorHandler(Error)
        )

        this.client.interceptors.response.use(
            (Response: AxiosResponse) => this.responseHandler(Response),
            (Error: Response) => this.errorHandler(Error)
        );
    }

    private async requestHandler(request) {
        return request;
    }

    private async responseHandler(response: AxiosResponse): Promise<IApiResponse<any>> {
        return await response.data;
    }

    private async errorHandler(error): Promise<IApiResponse<any>> {
        console.log('hti')
        if (axios.isAxiosError(error)) {
            const { status } = error.response;

            if (status == 401) {
                let silentResponse = await SilentRefresh();
                let silentResult: any = await silentResponse.json();

                if (silentResult.isSuccess) {
                    return await this.client.request(error.config); // retry
                }

                // logout
                window.location.replace(
                    `${window.location.protocol}//${window.location.host}/logout`
                );
            } else if (status == 400) {
                console.log('q');
                let result: any = error.response.data;
                errorModal(result.errors.join("\n"), 10000);
            }
        }

        return Promise.reject(error);
    }
}

export default new HttpClient();