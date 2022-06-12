export interface IApiResponse<T> {
    Data?: T | any,
    Errors: string[],
    IsSuccess: Boolean
}