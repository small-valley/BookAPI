FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Add Path to the .NET tools
ENV PATH="/root/.dotnet/tools:$PATH"

# Expose port 5001 for the API
EXPOSE 5001