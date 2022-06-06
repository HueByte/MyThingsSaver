import {
  EntriesAddEnpoint,
  EntriesDeleteEndpoint,
  EntriesGetAllEndpoint,
  EntriesGetEndpoint,
  EntriesGetRecentEndpoint,
  EntriesUpdateEndpoint,
  EntriesUpdateWithoutContentEndpoint,
} from "../ApiEndpoints";
import ApiClient from "../ApiClient";

class EntriesRepository {
  static async Get(entryID) {
    let params = [{ key: "id", value: entryID }];

    return await ApiClient.Get(EntriesGetEndpoint, params);
  }

  static async GetAll(categoryID, withContent = false) {
    let params = [
      { key: "CategoryId", value: categoryID },
      { key: "withContent", value: withContent },
    ];

    return await ApiClient.Get(EntriesGetAllEndpoint, params);
  }

  static async GetRecent() {
    console.log(ApiClient);
    return await ApiClient.Get(EntriesGetRecentEndpoint);
  }

  static async Update(entryID, name, content) {
    let body = {
      entryId: entryID,
      entryName: name,
      content: content,
    };

    return await ApiClient.Post(EntriesUpdateEndpoint, body);
  }

  static async UpdateWithoutContent(entryID, name, categoryID) {
    let body = {
      entryId: entryID,
      entryName: name,
      categoryId: categoryID,
    };

    return await ApiClient.Post(EntriesUpdateWithoutContentEndpoint, body);
  }

  static async Add(name, categoryID) {
    let body = {
      entryName: name,
      categoryId: categoryID,
      content: "",
    };

    return await ApiClient.Post(EntriesAddEnpoint, body);
  }

  static async Delete(entryID) {
    let params = [{ key: "id", value: entryID }];

    return await ApiClient.Delete(EntriesDeleteEndpoint, params);
  }
}

export default EntriesRepository;
