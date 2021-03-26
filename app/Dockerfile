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

#Set locale to PL
RUN apt-get update && DEBIAN_FRONTEND=noninteractive apt-get install -y locales
RUN sed -i -e 's/# pl_PL.UTF-8 UTF-8/pl_PL.UTF-8 UTF-8/' /etc/locale.gen && \
    dpkg-reconfigure --frontend=noninteractive locales && \
    update-locale LANG=pl_PL.UTF-8

ENV LANG pl_PL.UTF-8 
ENV LANGUAGE pl_PL:pl  
ENV LC_ALL pl_PL.UTF-8

#To enable logging on web
#ENV ASPNETCORE_ENVIRONMENT Development 

# Run the app on container startup
#ENTRYPOINT [ "dotnet", "freelancerzy.dll" ]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet freelancerzy.dll 