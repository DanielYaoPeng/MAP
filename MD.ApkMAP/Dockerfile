FROM microsoft/dotnet:3.0-aspnetcore-runtime AS base
#WORKDIR /app
#EXPOSE 9880
#
#FROM microsoft/dotnet:2.1-sdk AS build
#WORKDIR /src
#COPY ["MD.ApkMAP/MD.ApkMAP.csproj", "MD.ApkMAP/"]
#COPY ["MD.Apk.MAP.Services/MD.ApkMAP.Services.csproj", "MD.Apk.MAP.Services/"]
#COPY ["MD.ApkMAP.IServices/MD.ApkMAP.IServices.csproj", "MD.ApkMAP.IServices/"]
#COPY ["MD.ApkMAP.Repository/MD.ApkMAP.Repository.csproj", "MD.ApkMAP.Repository/"]
#COPY ["MD.ApkMAP.IRepository/MD.ApkMAP.IRepository.csproj", "MD.ApkMAP.IRepository/"]
#COPY ["MD.ApkMAP.Common/MD.ApkMAP.Common.csproj", "MD.ApkMAP.Common/"]
#RUN dotnet restore "MD.ApkMAP/MD.ApkMAP.csproj"
#COPY . .
#WORKDIR "/src/MD.ApkMAP"
#RUN dotnet build "MD.ApkMAP.csproj" -c Release -o /app
#
#FROM build AS publish
#RUN dotnet publish "MD.ApkMAP.csproj" -c Release -o /app
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app .
#ENTRYPOINT ["dotnet", "MD.ApkMAP.dll"]

COPY bin/Release/netcoreapp3.0/publish  /app/
WORKDIR /app
EXPOSE 9880
ENTRYPOINT ["dotnet", "MD.ApkMAP.dll"]