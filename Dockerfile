FROM node:13.12.0-alpine AS client
COPY /client /app

WORKDIR /app
RUN npm install
RUN npm run build

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["server/fugdj/fugdj.csproj", "./"]
RUN dotnet restore "fugdj.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "server/fugdj/fugdj.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "server/fugdj/fugdj.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=client /app/dist/ /app/wwwroot/
CMD ASPNETCORE_URLS=http://*:$PORT dotnet fugdj.dll
