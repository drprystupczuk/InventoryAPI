FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY InventoryAPI.csproj ./InventoryAPI/
RUN dotnet restore InventoryAPI/InventoryAPI.csproj

COPY . ./InventoryAPI/

WORKDIR /src/InventoryAPI
RUN dotnet publish -c Debug -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
COPY --from=build /app .

EXPOSE 5000

ENTRYPOINT ["dotnet", "InventoryAPI.dll"]