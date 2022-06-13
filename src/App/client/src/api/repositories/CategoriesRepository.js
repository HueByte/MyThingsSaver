import {
  CategoryAddEndpoint,
  CategoryRemoveEndpoint,
  CategoryGetAllEndpoint,
  CategoryUpdateEndpoint,
  CategoryGetWithEntriesEndpoint,
  CategoryGetAllRootEndpoint,
  CategoryGetAllSubEndpoint,
} from "../ApiEndpoints";
import HttpClient from "../HttpClient.ts";

class CategoriesRepository {
  static async Add(name, parentID = null) {
    let body = { name: name, categoryParentId: parentID };

    return await HttpClient.client.post(CategoryAddEndpoint, body);
  }

  static async GetAll() {
    return await HttpClient.client.get(CategoryGetAllEndpoint);
  }

  static async GetRoot() {
    return await HttpClient.client.get(CategoryGetAllRootEndpoint);
  }

  static async GetSub(parentID) {
    return await HttpClient.client.get(CategoryGetAllSubEndpoint, {
      params: {
        parentId: parentID,
      },
    });
  }

  static async GetWithEntries(categoryID, withContent = false) {
    return await HttpClient.client.get(CategoryGetWithEntriesEndpoint, {
      params: {
        CategoryID: categoryID,
        WithContent: withContent,
      },
    });
  }

  static async Remove(categoryID) {
    let body = { categoryId: categoryID };

    return await HttpClient.client.post(CategoryRemoveEndpoint, body);
  }

  static async Update(categoryId, name) {
    let body = { categoryId: categoryId, name: name };

    return await HttpClient.client.post(CategoryUpdateEndpoint, body);
  }
}

export default CategoriesRepository;
