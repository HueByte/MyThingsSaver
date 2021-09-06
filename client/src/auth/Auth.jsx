import { HandleApiResponse } from "../api/ApiHandler";
import {
  LoginEndpoint,
  LogoutEndpoint,
  RegisterEndpoint,
  SilentLoginEndpoint,
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

  return await fetch(RegisterEndpoint, requestOptions).then(HandleApiResponse);
};

export const AuthLogin = async (Username, Password) => {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json " },
    body: JSON.stringify({ username: Username, password: Password }),
  };

  return await fetch(LoginEndpoint, requestOptions).then(HandleApiResponse);
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

export const SilentRefresh = async () => {
  const requestOptions = {
    method: "POST",
    headers: { "Content-Type": "application/json" },
  };

  return await fetch(SilentLoginEndpoint, requestOptions);
};
