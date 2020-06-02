Param(
    [switch]
    $Publish,

    [string]
    $Configuration = 'Release',

    [string]
    $BuildNumber = 'local'
)

$sourceDir = [System.IO.Path]::Combine($PSScriptRoot, '..')
$outputDir = [System.IO.Path]::Combine($PSScriptRoot, '..', 'output')
$webAppSourceDir = [System.IO.Path]::Combine($sourceDir, 'SGMH.Healthcare.Vedant.API');
$apiSolutionFile = 'SGMH.Healthcare.Vedant.API.sln'

Push-Location $sourceDir

# Restore packages
& dotnet restore $apiSolutionFile
if($LASTEXITCODE -ne 0) { exit 1 }

# Run builds
& dotnet build --no-restore --configuration $Configuration --version-suffix $BuildNumber $apiSolutionFile
if($LASTEXITCODE -ne 0) { exit 2 }

if ($Publish -eq $true)
{
    Push-Location $webAppSourceDir
    $appDir = [System.IO.Path]::Combine($outputDir, 'app')
    & dotnet publish --configuration $Configuration --output $appDir --version-suffix $BuildNumber --no-restore
    if($LASTEXITCODE -ne 0) { exit 4 }
    # Compress published items as artifact.
    $zipContentPath = $(Resolve-Path $appDir).Path
    $zipFilePath = [System.IO.Path]::Combine($outputDir, 'app.zip')
    if (Test-Path $zipFilePath)
    {
        Remove-Item $zipFilePath -Force
    }
    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::CreateFromDirectory($zipContentPath, $zipFilePath)

    Write-Host 'Done building API'
}