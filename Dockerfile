FROM mcr.microsoft.com/dotnet/sdk:latest AS build
WORKDIR /App

# Copy everything
COPY . ./

# Build and publish a release
RUN dotnet publish -o out ./src/Apiblokes.Telnet

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:latest
WORKDIR /App
COPY --from=build /App/out .
VOLUME /App/data

EXPOSE 23

ENTRYPOINT ["dotnet", "Apiblokes.Telnet.dll"]