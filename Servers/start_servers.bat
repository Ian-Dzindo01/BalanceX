@echo off
start cmd /k "cd /d server8080 && python server.py --port 8080"
start cmd /k "cd /d server8081 && python server.py --port 8081"
start cmd /k "cd /d server8082 && python server.py --port 8082"
echo All servers started!
