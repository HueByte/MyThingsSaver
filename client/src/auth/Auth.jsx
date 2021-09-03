import { HandleBasicApiResponse } from "../api/ApiErrors";
import {
  LoginEndpoint,
  LogoutEndpoint,
  RegisterEndpoint,
} from "../routes/ApiEndpoints";

export const AuthRegister = async (Email, Username, Password) => {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      email: Email,
      username: Username,
      password: Password,
    }),
  };

  return await fetch(RegisterEndpoint, requestOptions).then(
    HandleBasicApiResponse
  );
};

export const AuthLogin = async (Username, Password) => {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json " },
    body: JSON.stringify({ username: Username, password: Password }),
  };

  return await fetch(LoginEndpoint, requestOptions).then(
    HandleBasicApiResponse
  );
};

export const AuthLogout = async (token) => {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  };

  return await fetch(LogoutEndpoint, requestOptions);
};
