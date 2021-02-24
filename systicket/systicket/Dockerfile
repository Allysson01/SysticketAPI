#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

# Comentar para publicar
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["systicket.csproj", ""]
RUN dotnet restore "./systicket.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "systicket.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "systicket.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Padrão de container ASP.NET
# Comentar para publicar no heroku
ENTRYPOINT ["dotnet", "systicket.dll"]

# Opção utilizada pelo Heroku
# Descomentar para publicar no heroku
#CMD ASPNETCORE_URLS=http://*:$PORT dotnet dataStack2.0.dll

## REGION PARA RODAR NO HEROKU
#Vá até o diretório onde se encontra esse DockerFile e rode os comando a seguir

#docker build -t datastack-api .

#heroku login

#heroku container:login

#heroku container:push web -a datastack-api

#heroku container:release web -a datastack-api

#Link da aplicação https://datastack-api.herokuapp.com/api/home

## ENDREGION

## PARA RODAR NO DOCKER LOCAL
#docker build -t systicket-api .
#docker run -d -p 47591:443 --name systicket-api systicket-api

# https://www.treinaweb.com.br/blog/publicando-uma-aplicacao-asp-net-core-no-heroku/#:~:text=Para%20isso%2C%20utilizamos%20o%20comando,podemos%20public%C3%A1%2Dla%20no%20Heroku.