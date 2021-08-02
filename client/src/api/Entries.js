import { GetAllEntriesEndpoint, GetEntryByIdEndpoint } from "../routes/ApiEndpoints";
import { HandleBasicApiResponse } from "./ApiErrors"

export async function GetAllEntries(token, categoryId) {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    }

    return await fetch(`${GetAllEntriesEndpoint}?categoryId=${categoryId}`, requestOptions)
        .then(HandleBasicApiResponse);
}

export async function GetOneEntry(token, entryId) {
    const requestOptions = {
        method: 'GET',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` }
    }

    return await fetch(`${GetEntryByIdEndpoint}/?id=${entryId}`, requestOptions)
        .then(HandleBasicApiResponse);
}