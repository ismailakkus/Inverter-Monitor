FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Inverter.Host/Inverter.Host.csproj", "src/Inverter.Host/"]
COPY ["src/Inverter.Publish.Mqtt/Inverter.Publish.Mqtt.csproj", "src/Inverter.Publish.Mqtt/"]
COPY ["src/Inverter/Inverter.csproj", "src/Inverter/"]
COPY ["src/Inverter.GoodWe/Inverter.GoodWe.csproj", "src/Inverter.GoodWe/"]
RUN dotnet restore "src/Inverter.Host/Inverter.Host.csproj"
COPY . .
WORKDIR "/src/src/Inverter.Host"
RUN dotnet build "Inverter.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inverter.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inverter.Host.dll"]