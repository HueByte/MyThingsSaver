import { AuthFetch } from "./ApiHandler";

class ApiClient {
  static async Get(token, endpoint, params = null) {
    const requestOptons = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    };

    let resultEndpoint = params ? ParamBuilder(endpoint, ...params) : endpoint;

    return await AuthFetch(resultEndpoint, requestOptons);
  }

  static async Post(token, endpoint, body) {
    const requestOptons = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: body,
    };

    return await AuthFetch(endpoint, requestOptons);
  }

  static async Delete(token, endpoint, params = null) {
    const requestOptons = {
      method: "DELETE",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    };

    let resultEndpoint = params ? ParamBuilder(endpoint, ...params) : endpoint;

    return await AuthFetch(resultEndpoint, requestOptons);
  }
}

export default ApiClient;

const ParamBuilder = (endpoint, ...params) => {
  if (params.length == 1)
    return `${endpoint}?${params[0].key}=${params[0].value}`;

  let result = endpoint;
  let isFirst = true;

  for (const item of params) {
    if (isFirst) {
      result += `?${item.key}=${item.value}`;
      isFirst = false;
    } else {
      result += `&${item.key}=${item.value}`;
    }
  }

  return result;
};
