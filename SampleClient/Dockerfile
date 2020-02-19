#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SampleClient/SampleClient.csproj", "SampleClient/"]
COPY ["Contest.Shared/Contest.Shared.csproj", "Contest.Shared/"]
RUN dotnet restore "SampleClient/SampleClient.csproj"
COPY . .
WORKDIR "/src/SampleClient"
RUN dotnet build "SampleClient.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleClient.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleClient.dll"]