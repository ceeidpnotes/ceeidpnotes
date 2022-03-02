import { graphConfig } from "./authConfig";

/**
 * Attaches a given access token to a Microsoft Graph API call. Returns information about the user
 */
export async function callMsGraph(accessToken) {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);

    const options = {
        method: "GET",
        headers: headers
    };

    return fetch(graphConfig.graphMeEndpoint, options)
        .then(response => response.json())
        .catch(error => console.log(error));
}

//Uncaught ServerError: invalid_client: AADSTS650052: The app needs access to a service ('api://d639132a-a25d-49ea-b5c3-2f8cb62e5615') that your organization '2feaa5b1-2722-4933-afca-4d14140d5ef0' has not subscribed to or enabled. Contact your IT Admin to review the configuration of your service subscriptions.

export async function callApiPermission01(accessToken) {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);

    const options = {
        method: "GET",
        headers: headers
    };

    return fetch('https://localhost:7103/api/ApiPermission01', options)
        .then(
            response => response.text()
            )
        .catch(
            error => console.log(error)
            );
}

export async function callApiPermission02WebApi01(accessToken) {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);

    const options = {
        method: "GET",
        headers: headers
    };

    return fetch('https://localhost:7103/api/ApiPermission02/WebApi01', options)
        .then(
            response => response.text()
            )
        .catch(
            error => console.log(error)
            );
}

export async function callApiPermission02WebApi02(accessToken) {
    const headers = new Headers();
    const bearer = `Bearer ${accessToken}`;

    headers.append("Authorization", bearer);

    const options = {
        method: "GET",
        headers: headers
    };

    return fetch('https://localhost:7103/api/ApiPermission02/WebApi02', options)
        .then(
            response => response.text()
            )
        .catch(
            error => console.log(error)
            );
}