# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
#USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Order/Order.API/Order.API.csproj", "Services/Order/Order.API/"]
COPY ["Services/Order/Order.Infrastructure/Order.Infrastructure.csproj", "Services/Order/Order.Infrastructure/"]
COPY ["Services/Order/Order.Application/Order.Application.csproj", "Services/Order/Order.Application/"]
COPY ["Services/Contracts/OrderSystem.Contracts/OrderSystem.Contracts.csproj", "Services/Contracts/OrderSystem.Contracts/"]
COPY ["Services/Order/Order.Domain/Order.Domain.csproj", "Services/Order/Order.Domain/"]
COPY ["Services/Order/Order.Presentation/Order.Presentation.csproj", "Services/Order/Order.Presentation/"]
RUN dotnet restore "./Services/Order/Order.API/Order.API.csproj"
COPY . .
WORKDIR "/src/Services/Order/Order.API"
RUN dotnet build "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Order.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /app/data && chown -R $APP_UID /app/data
ENTRYPOINT ["dotnet", "Order.API.dll"]