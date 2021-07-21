FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["src/SignalR.Socker.Server/SignalR.Socker.Server.csproj", "src/SignalR.Socker.Server/"]
RUN dotnet restore "src/SignalR.Socker.Server/SignalR.Socker.Server.csproj"
COPY . .
WORKDIR "/src/src/SignalR.Socker.Server"
RUN dotnet build "SignalR.Socker.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SignalR.Socker.Server.csproj" -c Release -o /app/publish

ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SignalR.Socker.Server.dll"]