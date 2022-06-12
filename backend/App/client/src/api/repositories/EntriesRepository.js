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
import HttpClient from "../HttpClient.ts";

class EntriesRepository {
  static async Get(entryID) {
    return await HttpClient.client.get(EntriesGetEndpoint, {
      params: {
        id: entryID,
      },
    });
  }

  static async GetAll(categoryID, withContent = false) {
    return await HttpClient.client.get(EntriesGetAllEndpoint, {
      params: {
        CategoryId: categoryID,
        withContent: withContent,
      },
    });
  }

  static async GetRecent() {
    return await HttpClient.client.get(EntriesGetRecentEndpoint);
  }

  static async Update(entryID, name, content) {
    let body = {
      entryId: entryID,
      entryName: name,
      content: content,
    };

    return await HttpClient.client.post(EntriesUpdateEndpoint, body);
  }

  static async UpdateWithoutContent(entryID, name, categoryID) {
    let body = {
      entryId: entryID,
      entryName: name,
      categoryId: categoryID,
    };

    return await HttpClient.client.post(
      EntriesUpdateWithoutContentEndpoint,
      body
    );
  }

  static async Add(name, categoryID) {
    let body = {
      entryName: name,
      categoryId: categoryID,
      content: "",
    };

    return await HttpClient.client.post(EntriesAddEnpoint, body);
  }

  static async Delete(entryID) {
    return await HttpClient.client.delete(EntriesDeleteEndpoint, {
      params: {
        id: entryID,
      },
    });
  }
}

export default EntriesRepository;
