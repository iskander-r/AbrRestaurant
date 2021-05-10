FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
ENV ASPNETCORE_URLS=http://+:5000
WORKDIR /
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY ["AbrRestaurant.MenuApi/AbrRestaurant.MenuApi.csproj", "AbrRestaurant.MenuApi/"]
COPY ["AbrRestaurant.Domain/AbrRestaurant.Domain.csproj", "AbrRestaurant.Domain/"]
COPY ["AbrRestaurant.Infrastructure/AbrRestaurant.Infrastructure.csproj", "AbrRestaurant.Infrastructure/"]
COPY ["AbrRestaurant.Application/AbrRestaurant.Application.csproj", "AbrRestaurant.Application/"]
RUN dotnet restore "AbrRestaurant.MenuApi/AbrRestaurant.MenuApi.csproj"
COPY . .
WORKDIR "/AbrRestaurant.MenuApi"
RUN dotnet build "AbrRestaurant.MenuApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AbrRestaurant.MenuApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AbrRestaurant.MenuApi.dll"]