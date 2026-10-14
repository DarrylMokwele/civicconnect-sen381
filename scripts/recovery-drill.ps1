$ErrorActionPreference = "Stop"

Write-Host "== CivicConnect recovery evidence drill ==" -ForegroundColor Cyan

$container = "civicconnect-sqlserver-staging"
$backupDir = Join-Path $PSScriptRoot "..\TestResults\Recovery"
New-Item -ItemType Directory -Force -Path $backupDir | Out-Null
$backupFile = "CivicConnectDb-recovery-$(Get-Date -Format yyyyMMdd-HHmmss).bak"
$containerBackup = "/var/opt/mssql/backup/$backupFile"

$running = docker ps --format "{{.Names}}" | Select-String -SimpleMatch $container
if (-not $running) { throw "Staging SQL Server container is not running. Start staging first." }

docker exec $container /bin/bash -c "mkdir -p /var/opt/mssql/backup"
if ($LASTEXITCODE -ne 0) { throw "Could not create SQL backup directory." }

$sql = "BACKUP DATABASE [CivicConnectDb] TO DISK = N'$containerBackup' WITH INIT, CHECKSUM, STATS=10;"
docker exec $container /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "$env:SA_PASSWORD" -C -Q $sql
if ($LASTEXITCODE -ne 0) {
    Write-Warning "sqlcmd v18 path failed; trying legacy sqlcmd path."
    docker exec $container /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$env:SA_PASSWORD" -Q $sql
    if ($LASTEXITCODE -ne 0) { throw "Database backup failed." }
}

docker cp "$container`:$containerBackup" (Join-Path $backupDir $backupFile)
if ($LASTEXITCODE -ne 0) { throw "Could not copy recovery backup out of the container." }

$local = Join-Path $backupDir $backupFile
if (-not (Test-Path $local) -or (Get-Item $local).Length -lt 1024) { throw "Backup file was not created or is unexpectedly small." }

Write-Host "PASS  Recovery backup created: $local" -ForegroundColor Green
Write-Host "The runbook requires a controlled restore rehearsal before final release approval." -ForegroundColor Yellow
