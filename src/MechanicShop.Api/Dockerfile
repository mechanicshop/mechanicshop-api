FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["MechanicShop.slnx", "."]
COPY ["src/MechanicShop.Api/MechanicShop.Api.csproj", "src/MechanicShop.Api/"]

RUN dotnet restore "src/MechanicShop.Api/MechanicShop.Api.csproj"

COPY . .

RUN dotnet publish "src/MechanicShop.Api/MechanicShop.Api.csproj" \
  -c Release \
  -o /app/publish \
  /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

RUN apt-get update \
  && apt-get install -y --no-install-recommends libgssapi-krb5-2 \
  && rm -rf /var/lib/apt/lists/*

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

EXPOSE 8080

COPY --from=build /app/publish .

CMD ["dotnet", "MechanicShop.Api.dll"]