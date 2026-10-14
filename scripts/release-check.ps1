$ErrorActionPreference = "Stop"

$solution = Join-Path $PSScriptRoot "..\CivicConnect\CivicConnect.sln"

Write-Host "== CivicConnect release candidate checks ==" -ForegroundColor Cyan

dotnet restore $solution
if ($LASTEXITCODE -ne 0) { throw "dotnet restore failed." }

dotnet build $solution -c Release --no-restore -warnaserror -p:EnableNETAnalyzers=true -p:AnalysisLevel=latest
if ($LASTEXITCODE -ne 0) { throw "Release build failed." }

dotnet test $solution -c Release --no-build --no-restore --logger "trx;LogFileName=release-tests.trx" --results-directory (Join-Path $PSScriptRoot "..\TestResults\Release")
if ($LASTEXITCODE -ne 0) { throw "Automated tests failed." }

Write-Host "Release checks passed. Review test results before tagging the release candidate." -ForegroundColor Green
