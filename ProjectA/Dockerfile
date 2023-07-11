FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

ARG BUILDCONFIG=DEBUG
ARG VERSION=1.0.0

COPY ["ProjectA/ProjectA.csproj", "ProjectA/"]
COPY ["MessagingContracts/MessagingContracts.csproj", "MessagingContracts/"]

RUN dotnet restore "ProjectA/ProjectA.csproj"

COPY . .
WORKDIR "/src/ProjectA"
RUN dotnet build "ProjectA.csproj" -c $BUILDCONFIG -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectA.csproj" -c $BUILDCONFIG -o /app/publish /p:Version=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ProjectA.dll"]