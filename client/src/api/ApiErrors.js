import { Redirect } from "react-router";
import { errorModal } from "../core/Modals";

export async function HandleBasicApiResponse(response) {
  switch (response.status) {
    case 400:
    case 200:
      var result = await response.json();
      if (!result?.isSuccess) {
        errorModal(result?.errors.join(" "), 6000);
        // throw new Error(result?.errors);
      }

      return result;

    case 401:
      let message = response.headers.get("www-authenticate");
      if (message ?? message.includes("The token expired at"))
        window.location.replace(
          `${window.location.protocol}//${window.location.host}/logout`
        );
      else errorModal("Unauthorized", 6000);
      break;

    default:
      errorModal("Something went wrong with server connection");
      throw new Error("Something went wrong with connection to server!");
  }
}
