export const BaseUrl = `${window.location.protocol}//${window.location.host}/`;

// api/auth
export const RegisterEndpoint = `${BaseUrl}api/Authenticate/register`;
export const LoginEndpoint = `${BaseUrl}api/authenticate/loginUsername`;

// api/category
export const CategoryAddEndpoint = `${BaseUrl}api/Category/AddCategory`;
export const CategoryRemoveEndpoint = `${BaseUrl}api/Category/RemoveCategory`;
export const CategoryGetAllEndpoint = `${BaseUrl}api/Category/GetAllCategories`;
export const CategoryGetEndpoint = `${BaseUrl}api/Category/RemoveCategory`;
export const CategoryUpdateEndpoint = `${BaseUrl}api/Category/UpdateCategory`;
export const CategoryGetWithEntriesEndpoint = `${BaseUrl}api/Category/GetCategoryWithEntries`;

// api/CategoryEntry
export const AddOneEntryEndpoint = `${BaseUrl}api/CategoryEntry/AddEntry`;
export const GetEntryByNameEndpoint = `${BaseUrl}api/CategoryEntry/GetEntryByName`;
export const GetEntryByIdEndpoint = `${BaseUrl}api/CategoryEntry/GetEntryById`;
export const GetAllEntriesEndpoint = `${BaseUrl}api/CategoryEntry/GetAllEntries`;
export const UpdateEntryEndpoint = `${BaseUrl}api/CategoryEntry/UpdateEntry`;
export const DeleteEntryEndpoint = `${BaseUrl}api/CategoryEntry/DeleteEntry`;
export const GetRecentEntriesEndpoint = `${BaseUrl}api/CategoryEntry/GetRecent`;
