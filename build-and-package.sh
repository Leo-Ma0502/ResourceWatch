#!/bin/bash

output_dir="publish"
service_dir="services"
release_dir="release"
zip_file="release.zip"

rm -rf $output_dir $release_dir $zip_file

mkdir -p $output_dir $release_dir/$service_dir

echo "Building web app..."
dotnet publish WebAPP/ResourceWatcher/ResourceWatcher.csproj -c Release -o $output_dir

echo "Building file watch service..."
dotnet publish FileWatcher/FileWatcherService/FileWatcherService.csproj -c Release -o $output_dir

cp -r $service_dir/* $release_dir/$service_dir/

mv $output_dir $release_dir/

echo "Packaging release..."
zip -r $zip_file $release_dir

echo "Build and packaging complete."
