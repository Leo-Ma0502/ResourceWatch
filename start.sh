#!/bin/bash

release_zip="release.zip"
release_dir="release"

echo "Unpacking release..."
unzip $release_zip

cd $release_dir/services

echo "Starting Docker Compose services..."
docker-compose up -d

echo "Waiting for Docker services to start..."
sleep 5

echo "Running .NET projects..."
dotnet ../publish/FileWatcherService.dll &
dotnet ../publish/ResourceWatcher.dll &

wait
