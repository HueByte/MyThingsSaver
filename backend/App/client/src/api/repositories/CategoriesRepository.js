import {
  CategoryAddEndpoint,
  CategoryRemoveEndpoint,
  CategoryGetAllEndpoint,
  CategoryUpdateEndpoint,
  CategoryGetWithEntriesEndpoint,
} from "../ApiEndpoints";
import ApiClient from "../ApiClient";
import { AuthFetch } from "../ApiHandler";

class CategoriesRepository {
  static async Add(token, name, parentID = null) {
    let body = JSON.stringify({ name: name, categoryParentId: parentID });
    return await ApiClient.Post(token, CategoryAddEndpoint, body);
  }

  static async GetAll(token) {
    return await ApiClient.Get(token, CategoryGetAllEndpoint);
  }

  static async GetWithEntries(token, categoryID, withContent = false) {
    let params = [{ key: "CategoryID", value: categoryID }];
    return await ApiClient.Get(token, CategoryGetWithEntriesEndpoint, params);
  }

  static async Remove(token, categoryID) {
    let body = JSON.stringify({ categoryId: categoryID });
    return await ApiClient.Post(token, CategoryRemoveEndpoint, body);
  }

  static async Update(token, categoryID, name) {
    let body = JSON.stringify({ categoryId: categoryID, name: name });
    return await ApiClient.Post(token, CategoryUpdateEndpoint, body);
  }
}

export default CategoriesRepository;
