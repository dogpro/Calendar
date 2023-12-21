Для проверки работы сервиса можно использовать Postman, CMD (curl) или PowerShell

Команда для cmd: curl -X POST -d "2023-12-31" http://localhost:8080

Компанда для PowerShell: Invoke-RestMethod -Uri "http://localhost:8080" -Method Post -Body "2023-12-31"
