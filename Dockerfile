FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
RUN apt-get update && apt-get install -y curl
USER app
WORKDIR /app
EXPOSE 3030
EXPOSE 3031

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./ArchitectureSample.sln", "ArchitectureSample.sln"]

# Application
COPY ["./ArchitectureSample.Application.Api/", "ArchitectureSample.Application.Api/"]
COPY ["./ArchitectureSample.Application.Dtos/", "ArchitectureSample.Application.Dtos/"]
COPY ["./ArchitectureSample.Application.Queries/", "ArchitectureSample.Application.Queries/"]
COPY ["./ArchitectureSample.Application.Commands/", "ArchitectureSample.Application.Commands/"]

# Domain
COPY ["./ArchitectureSample.Domain.Core/", "ArchitectureSample.Domain.Core/"]
COPY ["./ArchitectureSample.Domain.Entities/", "ArchitectureSample.Domain.Entities/"]
COPY ["./ArchitectureSample.Domain.Repository/", "ArchitectureSample.Domain.Repository/"]
COPY ["./ArchitectureSample.Domain.Specification/", "ArchitectureSample.Domain.Specification/"]

# Infrastructure
COPY ["./ArchitectureSample.Infrastructure.Core/", "ArchitectureSample.Infrastructure.Core/"]
COPY ["./ArchitectureSample.Infrastructure.Data/", "ArchitectureSample.Infrastructure.Data/"]
COPY ["./ArchitectureSample.Infrastructure.Cache/", "ArchitectureSample.Infrastructure.Cache/"]
COPY ["./ArchitectureSample.Infrastructure.Logging/", "ArchitectureSample.Infrastructure.Logging/"]
COPY ["./ArchitectureSample.Infrastructure.Persistence/", "ArchitectureSample.Infrastructure.Persistence/"]

# Tests
COPY ["./ArchitectureSample.Tests.Unit/", "ArchitectureSample.Tests.Unit/"]
COPY ["./ArchitectureSample.Tests.Steps/", "ArchitectureSample.Tests.Steps/"]
COPY ["./ArchitectureSample.Tests.Features/", "ArchitectureSample.Tests.Features/"]
COPY ["./ArchitectureSample.Tests.Integration/", "ArchitectureSample.Tests.Integration/"]

# Blazor
COPY ["./ArchitectureSample.Application.Blazor.Server/", "ArchitectureSample.Application.Blazor.Server/"]
COPY ["./ArchitectureSample.Application.Blazor.Client/", "ArchitectureSample.Application.Blazor.Client/"]


RUN dotnet restore "ArchitectureSample.sln"

RUN dotnet test "ArchitectureSample.Tests.Unit" -c Release --no-restore
RUN dotnet test "ArchitectureSample.Tests.Features" -c Release --no-restore
RUN dotnet test "ArchitectureSample.Tests.Integration" -c Release --no-restore

COPY . .

WORKDIR "/src/ArchitectureSample.Application.Api"

RUN dotnet build "./ArchitectureSample.Application.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ArchitectureSample.Application.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArchitectureSample.Application.Api.dll"]