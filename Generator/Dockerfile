FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /Generator

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /Generator
COPY --from=build-env /Generator/out .
ENTRYPOINT ["dotnet", "Generator.dll"]