# Notebodia
**Notebodia** is demo application demonstrated an application built using Vue + Vite as frontend; ASP.NET Core Web API. It allows user to create, update, delete, view note fast and efficient.
- **Techstack**: Vue, Vite, .NET Core Web Api, Dapper, SQL Server

# Feature
**Notebodia** has the following features: 
- Session-based authentication
- CRUD operation on note
- Beautiful & user friendly UI
- Built using the latest version of Tailwind (v4)


# Installation
This application makes use of monorepo, where both the frontend and backend exist on the same repository. 
## Frontend
To start the frontend, you must navigate to the directory
```
cd notebodia_frontend
```
Then you can just install and run it normally with your desired package manager, I just so happended to love Bun 
```
bun install && bun dev
```

## Migration
Before starting the application, we need to init the db. This project used SQL Server, which required you to install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) and SSMS to manage the database.
After initialize the db, you will need to migrate the database. To do this, you can either use bacpac and import it to the db or alternatively if you don't want to use SQL Server, then just open the `migration.sql` and modify the datatype to fit your desired datatabase  

## Backend
Similar to the frontend, you need to navigate to its directory
```
cd notebodia_api
```
Then just run 
```
dotnet build & dotnet run
```
In Development, you can use `watch` for hot reload 
```
dotnet watch
```
## DEMO
Due to my current cloud service limitation (linux/arm64), I was not able to self host SQL Server, so I opted for Postgres instead. The project also used the latest tailwind which currently doesn't support linux cpu architecture. So for now Vercel for frontend, Self-hosted Coolify for backend, Postgres for backend is the deployment util.

Demo available at: [https://notebodia.frontend.sator-tech.live](https://notebodia.frontend.sator-tech.live)
