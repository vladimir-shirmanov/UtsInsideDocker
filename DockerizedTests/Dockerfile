FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TodoList.Api/TodoList.Api.csproj", "TodoList.Api/"]
COPY ["TodoList.Tests/TodoList.Tests.csproj", "TodoList.Tests/"] 
RUN dotnet restore "TodoList.Api/TodoList.Api.csproj"
COPY . .
RUN dotnet build "TodoList.Api/TodoList.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet build "TodoList.Tests/TodoList.Tests.csproj" -c $BUILD_CONFIGURATION -o /app

FROM build AS test
RUN dotnet test -l "trx;logfilename=testResults.trx"; exit 0;

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TodoList.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoList.Api.dll"]
