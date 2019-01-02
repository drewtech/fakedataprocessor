FROM microsoft/dotnet:2.2-sdk-alpine AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY FakeDataProcessor/*.csproj ./FakeDataProcessor/
WORKDIR /app/FakeDataProcessor
RUN dotnet restore

# copy and publish app and libraries
WORKDIR /app/
COPY FakeDataProcessor/. ./FakeDataProcessor/dfi
WORKDIR /app/FakeDataProcessor
RUN dotnet publish -c Release -o out


# test application -- see: dotnet-docker-unit-testing.md
#FROM build AS testrunner
#WORKDIR /app/tests
#COPY tests/. .
#ENTRYPOINT ["dotnet", "test", "--logger:trx"]


FROM microsoft/dotnet:2.2-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/FakeDataProcessor/out ./
ENTRYPOINT ["dotnet", "FakeDataProcessor.dll"]
