# Hub.Shell BFF

A backend for frontend service for the new React Hub.Shell. 

## Table of Contents

1. [Prerequisites](#Prerequisites)
2. [Development](#Development)
3. [Docker](#Docker)

## Prerequisites

- [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) (if on Mac or using VS Code)

## Development

### Windows
- On Windows, the best development experience comes from using Visual Studio.

### Mac (and Windows)
- You are able to use Visual Studio Code and extensions to develop in the BFF.
  - Ensure you are logged in with the Azure CLI via `az login` in order to retrieve secrets from Key Vault
- You can use the .NET CLI to run the application.
  - `dotnet run --project hub.shell.api` from the `/api` directory.
- There are also extensions that you could utilize to run the API from within VS Code using Docker or .NET CLI.

## Docker

This project is deployed to our Kubernetes cluster in Azure running Linux VMs. If you wish to test the container locally, you'll need Docker installed. Note that this is entirely optional and you should really only ever need to do this when making changes to the `Dockerfile` or ensuring compatibility of specific system calls or libraries.

The pods have access to Key Vault via User-Assigned Managed Identity. When dev'ing locally in Visual Studio or with `dotnet run`, we have access to Key Vault via Azure AD. When running the application container, it can't use either of these methods.

In order to run the container locally, we must provide it an access token manually so that it can authenticate with Key Vault temporarily:

1. `az login` to authenticate with Azure. Note: your AD account must have access to GET secrets from Key Vault.
2. `az account get-access-token --resource https://vault.azure.net` and save the accessToken in your clipboard (it is valid for one hour)
3. `cd` into the `/api` folder
3. `docker build . -f ./Hub.Shell.Api/Dockerfile --build-arg PAT={PAT for private nuget repository}`
4. `docker run image_tag_here -rm -e AZURE_TOKEN={Access token from step 2 here}` to run the newly built image with the token in the environment variable AZURE_TOKEN. Note: you'll have to expose the port via -p
