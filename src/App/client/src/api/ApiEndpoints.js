export const BaseUrl = `${window.location.protocol}//${window.location.host}`; // {http | https}//{Ip}
export const ApiEndpoint = `${BaseUrl}/api`;

// api/Authenticate
export const RegisterEndpoint = `${ApiEndpoint}/Authenticate/register`;
export const LoginEndpoint = `${ApiEndpoint}/Authenticate/login`;
export const LogoutEndpoint = `${ApiEndpoint}/Authenticate/logout`;
export const SilentLoginEndpoint = `${ApiEndpoint}/Authenticate/refresh-token`;

// api/Category
export const CategoryGetAllEndpoint = `${ApiEndpoint}/Category/GetAll`;
export const CategoryGetAllRootEndpoint = `${ApiEndpoint}/Category/GetAllRoot`;
export const CategoryGetAllSubEndpoint = `${ApiEndpoint}/Category/GetAllSub`;
export const CategoryGetEndpoint = `${ApiEndpoint}/Category/Get`;
export const CategoryAddEndpoint = `${ApiEndpoint}/Category/Add`;
export const CategoryRemoveEndpoint = `${ApiEndpoint}/Category/Remove`;
export const CategoryUpdateEndpoint = `${ApiEndpoint}/Category/Update`;
export const CategoryGetWithEntriesEndpoint = `${ApiEndpoint}/Category/GetWithEntries`;

// api/CategoryEntry
export const EntriesGetEndpoint = `${ApiEndpoint}/CategoryEntry/Get`;
export const EntriesGetAllEndpoint = `${ApiEndpoint}/CategoryEntry/GetAll`;
export const EntriesAddEnpoint = `${ApiEndpoint}/CategoryEntry/Add`;
export const EntriesUpdateEndpoint = `${ApiEndpoint}/CategoryEntry/Update`;
export const EntriesUpdateWithoutContentEndpoint = `${ApiEndpoint}/CategoryEntry/UpdateWithoutContent`;
export const EntriesDeleteEndpoint = `${ApiEndpoint}/CategoryEntry/Delete`;
export const EntriesGetRecentEndpoint = `${ApiEndpoint}/CategoryEntry/GetRecent`;
