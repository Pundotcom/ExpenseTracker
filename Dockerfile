FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 10000  


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "ExpenseTracker.csproj"
RUN dotnet build "ExpenseTracker.csproj" -c Release -o /app/build


FROM build AS publish
RUN dotnet publish "ExpenseTracker.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


HEALTHCHECK --interval=30s --timeout=10s --retries=3 CMD curl --fail http://localhost:10000 || exit 1


ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "ExpenseTracker.dll"]
