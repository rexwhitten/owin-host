# owin-host test scripts 

Write-Host "TESTING POLICY POST"
$policy = @{
    SysKey = "1"
} | ConvertTo-Json;

Invoke-RestMethod -Method Post -Uri "http://127.0.0.1:9980/Policy" `
                            -ContentType "applivation/json" `
                            -Body $policy 