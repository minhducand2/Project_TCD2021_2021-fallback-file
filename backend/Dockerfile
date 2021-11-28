# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app 
EXPOSE 80
EXPOSE 443
EXPOSE 5001
EXPOSE 8000

# Copy csproj and restore as distinct layers
COPY ApiGen/*.csproj ./ 
RUN dotnet restore

# Copy everything else and build
COPY ApiGen/ ./
RUN dotnet publish -c Release -o out
RUN ls

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
     
WORKDIR /app
COPY --from=build /app/out .

HEALTHCHECK --interval=5m --timeout=3s --start-period=10s --retries=1 \
  CMD curl --fail http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "ApiGen.dll"]