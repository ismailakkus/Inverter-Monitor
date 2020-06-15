FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY *.sln ./
COPY src/Inverter.Host/Inverter.Host.csproj ./src/Inverter.Host/
COPY src/Inverter.Publish.Mqtt/Inverter.Publish.Mqtt.csproj ./src/Inverter.Publish.Mqtt/
COPY src/Inverter/Inverter.csproj ./src/Inverter/
COPY src/Inverter.GoodWe/Inverter.GoodWe.csproj ./src/Inverter.GoodWe/

RUN dotnet restore

COPY . .
WORKDIR /source/

RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1

VOLUME /app/configuration
VOLUME /app/logging

WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "Inverter.Host.dll"]