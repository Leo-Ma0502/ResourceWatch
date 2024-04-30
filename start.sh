#!/bin/bash

cd Services

echo "Starting Docker Compose services..."
docker-compose up -d

echo "Waiting for Docker services to start..."
sleep 1

echo "Running .NET projects..."
cd ../
dotnet run --project FileWatcher/FileWatcherService/FileWatcherService.csproj &
dotnet run --project WebApp/ResourceWatcher/ResourceWatcher.csproj &

wait
