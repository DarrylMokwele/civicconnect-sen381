$ErrorActionPreference = "Stop"

Write-Host "== CivicConnect staging verification ==" -ForegroundColor Cyan

function Assert-Healthy($Url) {
    $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 15
    if ($response.StatusCode -ne 200) { throw "$Url returned $($response.StatusCode)" }
    Write-Host "PASS  $Url" -ForegroundColor Green
}

Assert-Healthy "http://localhost:8080/health/live"
Assert-Healthy "http://localhost:8080/health/ready"

$swagger = Invoke-WebRequest -Uri "http://localhost:8080/swagger/index.html" -UseBasicParsing -TimeoutSec 15
if ($swagger.StatusCode -ne 200) { throw "Swagger returned $($swagger.StatusCode)" }
Write-Host "PASS  Swagger" -ForegroundColor Green

$frontend = Invoke-WebRequest -Uri "http://localhost:8080/" -UseBasicParsing -TimeoutSec 15
if ($frontend.StatusCode -ne 200 -or $frontend.Content -notmatch "CivicConnect") { throw "Frontend did not return CivicConnect." }
Write-Host "PASS  Frontend" -ForegroundColor Green

Write-Host "Staging verification completed." -ForegroundColor Green
