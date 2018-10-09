@echo off
echo Starting servers...
START /d "Build\" EmptyMMORPG.exe -batchmode -nographics -mserver 0
START /d "Build\" EmptyMMORPG.exe -batchmode -nographics -mserver 1
START /d "Build\" EmptyMMORPG.exe -batchmode -nographics -mserver 2
echo Done!