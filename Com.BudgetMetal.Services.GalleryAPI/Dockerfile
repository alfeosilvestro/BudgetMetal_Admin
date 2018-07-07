# set up network
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9908
EXPOSE 9908
#EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Com.BudgetMetal.Services.GalleryAPI.sln ./
COPY Com.BudgetMetal.Services.GalleryAPI/Com.BudgetMetal.Services.GalleryAPI.csproj Com.BudgetMetal.Services.GalleryAPI/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Com.BudgetMetal.Services.GalleryAPI
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Com.BudgetMetal.Services.GalleryAPI.dll"]
