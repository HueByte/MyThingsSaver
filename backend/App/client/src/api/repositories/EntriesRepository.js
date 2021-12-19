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
