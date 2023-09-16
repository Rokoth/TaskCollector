#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5720
EXPOSE 5721

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["TaskCollector/TaskCollector.csproj", "TaskCollector/"]
COPY ["Common/Common.csproj", "Common/"]
COPY ["Contract/Contract.csproj", "Contract/"]
COPY ["Db/Db.csproj", "Db/"]
COPY ["Deploy/Deploy.csproj", "Deploy/"]
COPY ["Service/Service.csproj", "Service/"]
RUN dotnet restore "TaskCollector/TaskCollector.csproj"
COPY . .
WORKDIR "/src/TaskCollector"
RUN dotnet build "TaskCollector.csproj" -c Release -o /app/build

FROM build AS publish
ENV NODE_OPTIONS=--openssl-legacy-provider
RUN dotnet publish "TaskCollector.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaskCollector.dll"]