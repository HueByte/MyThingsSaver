import { errorModal } from "../core/Modals";
import { SilentRefresh } from "../auth/Auth";

export const AuthFetch = (url, options = {}, isRetry = false) =>
  fetch(url, options)
    .then(async (response) => {
      if (response.status == 401) {
        let result = await HandleAuthorize(response, options, isRetry);

        // if JWT got refreshed make the request again with modified headers
        if (result) return AuthFetch(url, options, true);
      } else return HandleApiResponse(response);
    })
    .catch((err) => console.log(err.message));

export const HandleAuthorize = async (response, refOptions, isRetry) => {
  let message = response.headers.get("www-authenticate");
  if (message ?? message.includes("The token expired at")) {
    if (!isRetry) {
      // make silent refresh request
      let result = await SilentRefresh().then((response) => response.json());

      if (result.isSuccess) {
        // replcase old token with new one
        refOptions.headers.Authorization = `Bearer ${result.data.token}`;

        // update authState via event & set new user data in the storage
        localStorage.setItem("user", JSON.stringify(result.data));
        window.dispatchEvent(new Event("refreshUser"));
        return true; // everything went correctly
      } else {
        // refresh token expired
        window.location.replace(
          `${window.location.protocol}//${window.location.host}/logout`
        );

        return false;
      }
    }
    // token exchange fail
    errorModal("token exchange fail, please refresh the app");

    return false;
  }

  errorModal("UnAuthorized", 6000);
  return false;
};

export const HandleApiResponse = async (response) => {
  switch (response.status) {
    // on success request
    case 200:
    case 400:
      var result = await response.json();
      if (!result?.isSuccess) {
        errorModal(result?.errors.join(" "), 6000);
      }
      return result;

    // on fail request
    default:
      errorModal("Something went wrong with server connection");
      throw new Error("Something went wrong with connection to server!");
  }
};

// 200 - Request success
// 400 - Api handled error
// 401 - Api handled unauthorized, token might be expired or token might be empty/invalid
// 404 - Handled by SPA routing
// default - Errors weren't handled by API and something went wrong
