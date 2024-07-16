# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./ProductListApp/ProductListApp.csproj" --disable-parallel
RUN dotnet publish "./ProductListApp/ProductListApp.csproj" -c release -o /app --no-restore

# base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000

ENTRYPOINT ["dotnet", "ProductListApp.dll"]