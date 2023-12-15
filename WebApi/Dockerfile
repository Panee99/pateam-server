# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY WebApi/WebApi.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /src/WebApi
RUN dotnet publish "WebApi.csproj" -c release -o /app/publish

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
EXPOSE 5000
COPY . /app/publish
ENTRYPOINT ["dotnet", "WebApi.dll"]