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
- [x] Entity Framework Core
- [x] MS SQL Server
- [x] AutoMapper
- [x] Authentication via bearer token (JWT Bearer)
- [x] Swagger

## How to work with user?

On first start app will create first user:

- username = **admin**
- password = **Qs3PGVAyyhUXtkRw**

In this app all users are like administrators. _Only logged in user can create, update and delete meetups._ Every guest can read info about every meetup. To create another user you need to be logged in.

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

5. Move inside project folder

```sh
cd MeetUpApp.Api
```

6. Add new migration:

```sh
dotnet ef migrations add InitialCreate
```

7. Apply migration

```sh
dotnet ef database update
```

8. Start the application

```sh
dotnet run
```

Now you can navigate to https://localhost:7196/swagger and test it.
