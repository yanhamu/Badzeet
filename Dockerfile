FROM mcr.microsoft.com/dotnet/sdk:3.1 as build-env
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet publish -o output -c Debug

FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR app
COPY --from=build-env /app/output .
ENTRYPOINT ["dotnet", "Badzeet.Web.dll"]