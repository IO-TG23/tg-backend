FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build_env
WORKDIR /app

COPY /TG.Backend/*.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS release
COPY --from=build_env /app/out .

ENTRYPOINT ["dotnet", "TG.Backend.dll"]
