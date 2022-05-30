FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

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
CMD ASPNETCORE_URLS=http://*:$PORT dotnet fugdj.dll
