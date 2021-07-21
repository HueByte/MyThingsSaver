import { CategoryAddEndpoint } from "../routes/ApiEndpoints"
import { HandleBasicApiResponse } from "./ApiErrors"

export async function AddCategory(Name) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: Name })
    }

    return await fetch(CategoryAddEndpoint, requestOptions)
        .then(HandleBasicApiResponse);
}

export async function RemoveCategory() {

}