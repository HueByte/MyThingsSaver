import { errorModal } from "../core/Modals";
import { SilentRefresh } from "../auth/Auth";

// TODO remove
// 200 - Request success
// 400 - Api handled error
// 401 - Api handled unauthorized, token might be expired or token might be empty/invalid
// 404 - Handled by SPA routing
// default - Errors weren't handled by API and something went wrong

export const AuthFetch = async (endpoint, options = {}, isRetry = false) => {
  var response = await fetch(endpoint, options);
  if (response.status == 401) {
    let authorizeResult = await HandleApiAuthorize(response, options, isRetry);

    if (authorizeResult) return await AuthFetch(endpoint, options, true);
  } else {
    return await HandleApiResponse(response);
  }
};

export const HandleApiResponse = async (response) => {
  switch (response.status) {
    // Request handled correctly
    case 200:
    case 400:
      let result = await response.json();
      if (!result?.isSuccess) {
        errorModal(result?.errors.join(" "), 6000);
      }

      return result;

    // something went wrong at the server side (e.g 500)
    default:
      errorModal("Something went wrong with server connection");
      throw new Error("Something went wrong with server connection");
  }
};

export const HandleApiAuthorize = async (response, refOptions, isRetry) => {
  let message = response.headers.get("www-authenticate");
  if (message ?? message.includes("The token expired")) {
    if (!isRetry) {
      let result = await SilentRefresh().then((response) => response.json());

      // successfully refreshed token
      if (result.isSuccess) {
        refOptions.headers.Authorization = `Bearer ${result.data.token}`;
        localStorage.setItem("user", JSON.stringify(result.data));
        window.dispatchEvent(new Event("refreshUser"));

        return true;
      }

      // silent token was expired
      else {
        window.location.replace(
          `${window.location.protocol}//${window.location.host}/logout`
        );

        return false;
      }
    }

    // silent token was expired and user wasn't redirected to logout
    errorModal("token exchange fail, please refresh the app");
    return false;
  }

  // unauthorized
  errorModal("Unauthorized", 6000);
  return false;
};
