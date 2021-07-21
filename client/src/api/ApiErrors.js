export async function HandleBasicApiResponse(response) {
    if (!response.status === 400 || !response.status === 200) {
        throw new Error('Something went wrong with connection to server');
    }

    var result = await response.json();
    if (!result?.isSuccess) {
        throw new Error(result?.errors.join(', '));
    }

    return result
}
