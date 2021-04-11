#build image
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build

WORKDIR /src
COPY . .

RUN dotnet restore
RUN dotnet publish -c release -o /app --no-self-contained  --no-restore

# final image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["./schemas"]