## EntityFramework Core sample project for for Parbad Storage.

**Steps to do before running the application**: 
* Before running the application, add the migrations using this command:

```
dotnet-ef migrations add "Your Migration Name" -c ParbadDataContext
```

* Update your database using:
```
dotnet-ef database update -c ParbadDataContext
```

Learn more about EntityFramework Core Storage: [Using EntityFramework Core Storage](https://github.com/Sina-Soltani/Parbad/wiki/Configuration#using-entityframework-core)
