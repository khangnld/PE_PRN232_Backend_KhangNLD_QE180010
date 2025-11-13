# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["PE_PRN232_NguyenLeDucKhang_QE180010.csproj", "./"]
RUN dotnet restore "PE_PRN232_NguyenLeDucKhang_QE180010.csproj"

COPY . .
RUN dotnet publish "PE_PRN232_NguyenLeDucKhang_QE180010.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Expose Render port
EXPOSE 10000

# Use dynamic port from Render
ENV ASPNETCORE_URLS=http://+:${PORT}
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "PE_PRN232_NguyenLeDucKhang_QE180010.dll"]
