#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["SGMH.Healthcare.Vedant.API/SGMH.Healthcare.Vedant.API.csproj", "SGMH.Healthcare.Vedant.API/"]
COPY ["SGMH.Healthcare.Vedant.Business/SGMH.Healthcare.Vedant.Business.csproj", "SGMH.Healthcare.Vedant.Business/"]
COPY ["SGMH.Healthcare.Vedant.Data/SGMH.Healthcare.Vedant.Data.csproj", "SGMH.Healthcare.Vedant.Data/"]
RUN dotnet restore "SGMH.Healthcare.Vedant.API/SGMH.Healthcare.Vedant.API.csproj"
COPY . .
WORKDIR "/src/SGMH.Healthcare.Vedant.API"
RUN dotnet build "SGMH.Healthcare.Vedant.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SGMH.Healthcare.Vedant.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SGMH.Healthcare.Vedant.API.dll"]