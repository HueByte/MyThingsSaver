export const BaseUrl = `${window.location.protocol}//${window.location.host}/`;
export const ApiEndpoint = `${BaseUrl}api`;

// api/Authenticate
export const RegisterEndpoint = `api/Authenticate/register`;
export const LoginEndpoint = `api/Authenticate/loginUsername`;
export const LogoutEndpoint = `api/Authenticate/logout`;
export const SilentLoginEndpoint = `api/Authenticate/refresh-token`;

// api/Category
export const CategoryGetAllEndpoint = `api/Category/GetAll`;
export const CategoryGetEndpoint = `api/Category/Get`;
export const CategoryAddEndpoint = `api/Category/Add`;
export const CategoryRemoveEndpoint = `api/Category/Remove`;
export const CategoryUpdateEndpoint = `api/Category/Update`;
export const CategoryGetWithEntriesEndpoint = `api/Category/GetWithEntries`;

// api/CategoryEntry
export const EntriesGetEndpoint = `api/CategoryEntry/Get`;
export const EntriesGetAllEndpoint = `api/CategoryEntry/GetAll`;
export const EntriesAddEnpoint = `api/CategoryEntry/Add`;
export const EntriesUpdateEndpoint = `api/CategoryEntry/Update`;
export const EntriesDeleteEndpoint = `api/CategoryEntry/Delete`;
export const EntriesGetRecentEndpoint = `api/CategoryEntry/GetRecent`;
