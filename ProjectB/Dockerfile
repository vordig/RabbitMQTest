FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

COPY ["ProjectB/ProjectB.csproj", "ProjectB/"]
COPY ["MessagingContracts/MessagingContracts.csproj", "MessagingContracts/"]

RUN dotnet restore "ProjectB/ProjectB.csproj"

COPY . .
WORKDIR "/src/ProjectB"
RUN dotnet build "ProjectB.csproj" -c $BUILDCONFIG -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectB.csproj" -c $BUILDCONFIG -o /app/publish /p:Version=$VERSION

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "ProjectB.dll"]