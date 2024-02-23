# Refer to https://github.com/iQmetrix/k8s.documentation/blob/master/how-tos/dotnet-setup-files/Dockerfile for documentation
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

LABEL org.opencontainers.image.vendor="iQmetrix"
LABEL org.opencontainers.image.source="https://dev.azure.com/iqmetrix/Hub/_git/Hub.Shell"
LABEL org.opencontainers.image.authors="Web Apps and Hub"
LABEL org.opencontainers.image.title="Hub.Shell BFF"
LABEL org.opencontainers.image.description="The ASP.NET Core web API that serves the Hub Shell"

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

WORKDIR /nuget_cache_warmer

RUN wget -qO- https://iqpublicscripts.blob.core.windows.net/scripts/installcredprovider.sh | bash
ARG NUGET_SOURCE=https://pkgs.dev.azure.com/iqmetrix/_packaging/iqmetrix/nuget/v3/index.json
ARG PAT
ENV NUGET_CREDENTIALPROVIDER_SESSIONTOKENCACHE_ENABLED=true \
    VSS_NUGET_EXTERNAL_FEED_ENDPOINTS="{\"endpointCredentials\": [{\"endpoint\":\"${NUGET_SOURCE}\", \"username\":\"Docker\", \"password\":\"${PAT}\"}]}"

WORKDIR /nuget_cache_warmer
COPY **/*.csproj ./
RUN for f in ./*.csproj; do dotnet restore $f --source $NUGET_SOURCE --source https://api.nuget.org/v3/index.json; done

COPY . /code
WORKDIR /code/Hub.Shell.Api
RUN dotnet build -c Release -o /app

# Uncomment once we have test projects
FROM build AS test
WORKDIR /code
RUN for f in ./**/*.Test.csproj; do dotnet test $f --logger "trx;LogFileName=$f.trx"; done
# RUN for f in ./**/*.IntegrationTests.csproj; do dotnet test $f --logger "trx;LogFileName=$f.trx"; done

ARG VERSION
FROM build AS publish
RUN dotnet publish -c Release -o /app -p:InformationalVersion=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ARG VCS_ID
ARG BUILD_DATE
ENV VERSION=${VERSION} \
    VCS_ID=${VCS_ID} \
    BUILD_DATE=${BUILD_DATE}
LABEL org.opencontainers.image.version="${VERSION}"
LABEL org.opencontainers.image.revision="${VCS_ID}"
LABEL org.opencontainers.image.created="${BUILD_DATE}" 

ENTRYPOINT ["dotnet", "Hub.Shell.Api.dll"]
