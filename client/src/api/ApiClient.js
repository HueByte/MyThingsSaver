import { AuthFetch } from "./ApiHandler";

class ApiClient {
  static async Get(endpoint, params = null) {
    const requestOptons = {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authContext.authState?.token}`,
      },
    };

    let resultEndpoint = params ? ParamBuilder(endpoint, params) : endpoint;

    return await AuthFetch(resultEndpoint, requestOptons);
  }

  static async Post(endpoint, body) {
    const requestOptons = {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${authContext.authState?.token}`,
      },
      body: JSON.stringify(body),
    };

    return await AuthFetch(endpoint, requestOptons);
  }
}

export default ApClient;

const ParamBuilder = (endpoint, params) => {
  let result = endpoint;
  let isFirst = true;

  params?.foreach((item) => {
    if (isFirst) {
      result += `?${item.key}${item.value}`;
    } else {
      result += `&${item.key}${item.value}`;
    }
  });

  return result;
};
