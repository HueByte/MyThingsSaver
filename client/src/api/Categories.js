import {
  CategoryAddEndpoint,
  CategoryRemoveEndpoint,
  CategoryGetAllEndpoint,
  CategoryUpdateEndpoint,
  CategoryGetWithEntriesEndpoint,
} from "../routes/ApiEndpoints";
import { AuthFetch } from "./ApiHandler";

export async function AddCategory(token, Name) {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
    body: JSON.stringify({ name: Name }),
  };

  return await AuthFetch(CategoryAddEndpoint, requestOptions);
}

export async function GetAllCategories(token) {
  const requestOptions = {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await AuthFetch(CategoryGetAllEndpoint, requestOptions);
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

  return await AuthFetch(
    `${CategoryGetWithEntriesEndpoint}?categoryId=${CategoryId}`,
    requestOptions
  );
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

  return await AuthFetch(CategoryRemoveEndpoint, requestOptions);
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

  return await AuthFetch(CategoryUpdateEndpoint, requestOptions);
}
