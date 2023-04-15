FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build_env
WORKDIR /TG.Backend/app

COPY *.csproj .
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS release
COPY --from=build_env /TG.Backend/app/out .

ENTRYPOINT ["dotnet", "TG.Backend.dll"]
