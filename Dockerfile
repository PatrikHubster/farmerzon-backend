ARG IMAGE=latest
FROM mcr.microsoft.com/dotnet/core/sdk:${IMAGE} AS base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT Production
ENV ASPNETCORE_URLS http://*:5000

FROM base AS builder
ARG Configuration=Release
WORKDIR /src
COPY *.sln ./
COPY ./FarmerzonBackend/*.csproj FarmerzonBackend/
COPY ./FarmerzonBackendDataTransferModel/*.csproj FarmerzonBackendDataTransferModel/
COPY ./FarmerzonBackendManager/*.csproj FarmerzonBackendManager/
RUN dotnet restore --verbosity detailed
COPY . .
WORKDIR /src/FarmerzonBackend
RUN dotnet build -c $Configuration -o /app

FROM builder AS publish
ARG Configuration=Release
RUN dotnet publish -c $Configuration -o /app

FROM base as final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FarmerzonBackend.dll"]