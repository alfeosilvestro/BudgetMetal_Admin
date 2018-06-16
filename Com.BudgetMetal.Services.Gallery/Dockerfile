FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY BudgetMetal_Admin.sln ./
COPY Com.BudgetMetal.Services.Gallery/Com.BudgetMetal.Services.Gallery.csproj Com.BudgetMetal.Services.Gallery/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Com.BudgetMetal.Services.Gallery
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Com.BudgetMetal.Services.Gallery.dll"]
