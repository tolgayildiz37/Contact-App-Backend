#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Contacts/Contacts.csproj", "Contacts/"]
COPY ["Contacts.Infrastructure/Contacts.Infrastructure.csproj", "Contacts.Infrastructure/"]
COPY ["Contacts.Domain/Contacts.Domain.csproj", "Contacts.Domain/"]
COPY ["Contacts.Application/Contacts.Application.csproj", "Contacts.Application/"]
RUN dotnet restore "Contacts/Contacts.csproj"
COPY . .
WORKDIR "/src/Contacts"
RUN dotnet build "Contacts.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Contacts.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Contacts.dll"]
