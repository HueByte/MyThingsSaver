import {
  EntriesAddEnpoint,
  EntriesDeleteEndpoint,
  EntriesGetAllEndpoint,
  EntryGetEndpoint,
  EntriesGetRecentEndpoint,
  EntriesUpdateEndpoint,
} from "../ApiEndpoints";
import { AuthFetch } from "../ApiHandler";

export async function GetAllEntries(token, categoryId, withContent = false) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await AuthFetch(
    `${EntriesGetAllEndpoint}?categoryId=${categoryId}&withContent=${withContent}`,
    requestOptions
  );
}

export async function GetOneEntry(token, entryId) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await AuthFetch(`${EntryGetEndpoint}/?id=${entryId}`, requestOptions);
}

export async function UpdateOneEntry(token, EntryId, Name, Content) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({
      entryId: EntryId,
      entryName: Name,
      content: Content,
    }),
  };

  return await AuthFetch(EntriesUpdateEndpoint, requestOptions);
}

export async function AddOneEntry(token, Name, CategoryId) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({
      entryName: Name,
      categoryId: CategoryId,
      content: "",
    }),
  };

  return await AuthFetch(EntriesAddEnpoint, requestOptions);
}

export async function DeleteOneEntry(token, id) {
  const requestOptions = {
    method: "DELETE",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await AuthFetch(`${EntriesDeleteEndpoint}?id=${id}`, requestOptions);
}

export async function GetRecentEntries(token) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await AuthFetch(EntriesGetRecentEndpoint, requestOptions);
}
