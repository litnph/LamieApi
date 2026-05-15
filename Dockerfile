## Build stage
## Monorepo (Render mặc định: context = gốc repo):
##   docker build -f LamieApi/Dockerfile .
## Chỉ thư mục LamieApi làm context:
##   docker build --build-arg PROJECT_ROOT=. -f Dockerfile .
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG PROJECT_ROOT=LamieApi
WORKDIR /src

COPY ${PROJECT_ROOT}/Lamie.sln ./
COPY ${PROJECT_ROOT}/Lamie.API/Lamie.API.csproj ./Lamie.API/
COPY ${PROJECT_ROOT}/Lamie.Application/Lamie.Application.csproj ./Lamie.Application/
COPY ${PROJECT_ROOT}/Lamie.Domain/Lamie.Domain.csproj ./Lamie.Domain/
COPY ${PROJECT_ROOT}/Lamie.Infrastructure/Lamie.Infrastructure.csproj ./Lamie.Infrastructure/
COPY ${PROJECT_ROOT}/Lamie.Shared/Lamie.Shared.csproj ./Lamie.Shared/

RUN dotnet restore "./Lamie.sln"

COPY ${PROJECT_ROOT}/ .
RUN dotnet publish "Lamie.API/Lamie.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

## Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Lamie.API.dll"]
