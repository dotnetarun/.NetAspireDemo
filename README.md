# Task Manager App
A modern, cloud-ready Task Manager built with .NET Aspire, ASP.NET Core, Blazor, and Redis. This app demonstrates a full-stack application with a frontend, backend API, and database, orchestrated using .NET Aspire.

## Features
Add, view, and complete tasks.  
Responsive Blazor frontend.  
Minimal API backend with Redis for data storage.  
.NET Aspire Dashboard for monitoring logs, metrics, and traces.  
## How to Run
Install .NET 8 SDK.  
Clone this repository.  
Run dotnet run --project TaskManagerApp.AppHost.  
Open the frontend (e.g., http://localhost:5000) and Dashboard (e.g., http://localhost:18888).  
## Technologies
**.NET Aspire**: Orchestrates the app and provides the Dashboard.  
**ASP.NET Core**: Backend API and Blazor frontend.  
**Redis**: Fast key-value store for tasks.  
**Bootstrap**: Clean UI styling.  
## Structure
**TaskApi** : Backend API for task management.  
**TaskWeb**: Blazor frontend for user interaction.  
**TaskManagerApp.AppHost**: .NET Aspire host to orchestrate services.  
**TaskManagerApp.ServiceDefaults**: Shared service configurations. 

Built by _Arunkumar Murugan_ to showcase modern .NET development.  
