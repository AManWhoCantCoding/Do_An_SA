FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DocDocGo.sln", "./"]
COPY ["DocDocGo.csproj", "./"]
COPY ["DocDocGo.Tests/DocDocGo.Tests.csproj", "DocDocGo.Tests/"]
RUN dotnet restore "DocDocGo.sln"
COPY . .
RUN dotnet build "DocDocGo.sln" -c Release
RUN dotnet test "DocDocGo.Tests/DocDocGo.Tests.csproj" -c Release --no-build

FROM build AS publish
RUN dotnet publish "DocDocGo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "DocDocGo.dll"]
