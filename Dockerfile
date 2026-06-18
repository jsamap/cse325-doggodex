# Step 1: Build the application using the .NET 9 SDK
FROM ://microsoft.com AS build
WORKDIR /src

# Copy the csproj file and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the remaining source files and build the app
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Step 2: Create the runtime image
FROM ://microsoft.com AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render exposes traffic via the PORT environment variable.
# This forces the .NET app to listen on that exact port.
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "DoggoDex.dll"]
