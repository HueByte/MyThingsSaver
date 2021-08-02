import { AddOneEntryEndpoint, GetAllEntriesEndpoint, GetEntryByIdEndpoint, UpdateEntryEndpoint } from "../routes/ApiEndpoints";
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

export async function UpdateOneEntry(token, EntryId, Name, Content) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ entryId: EntryId, entryName: Name, content: Content })
    }

    return await fetch(UpdateEntryEndpoint, requestOptions)
        .then(HandleBasicApiResponse);
}

export async function AddOneEntry(token, Name, CategoryId) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', 'Authorization': `Bearer ${token}` },
        body: JSON.stringify({ entryName: Name, categoryId: CategoryId, content: '', })
    }

    return await fetch(AddOneEntryEndpoint, requestOptions)
        .then(HandleBasicApiResponse);
}