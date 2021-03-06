## EntityFramework Core sample project for for Parbad Storage.

**Steps to do before running the application**: 
* Add the Migrations using:
```
dotnet-ef migrations add "Your Migration Name" -c ParbadDataContext
```
Note: In this sample the Migrations are already included. You can delete the folder and add them yourself.

* Update your database using:
```
dotnet-ef database update -c ParbadDataContext
```

Learn more about EntityFramework Core Storage: [Using EntityFramework Core Storage](https://github.com/Sina-Soltani/Parbad/wiki/Configuration#using-entityframework-core)
