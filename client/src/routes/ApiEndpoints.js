export const BaseUrl = `${window.location.protocol}//${window.location.host}/`;
export const ApiEndpoint = `${BaseUrl}api`;

// api/auth
export const RegisterEndpoint = `${ApiEndpoint}/Authenticate/register`;
export const LoginEndpoint = `${ApiEndpoint}/Authenticate/loginUsername`;
export const LogoutEndpoint = `${ApiEndpoint}/Authenticate/logout`;
export const SilentLoginEndpoint = `${ApiEndpoint}/Authenticate/refresh-token`;

// api/category
export const CategoryGetAllEndpoint = `${ApiEndpoint}/Category/GetAll`;
export const CategoryGetEndpoint = `${ApiEndpoint}/Category/Get`;
export const CategoryAddEndpoint = `${ApiEndpoint}/Category/Add`;
export const CategoryRemoveEndpoint = `${ApiEndpoint}/Category/Remove`;
export const CategoryUpdateEndpoint = `${ApiEndpoint}/Category/Update`;
export const CategoryGetWithEntriesEndpoint = `${ApiEndpoint}/Category/GetWithEntries`;

// api/CategoryEntry
export const GetEntryByIdEndpoint = `${ApiEndpoint}/CategoryEntry/Get`;
export const GetAllEntriesEndpoint = `${ApiEndpoint}/CategoryEntry/GetAll`;
export const AddOneEntryEndpoint = `${ApiEndpoint}/CategoryEntry/Add`;
export const UpdateEntryEndpoint = `${ApiEndpoint}/CategoryEntry/Update`;
export const DeleteEntryEndpoint = `${ApiEndpoint}/CategoryEntry/Delete`;
export const GetRecentEntriesEndpoint = `${ApiEndpoint}/CategoryEntry/GetRecent`;
