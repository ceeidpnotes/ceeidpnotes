#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["fy21-simplemtapp/TKFY21-simplemtapp.csproj", "fy21-simplemtapp/"]
RUN dotnet restore "fy21-simplemtapp/TKFY21-simplemtapp.csproj"
COPY . .
WORKDIR "/src/fy21-simplemtapp"
RUN dotnet build "TKFY21-simplemtapp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TKFY21-simplemtapp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TKFY21-simplemtapp.dll"]