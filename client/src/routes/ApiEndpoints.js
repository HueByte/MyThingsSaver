export const BaseUrl = false ? 'https://localhost:5001/' : '';

export const RegisterEndpoint = `${BaseUrl}api/Authenticate/register`;
export const LoginEndpoint = `${BaseUrl}api/authenticate/loginUsername`;

export const CategoryAddEndpoint = `${BaseUrl}api/Category/AddCategory`;
export const CategoryRemoveEndpoint = `${BaseUrl}api/Category/RemoveCategory`;
export const CategoryGetAllEndpoint = `${BaseUrl}api/Category/GetAllCategories`;
export const CategoryGetEndpoint = `${BaseUrl}api/Category/RemoveCategory`;
export const CategoryUpdateEndpoint = `${BaseUrl}api/Category/UpdateCategory`;
export const CategoryGetWithEntries = `${BaseUrl}api/Category/GetCategoryWithEntries`;

export const AddEntryEndpoint = `${BaseUrl}api/CategoryEntry/AddEntry`;
export const AddOneEntryEndpoint = `${BaseUrl}api/CategoryEntry/AddEntry`
export const GetEntryByNameEndpoint = `${BaseUrl}api/CategoryEntry/GetEntryByName`;
export const GetEntryByIdEndpoint = `${BaseUrl}api/CategoryEntry/GetEntryById`;
export const GetAllEntriesEndpoint = `${BaseUrl}api/CategoryEntry/GetAllEntries`;
export const UpdateEntryEndpoint = `${BaseUrl}api/CategoryEntry/UpdateEntry`;
export const DeleteEntryEndpoint = `${BaseUrl}api/CategoryEntry/DeleteEntry`;