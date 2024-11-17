FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine as runtime
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY ["./Web/Parbad.Sample.AspNetCore/", "./Web/Parbad.Sample.AspNetCore/"]
COPY ["./Shared/", "./Shared/"]

RUN dotnet publish -c release -o /publish "./Web/Parbad.Sample.AspNetCore/Parbad.Sample.AspNetCore.csproj"

FROM runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Parbad.Sample.AspNetCore.dll"]
EXPOSE 8080