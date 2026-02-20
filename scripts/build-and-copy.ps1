# PowerShell script for Windows
param(
    [string]$TigerTradePath = "C:\Program Files\Tiger Trade"
)

Write-Host "Building project..." -ForegroundColor Green
dotnet build -c Release

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build successful!" -ForegroundColor Green
    
    $dllPath = "src\TrendLineLibrary\bin\Release\net8.0-windows\TrendLineLibrary.dll"
    $targetPath = "$TigerTradePath\Plugins\"
    
    if (Test-Path $targetPath) {
        Copy-Item $dllPath $targetPath -Force
        Write-Host "DLL copied to Tiger Trade plugins folder" -ForegroundColor Green
    } else {
        Write-Host "Tiger Trade plugins folder not found" -ForegroundColor Red
    }
} else {
    Write-Host "Build failed!" -ForegroundColor Red
}