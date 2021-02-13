FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY *.sln .
COPY Calculation/*.csproj ./Calculation/
COPY Calculation.Tests/*.csproj ./Calculation.Tests/
RUN dotnet restore
COPY . .
RUN dotnet build

FROM build AS publish
RUN dotnet publish -c Release -o Release

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS runtime
COPY --from=publish /Release ./

ENTRYPOINT ["dotnet", "Calculation.dll"]