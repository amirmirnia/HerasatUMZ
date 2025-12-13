# مرحله base (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# مرحله build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# کپی csproj ها
COPY ["HerasatUmz/Server/Server.csproj", "HerasatUmz/Server/"]
COPY ["HerasatUmz/Client/Client.csproj", "HerasatUmz/Client/"]
COPY ["HerasatUmz/Domain/Domain.csproj", "HerasatUmz/Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]

RUN dotnet restore "HerasatUmz/Server/Server.csproj"

# کپی کل سورس
COPY . .

WORKDIR "/src/HerasatUmz/Server"
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# مرحله final
FROM base AS final
WORKDIR /app

# کپی خروجی برنامه
COPY --from=publish /app/publish .

# ساخت پوشه HTTPS
RUN mkdir /https

# کپی فایل گواهی (✔️ فقط فایل)
COPY HerasatUmz/Server/aspnetapp.pfx /https/aspnetapp.pfx

# تنظیمات HTTPS
ENV ASPNETCORE_URLS=https://+:443;http://+:80
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

ENTRYPOINT ["dotnet", "Server.dll"]
