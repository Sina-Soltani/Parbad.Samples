FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine as runtime
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c release -o /publish --no-restore

FROM runtime
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Parbad.Sample.AspNetCore.dll"]
EXPOSE 8080