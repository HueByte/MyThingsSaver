export function HandleBasicApiResponse(response) {
    if (!response.status === 400 || !response.status === 200) {
        throw new Error('Something went wrong with connection to server');
    }

    return response.json();
}
