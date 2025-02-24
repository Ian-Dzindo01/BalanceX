@echo off
start python -m http.server 8080 --directory server8080
start python -m http.server 8081 --directory server8081
start python -m http.server 8082 --directory server8082
echo All servers started!