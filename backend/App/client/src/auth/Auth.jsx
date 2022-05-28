import {
  LoginEndpoint,
  LogoutEndpoint,
  RegisterEndpoint,
  SilentLoginEndpoint,
} from "../api/ApiEndpoints";
import ApiClient from "../api/ApiClient";

export const AuthRegister = async (Email, Username, Password) => {
  let user = {
    email: Email,
    username: Username,
    password: Password,
  };

  let body = JSON.stringify(user);

  return await ApiClient.Post(RegisterEndpoint, body);
};

export const AuthLogin = async (Username, Password) => {
  let user = {
    username: Username,
    password: Password,
  };

  let body = JSON.stringify(user);

  return await ApiClient.Post(LoginEndpoint, body);
};

export const AuthLogout = async () => {
  return await ApiClient.Post(LogoutEndpoint);
};

export const SilentRefresh = async () => {
  return await ApiClient.Post(SilentLoginEndpoint);
};
