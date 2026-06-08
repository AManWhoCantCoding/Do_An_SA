FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MediSphere.sln", "./"]
COPY ["MediSphere.csproj", "./"]
COPY ["MediSphere.Tests/MediSphere.Tests.csproj", "MediSphere.Tests/"]
RUN dotnet restore "MediSphere.sln"
COPY . .
RUN dotnet build "MediSphere.sln" -c Release
RUN dotnet test "MediSphere.Tests/MediSphere.Tests.csproj" -c Release --no-build

FROM build AS publish
RUN dotnet publish "MediSphere.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "MediSphere.dll"]
