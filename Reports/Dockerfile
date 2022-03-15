#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Reports/Reports.csproj", "Reports/"]
COPY ["Reports.Infrastructure/Reports.Infrastructure.csproj", "Reports.Infrastructure/"]
COPY ["Reports.Domain/Reports.Domain.csproj", "Reports.Domain/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
COPY ["Reports.Application/Reports.Application.csproj", "Reports.Application/"]
RUN dotnet restore "Reports/Reports.csproj"
COPY . .
WORKDIR "/src/Reports"
RUN dotnet build "Reports.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reports.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reports.dll"]
