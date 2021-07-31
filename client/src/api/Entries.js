import { HandleBasicApiResponse } from "./ApiErrors"

export async function AddEntry(token, categoryId, cate) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({})
    }

    return await fetch().then(HandleBasicApiResponse);
}