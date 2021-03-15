# Dockerfile
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 587
#To enable logging on web
#ENV ASPNETCORE_ENVIRONMENT Development 

# Run the app on container startup
#ENTRYPOINT [ "dotnet", "freelancerzy.dll" ]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet freelancerzy.dll 