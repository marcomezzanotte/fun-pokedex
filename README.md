# fun-pokedex
Funny AspNetCore Pokedex

## Implementation

### Framework and language
The app is an AspNetCore application using **Minimal API** approach and its written in C# language.

### Source code structure

A Three-Layered Architecture approach has been used in source code organization:
- Presentation/API layer: responsible to handler HTTP protocol details to expose REST API endpoints
- Business logic layer: responsible to implement all the business logic of the application, and it doesn't know techincal details of REST API exposed and external service used.
- Data Layer: responsible to hide all techical details about external service communications (PokeAPI or FunTranslation APIs used to perform requested tasks)

The project is composed of 4 projects:
- FunPokedex.Core: library project implementing pure business logic of the app and defining necessary abstraction (interfaces)
- FunPokedex.Clients.FunTranslations: library project abstracting HTTP client details of FunTranslations APIs implementations and implementing `ITextTranslator` business logic interface
- FunPokedex.Clients.PokeApi: library project abstracting HTTP client details of PokeApi APIs implementations and implementing `IPokemonInfoReader` business logic interface
- FunPokedex.Api: AspNetCore web project exposing REST API and using business logic services. It is responsible to register in DI container and properly setup all required services (through utility `ServiceCollection` extension methods exposed ad-hoc by libraries)

> **Note:** external API URIs are hardcoded in `Program.cs` for simplicity. In a real-world production environment they should be read from AspNetCore Configuration framework and validated on startup.

### Logging
To keep it simple and concise, only **Console log** has been enabled without any specific requirement was given by the assigments.
In a real-world production environment a different log target should be used (usually a centralized logging and monitoring service).

Log scopes have been enabled and used for example by business logic layer, in order to use **structured logging** concepts to enrich log records with useful contextual infos (e.g. Pokemon name subject of a certain client request).

### Testing
In test directory an MSTest project contains unit test for:
- `PokemonDataService` to test business logic
- `PokeApiPokemonInfoReader` and `FunTranslationTextTranslator` to test HTTP response handling (different status code and response data mapping)
Test project uses `Moq` to implement mock objects for data layer interface implmentations and `FluentAssertion` for readable *verify* stage of each test.

To run test the dotnet CLI can be used running the following command:
```
dotnet test .\test\FunPokedex.Tests\FunPokedex.Tests.csproj
```


## Build and run
### `dotnet` CLI command
In this case a dotnet 8+ SDK is required to build and run the application as described here.
Run the following command (from the root directory):
```
dotnet run --urls="http://localhost:8000" --project .\src\FunPokedex.Api\FunPokedex.Api.csproj
```

### Docker
To run the project through Docker, an apline-based image can be built using the Dockerfile in the **build** folder.
Build image through the following command (from the root directory):
```
docker build -f .\build\Dockerfile -t fun-pokedex .
```

and then to run it use
```
docker run -it --rm -p 8000:8080 --name fun-pokedex-local fun-pokedex
```

For both scenario, navigate to `http://localhost:8000/swagger` in order to show the Swagger documentation and test the APIs.
