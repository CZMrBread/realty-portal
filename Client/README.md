# Client
Client is a Blazor WebAssembly application that runs in the browser.
It communicates with the Server API to display and manage real estate listings, user accounts, and other data.

## Configuration
The configuration for the Client is located in the `Client/wwwroot/appsettings.json` file.
The `ServerAPI` key specifies the base URL for the Server API:
```json
{
  "ServerAPI": "http://localhost:5094/api/"
}
```

## Structure
The Client project follows vertical slice architecture, with each feature organized into its own folder.
The vertical slices are not strictly separated, but they are organized in a way that makes it easy to find and modify code related to a specific feature.
The main folders in the Client project are:
- [Features](Features) - Contains the vertical slices for each feature of the application.
    - Each feature folder contains the components, services, and other code related to that feature.
- [Infrastructure](Infrastructure) - Contains shared form components and error handlers.
- [Layout](Layout) - Contains the authorization layouts to ensure that only authorized users can access certain pages.

### Inside a feature
Every feature has the same shape:
- `<Feature>ApiClient.cs` - The API client for the feature, which calls the Server API and handles errors.
- `<Feature>Feature.cs` - Registers the API client and the feature's services in the DI container.
- `Components/` - Contains the Blazor components which could be used in other features as well.
