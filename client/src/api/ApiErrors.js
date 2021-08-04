import { errorModal } from "../core/Modals";

export async function HandleBasicApiResponse(response) {
    if (response.status !== 400 && response.status !== 200) {
        errorModal('Something went wrong with server connection');
        throw new Error('Something went wrong with connection to server');
    }

    var result = await response.json();
    if (!result?.isSuccess) {
        errorModal(result?.errors.join(', '), 10000);
        // throw new Error(result?.errors.join(', '));
    }

    return result;
}
