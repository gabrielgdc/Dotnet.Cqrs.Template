FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src

COPY . .
RUN dotnet publish "src/Cqrs.Template" -c Release -o /app/publish
FROM mcr.microsoft.com/dotnet/sdk:6.0

WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 5000
EXPOSE 5001
ENTRYPOINT ["dotnet", "Cqrs.Template.dll"]
