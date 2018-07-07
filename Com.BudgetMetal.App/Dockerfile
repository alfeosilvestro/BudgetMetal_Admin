# set up network
FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:9908
EXPOSE 9908
#EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY BudgetMetal_Admin.sln ./
COPY Com.BudgetMetal.App/Com.BudgetMetal.App.csproj Com.BudgetMetal.App/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Com.BudgetMetal.App
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Com.BudgetMetal.App.dll"]
