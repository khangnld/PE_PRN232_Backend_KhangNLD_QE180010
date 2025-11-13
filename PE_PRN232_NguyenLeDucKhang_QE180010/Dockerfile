# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj file and restore dependencies
COPY ["PE_PRN232_NguyenLeDucKhang_QE180010.csproj", "./"]
RUN dotnet restore "PE_PRN232_NguyenLeDucKhang_QE180010.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "PE_PRN232_NguyenLeDucKhang_QE180010.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "PE_PRN232_NguyenLeDucKhang_QE180010.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published app
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app
USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Entry point
ENTRYPOINT ["dotnet", "PE_PRN232_NguyenLeDucKhang_QE180010.dll"]

