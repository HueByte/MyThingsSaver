import { CategoryAddEndpoint, CategoryARemoveEndpoint, CategoryGetAllEndpoint } from "../routes/ApiEndpoints"
import { HandleBasicApiResponse } from "./ApiErrors"

export async function AddCategory(token, Name) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ name: Name })
    }

    return await fetch(CategoryAddEndpoint, requestOptions)
        .then(HandleBasicApiResponse);
}

export async function GetAllCategories(token) {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    }

    return await fetch(CategoryGetAllEndpoint, requestOptions)
        .then(HandleBasicApiResponse);
}

export async function RemoveCategory(token, CategoryId) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ categoryId: CategoryId })
    }

    return await fetch(CategoryARemoveEndpoint, requestOptions)
        .then(HandleBasicApiResponse);
}