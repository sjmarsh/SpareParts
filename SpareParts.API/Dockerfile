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
#RUN apt-get update && apt-get -y install wkhtmltopdf
# Note above command installs 0.12.2.4 with un-patched qt.  This doesn't work with options like page footer, size, etc.
RUN apt-get update && apt-get -y install xz-utils && apt-get -y install wget && apt-get -y install libfontconfig1 libxrender1 libxtst6 libxi6
RUN wget https://github.com/wkhtmltopdf/wkhtmltopdf/releases/download/0.12.3/wkhtmltox-0.12.3_linux-generic-amd64.tar.xz
RUN tar vxf wkhtmltox-0.12.3_linux-generic-amd64.tar.xz
RUN cp wkhtmltox/bin/wk* /usr/bin/