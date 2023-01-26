# MeetUpApp

## Description

CRUD Web API for meetups management (create, update, delete, get/select) using ASP.NET Core with EF Core

## Necessary functionality

1. Getting list of all meetups
2. Getting specific meetup by its id
3. Registration of new meetup
4. Changing information about an existing meetup
5. Deleting specific meetup

## Info about meetup

1. Meetup name
2. Description
3. Speaker
4. Time and its location

## Tech stack

- [x] .NET 6
- [x] MS SQL Server
- [x] Entity Framework Core
- [x] FluentApi
- [x] AutoMapper
- [x] Authentication via IdentityServer4 (JWT Bearer)
- [x] Swagger
- [x] xunit
- [x] FluentAssertions
- [x] Moq
- [x] FluentValidation

In this app all clients are like administrators. _Only logged in user can create, update and delete meetups._ Every guest can read info about every meetup.

## To start application you need

1. Clone repository:

```sh
git clone https://github.com/dmitry-ship-it/MeetUpApp.git
```

2. Move inside sln folder

```sh
cd MeetUpApp
```

3. Build solution:

```sh
dotnet build
```

4. Download EF tools

```sh
dotnet tool install --global dotnet-ef
```

5. Move inside data project folder

```sh
cd MeetUpApp.Data
```

6. Create database

```sh
dotnet ef --startup-project ../MeetUpApp.Api/MeetUpApp.Api.csproj database update
```

7. Move to IdentityServer4 folder

```sh
cd ../MeetUpApp.Identity
```

8. Update database for PersistedGrantDbContext of IdentityServer4

```sh
dotnet ef database update --context PersistedGrantDbContext
```

9. Update database for ConfigurationDbContext of IdentityServer4

```sh
dotnet ef database update --context ConfigurationDbContext
```

10. Start IdentityServer4

```sh
dotnet run -c Release
```

11. Move to API folder

```sh
cd ../MeetUpApp.Api
```

12. Start API

```sh
dotnet run -c Release
```

Now you can navigate to https://localhost:7196/swagger and test it.
Identity server is available on https://localhost:7216
