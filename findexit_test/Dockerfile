FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["findexit_test/findexit_test.csproj", "findexit_test/"]
RUN dotnet restore "findexit_test/findexit_test.csproj"

COPY . .
RUN dotnet publish "findexit_test/findexit_test.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "findexit_test.dll"]
