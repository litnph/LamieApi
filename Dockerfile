## Build context phải là thư mục LamieApi (chứa Lamie.sln).
## Render: Root Directory = LamieApi, Dockerfile Path = Dockerfile, Docker Context = .
## Local: docker build -f LamieApi/Dockerfile LamieApi
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Lamie.sln ./
COPY Lamie.API/Lamie.API.csproj Lamie.API/
COPY Lamie.Application/Lamie.Application.csproj Lamie.Application/
COPY Lamie.Domain/Lamie.Domain.csproj Lamie.Domain/
COPY Lamie.Infrastructure/Lamie.Infrastructure.csproj Lamie.Infrastructure/
COPY Lamie.Shared/Lamie.Shared.csproj Lamie.Shared/

RUN dotnet restore "./Lamie.sln"

COPY . .
RUN dotnet publish "Lamie.API/Lamie.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Lamie.API.dll"]
