# Test Project

# Technology
1. .NET 6 Web API
2. PostgreSQL
3. Entity Framework
4. XUnit, Moq
5. Fluent Validations
6. Angular 15.0.1
7. Material UI

# Steps to Run Project
1. Clone Project into local directory
2. Build Project in Visual Studio. Before making build, please make ensure you have .NET 6 SDK and VS 2022 in your system.
3. Change database connectionstring in `appsettings.json` of WebAPI project
4. Run solution using F5. Please make ensure API project as startup project.
5. From UI project, run below command in console
   
   `npm run start`
6. In Browser, Go to http://localhost:4200
7. As we are seeding one user in API, so one user will be created by default. For that user below are credentials

   Username: Test@gmail.com
   
   Password: Admin@123
