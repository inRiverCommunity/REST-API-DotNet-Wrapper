# iPMC REST API .NET Wrapper

## Prerequisites

To being able to use the REST API .NET Wrapper, you need to:

* Have administrative access to a demo, partner or customer inRiver Product Marketing Cloud (iPMC) enviroment.
* Have elevated customer or partner administrative access for package and extension management.
* Have generated an API key for a user that has the bespoke rights.

See https://www.inriver.com for more information about the iPMC product.

## Usage

Add reference to the following namespace:

```csharp
using inRiverCommunity.Connectivity.iPMC;
```

Example to access a partner environment:

```csharp
ApiEnvironment apiEnvironment = ApiEnvironment.Partner;
string restApiKey = @"{the rest api key}";

using IApiClient adapter = new ApiClient(apiEnvironment, restApiKey);
```

To list all packages:

```csharp
List<PackageModel> packages = await adapter.PackageService.GetAllPackages();
```

For more examples, take a look at the test project.
