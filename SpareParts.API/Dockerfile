#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SpareParts.API/SpareParts.API.csproj", "SpareParts.API/"]
COPY ["SpareParts.Client/SpareParts.Client.csproj", "SpareParts.Client/"]
COPY ["SpareParts.Shared/SpareParts.Shared.csproj", "SpareParts.Shared/"]
RUN dotnet restore "SpareParts.API/SpareParts.API.csproj"
COPY . .
WORKDIR "/src/SpareParts.API"
RUN dotnet build "SpareParts.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpareParts.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpareParts.API.dll"]

# Install wkhtmltopdf
RUN apt-get update && apt-get -y install wkhtmltopdf