FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY *.sln .
COPY Api/*.csproj ./Api/
COPY Api.Tests/*.csproj ./Api.Tests/
RUN dotnet restore
COPY . .
RUN dotnet build

FROM build AS publish
RUN dotnet publish -c Release -o Release

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS runtime
COPY --from=publish /Release ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "Api.dll"]