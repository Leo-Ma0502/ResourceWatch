#!/bin/bash

cd Services

echo "Starting Docker Compose services..."
docker-compose up -d

echo "Waiting for Docker services to start..."
sleep 1

echo "Running .NET projects..."
cd ../
dotnet run --project FileWatcher/FileWatcherService/FileWatcherService.csproj &
PID1=$!
dotnet run --project WebApp/ResourceWatcher/ResourceWatcher.csproj &
PID2=$!

function cleanup {
    echo "Stopping .NET applications..."
    kill $PID1 $PID2
}

trap cleanup SIGINT SIGTERM

wait $PID1 $PID2
