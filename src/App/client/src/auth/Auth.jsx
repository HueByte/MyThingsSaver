import {
  LoginEndpoint,
  LogoutEndpoint,
  RegisterEndpoint,
  SilentLoginEndpoint,
} from "../api/ApiEndpoints";

export const AuthRegister = async (Email, Username, Password) => {
  let user = {
    email: Email,
    username: Username,
    password: Password,
  };

  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(user),
  };

  return await fetch(RegisterEndpoint, requestOptions).then((result) =>
    result.json()
  );
};

export const AuthLogin = async (Username, Password) => {
  let user = {
    username: Username,
    password: Password,
  };

  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(user),
  };

  return await fetch(LoginEndpoint, requestOptions).then((result) =>
    result.json()
  );
};

export const AuthLogout = async () => {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await fetch(LogoutEndpoint, requestOptions);
};

export const SilentRefresh = async () => {
  const requestOptions = {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
  };

  return await fetch(SilentLoginEndpoint, requestOptions);
};
