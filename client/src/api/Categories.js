import {
  CategoryAddEndpoint,
  CategoryRemoveEndpoint,
  CategoryGetAllEndpoint,
  CategoryUpdateEndpoint,
  CategoryGetWithEntriesEndpoint,
} from "../routes/ApiEndpoints";
import { HandleBasicApiResponse } from "./ApiErrors";

export async function AddCategory(token, Name) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ name: Name }),
  };

  return await fetch(CategoryAddEndpoint, requestOptions).then(
    HandleBasicApiResponse
  );
}

export async function GetAllCategories(token) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await fetch(CategoryGetAllEndpoint, requestOptions).then(
    HandleBasicApiResponse
  );
}

export async function GetCategoryWithEntries(
  token,
  CategoryId,
  withContent = false
) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await fetch(
    `${CategoryGetWithEntriesEndpoint}?categoryId=${CategoryId}`,
    requestOptions
  ).then(HandleBasicApiResponse);
}

export async function RemoveCategory(token, CategoryId) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ categoryId: CategoryId }),
  };

  return await fetch(CategoryRemoveEndpoint, requestOptions).then(
    HandleBasicApiResponse
  );
}

export async function UpdateCategory(token, CategoryId, Name) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ categoryId: CategoryId, name: Name }),
  };

  return await fetch(CategoryUpdateEndpoint, requestOptions).then(
    HandleBasicApiResponse
  );
}
