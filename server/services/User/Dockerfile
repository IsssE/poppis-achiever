# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY *.csproj .

# copy csproj and restore as distinct layers
RUN dotnet restore --use-current-runtime
COPY . . 
# copy everything else and build app
RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "User.dll"]