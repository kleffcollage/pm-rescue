#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /app
COPY ["PropertyMataaz/PropertyMataaz.csproj", "PropertyMataaz/"]
RUN dotnet restore "PropertyMataaz/PropertyMataaz.csproj"

COPY PropertyMataaz/. PropertyMataaz/
COPY PropertyMataaz/EmailTemplates/. PropertyMataaz/EmailTemplates

WORKDIR "/app/PropertyMataaz"
RUN dotnet publish -c Release -o out
#RUN dotnet build "PropertyMataaz.csproj" -c Release -o /app/build
#FROM build AS publish
#RUN dotnet publish "PropertyMataaz.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/PropertyMataaz/out ./
CMD ASPNETCORE_URLS=http://*:$PORT dotnet PropertyMataaz.dll