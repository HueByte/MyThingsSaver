import { Redirect } from "react-router";
import { errorModal } from "../core/Modals";

export async function HandleBasicApiResponse(response) {
  if (
    response.status !== 400 &&
    response.status !== 200 &&
    response.status !== 401
  ) {
    errorModal("Something went wrong with server connection");
    throw new Error("Something went wrong with connection to server");
  }

  if (response.status === 401) {
    let message = response.headers.get("www-authenticate");
    if (message != null && message.includes("The token expired at")) {
      window.location.replace(
        `${window.location.protocol}//${window.location.host}/logout`
      );
    }
  }

  var result = await response.json();
  if (!result?.isSuccess) {
    errorModal(result?.errors.join(", "), 6000);
  }

  return result;
}
