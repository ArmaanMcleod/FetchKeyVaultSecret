# FetchKeyVaultSecret
Fetches secret key from Azure key vault using REST API inside a .NET Core app. Seems pretty useful for building secure Azure applications so I thought I would document the process to the best of my ability. 

## Prerequisites
- [Git](https://git-scm.com/)
- [Get started with Azure](https://azure.microsoft.com/en-us/get-started/)
- [Set and retrieve a secret from Azure Key Vault using the Azure portal](https://docs.microsoft.com/en-us/azure/key-vault/quick-create-portal)
- [Download .NET](https://dotnet.microsoft.com/download)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)

## Building Project

1. Clone project:

    `PS C:\> git clone https://github.com/OpticGenius/FetchKeyVaultSecret.git`

2. `cd` into solution directory:

    `PS C:\> cd FetchKeyVaultSecret`

3. Build project dependencies:

    `PS C:\> dotnet build`

## Authentication

1. Login to your Azure account with the following command:

    `PS C:\> az login`

2. Set your active subscription with:

    `PS C:\> az account set --subscription "<YOUR SUBSCRIPTION NAME>"`

3. Create a Service Principal:

    `PS C:\> az ad sp create-for-rbac -n "<YOUR SERVICE PRINCIPAL NAME>"`

4. Note the following **appId**, **password** and **tenant** values:

    ```
    {
      "appId": "<YOUR APP ID>",
      "displayName": "blahblah",
      "name": "http://blahblah",
      "password": "<YOUR PASSWORD>",
      "tenant": "<YOUR TENANT ID>"
    }
    ```

## Configuration
- Add an `appsettings.json` file to the directory with `Program.cs`
- Fill in the authentication values from above and key vault settings:

    ```
    {
      "AzureADAuthSettings": {
        "appId": "<YOUR APP ID>",
        "password": "<YOUR PASSWORD>",
        "tenantId": "<YOUR TENANT ID>"
      },
      "KeyVaultSettings": {
        "keyVaultName": "<YOUR KEY VAULT NAME>",
        "secretName":  "<YOUR KEY VAULT SECRET NAME>"
      }
    }
    ```

## Permissions
- Give the application permission to get the secret from your keyvault
    
    `PS C:\> az keyvault set-policy --name "<YOUR KEY VAULT NAME>" --spn "<YOUR APP ID>" --secret-permissions get`

## Running Project
- Either run `dotnet run` or run within Visual Studio/Visual Studio Code.