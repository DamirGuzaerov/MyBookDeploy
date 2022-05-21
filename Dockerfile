FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyBook/MyBook.csproj", "MyBook/"]
COPY ["MyBook.Core/MyBook.Core.csproj", "MyBook.Core/"]
COPY ["MyBook.SharedKernel/MyBook.SharedKernel.csproj", "MyBook.SharedKernel/"]
COPY ["MyBook.Infrastructure/MyBook.Infrastructure.csproj", "MyBook.Infrastructure/"]
RUN dotnet restore "MyBook/MyBook.csproj"
COPY . .
WORKDIR "/src/MyBook"
RUN dotnet build "MyBook.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyBook.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MyBook.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MyBook.dll
