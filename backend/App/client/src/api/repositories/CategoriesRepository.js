import {
  CategoryAddEndpoint,
  CategoryRemoveEndpoint,
  CategoryGetAllEndpoint,
  CategoryUpdateEndpoint,
  CategoryGetWithEntriesEndpoint,
  CategoryGetAllRootEndpoint,
  CategoryGetAllSubEndpoint,
} from "../ApiEndpoints";
import ApiClient from "../ApiClient";
import { AuthFetch } from "../ApiHandler";

class CategoriesRepository {
  static async Add(name, parentID = null) {
    let body = JSON.stringify({ name: name, categoryParentId: parentID });
    return await ApiClient.Post(CategoryAddEndpoint, body);
  }

  static async GetAll() {
    return await ApiClient.Get(CategoryGetAllEndpoint);
  }

  static async GetRoot() {
    return await ApiClient.Get(CategoryGetAllRootEndpoint);
  }

  static async GetSub(parentID) {
    let params = [{ key: "parentId", value: parentID }];
    return await ApiClient.Get(CategoryGetAllSubEndpoint, params);
  }

  static async GetWithEntries(categoryID, withContent = false) {
    let params = [{ key: "CategoryID", value: categoryID }];
    return await ApiClient.Get(CategoryGetWithEntriesEndpoint, params);
  }

  static async Remove(categoryID) {
    let body = JSON.stringify({ categoryId: categoryID });
    return await ApiClient.Post(CategoryRemoveEndpoint, body);
  }

  static async Update(categoryID, name) {
    let body = JSON.stringify({ categoryId: categoryID, name: name });
    return await ApiClient.Post(CategoryUpdateEndpoint, body);
  }
}

export default CategoriesRepository;
