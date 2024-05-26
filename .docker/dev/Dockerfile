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
RUN alias dotnet:publish="dotnet publish -c Release -r linux-x64"

# Add Path to the .NET tools
ENV PATH="/root/.dotnet/tools:$PATH"

# Add aws configurations for cdk deploy
ENV AWS_DEFAULT_REGION=
ENV AWS_ACCOUNT_ID=
ENV AWS_ACCESS_KEY_ID=
ENV AWS_SECRET_ACCESS_KEY=
ENV AWS_COGNITO_USER_POOL_ID=
ENV AWS_COGNITO_USER_POOL_CLIENT_ID=
ENV AWS_COGNITO_USER_POOL_CLIENT_SECRET=
ENV AWS_COGNITO_USER_POOL_DOMAIN=
ENV AWS_COGNITO_REDIRECT_URI=
ENV SIGNIN_SUCCESS_REDIRECT_URI=
ENV AUTH_FAIL_REDIRECT_URI=
ENV COOKIE_DOMAIN=
ENV DEFAULT_CONNECTION_STRING=

# Expose port 5001 for the API
EXPOSE 5001