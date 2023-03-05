FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IS_Lab1_XML/IS_Lab1_XML.csproj", "IS_Lab1_XML/"]
RUN dotnet restore "IS_Lab1_XML/IS_Lab1_XML.csproj"
COPY . .
WORKDIR "/src/IS_Lab1_XML"
RUN dotnet build "IS_Lab1_XML.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IS_Lab1_XML.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IS_Lab1_XML.dll"]
