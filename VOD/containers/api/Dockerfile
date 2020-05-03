FROM mcr.microsoft.com/dotnet/core/aspnet:latest AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:latest AS build
WORKDIR /project
COPY ["src/VOD.API/VOD.API.csproj", "src/VOD.API/"]
COPY . .
WORKDIR "/project/src/VOD.API"
RUN dotnet build "VOD.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VOD.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VOD.API.dll"]