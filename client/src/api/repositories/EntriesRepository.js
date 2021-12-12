import {
  EntriesAddEnpoint,
  EntriesDeleteEndpoint,
  EntriesGetAllEndpoint,
  EntriesGetEndpoint,
  EntriesGetRecentEndpoint,
  EntriesUpdateEndpoint,
} from "../ApiEndpoints";
import ApiClient from "../ApiClient";
import { AuthFetch } from "../ApiHandler";

class EntriesRepository {
  static async Get(token, entryID) {
    let params = [{ key: "id", value: entryID }];
    return await ApiClient.Get(token, EntriesGetEndpoint, params);
  }

  static async GetAll(token, categoryID, withContent = false) {
    let params = [
      { key: "CategoryId", value: categoryID },
      { key: "withContent", value: withContent },
    ];
    return await ApiClient.Get(token, EntriesGetAllEndpoint, params);
  }

  static async GetRecent(token) {
    return await ApiClient.Get(token, EntriesGetRecentEndpoint);
  }

  static async Update(token, entryID, name, content) {
    let body = JSON.stringify({
      entryId: entryID,
      entryName: name,
      content: content,
    });
    return await ApiClient.Post(token, EntriesUpdateEndpoint, body);
  }

  static async Add(token, name, categoryID) {
    let body = JSON.stringify({
      entryName: name,
      categoryId: categoryID,
      content: "",
    });
    return await ApiClient.Post(token, EntriesAddEnpoint, body);
  }

  static async Delete(token, entryID) {
    let params = [{ key: "id", value: entryID }];
    return await ApiClient.Delete(token, EntriesDeleteEndpoint, params);
  }
}

export default EntriesRepository;

// export async function GetAllEntries(token, categoryId, withContent = false) {
//   const requestOptions = {
//     method: "GET",
//     headers: {
//       "Content-Type": "application/json",
//       Authorization: `Bearer ${token}`,
//     },
//   };

//   return await AuthFetch(
//     `${EntriesGetAllEndpoint}?categoryId=${categoryId}&withContent=${withContent}`,
//     requestOptions
//   );
// }

// export async function GetOneEntry(token, entryId) {
//   const requestOptions = {
//     method: "GET",
//     headers: {
//       "Content-Type": "application/json",
//       Authorization: `Bearer ${token}`,
//     },
//   };

//   return await AuthFetch(
//     `${EntriesGetEndpoint}/?id=${entryId}`,
//     requestOptions
//   );
// }

// export async function UpdateOneEntry(token, EntryId, Name, Content) {
//   const requestOptions = {
//     method: "POST",
//     headers: {
//       "Content-Type": "application/json",
//       Authorization: `Bearer ${token}`,
//     },
//     body: JSON.stringify({
//       entryId: EntryId,
//       entryName: Name,
//       content: Content,
//     }),
//   };

//   return await AuthFetch(EntriesUpdateEndpoint, requestOptions);
// }

// export async function AddOneEntry(token, Name, CategoryId) {
//   const requestOptions = {
//     method: "POST",
//     headers: {
//       "Content-Type": "application/json",
//       Authorization: `Bearer ${token}`,
//     },
//     body: JSON.stringify({
//       entryName: Name,
//       categoryId: CategoryId,
//       content: "",
//     }),
//   };

//   return await AuthFetch(EntriesAddEnpoint, requestOptions);
// }

// export async function DeleteOneEntry(token, id) {
//   const requestOptions = {
//     method: "DELETE",
//     headers: {
//       "Content-Type": "application/json",
//       Authorization: `Bearer ${token}`,
//     },
//   };

//   return await AuthFetch(`${EntriesDeleteEndpoint}?id=${id}`, requestOptions);
// }

// export async function GetRecentEntries(token) {
//   const requestOptions = {
//     method: "GET",
//     headers: {
//       "Content-Type": "application/json",
//       Authorization: `Bearer ${token}`,
//     },
//   };

//   return await AuthFetch(EntriesGetRecentEndpoint, requestOptions);
// }
