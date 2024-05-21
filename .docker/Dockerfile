FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Install npm command for cdk deploy
RUN apt-get update && \
    apt-get install -y nodejs npm && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/*
RUN npm install -g aws-cdk

# Add Path to the .NET tools
ENV PATH="/root/.dotnet/tools:$PATH"
ENV AWS_DEFAULT_REGION=
ENV AWS_ACCOUNT_ID=
ENV AWS_ACCESS_KEY_ID=
ENV AWS_SECRET_ACCESS_KEY=

# Expose port 5001 for the API
EXPOSE 5001