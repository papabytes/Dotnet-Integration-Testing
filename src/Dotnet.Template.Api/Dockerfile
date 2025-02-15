FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dotnet.Template.Api/Dotnet.Template.Api.csproj", "Dotnet.Template.Api/"]
COPY ["Dotnet.Template.Application/Dotnet.Template.Application.csproj", "Dotnet.Template.Application/"]
COPY ["Dotnet.Template.Core/Dotnet.Template.Core.csproj", "Dotnet.Template.Core/"]
COPY ["Dotnet.Template.Infrastructure/Dotnet.Template.Infrastructure.csproj", "Dotnet.Template.Infrastructure/"]
RUN dotnet restore "Dotnet.Template.Api/Dotnet.Template.Api.csproj"
COPY . .
WORKDIR "/src/Dotnet.Template.Api"
RUN dotnet build "Dotnet.Template.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Dotnet.Template.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet.Template.Api.dll"]
