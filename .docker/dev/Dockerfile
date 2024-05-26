FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Install npm command for cdk deploy
RUN apt-get update && \
    apt-get install -y nodejs npm && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* && \
    npm install -g aws-cdk

# Add alias commands
RUN alias format="dotnet format"
RUN alias database:update="dotnet ef database update --project Book_API/Book_API.csproj"

# Add Path to the .NET tools
ENV PATH="/root/.dotnet/tools:$PATH"

# Add aws configurations for cdk deploy
ENV AWS_DEFAULT_REGION=
ENV AWS_ACCOUNT_ID=
ENV AWS_ACCESS_KEY_ID=
ENV AWS_SECRET_ACCESS_KEY=
ENV AWS_COGNITO_USER_POOL_ID=

# Expose port 5001 for the API
EXPOSE 5001