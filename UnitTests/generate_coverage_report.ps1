$projectDirectory = Get-Location
$outputDirectory = "$projectDirectory\coveragereport"

Write-Host "Executando testes e gerando arquivo de cobertura..."
dotnet test $projectDirectory --collect:"XPlat Code Coverage" /p:CoverletOutputFormat=cobertura

$testResultsDirectory = "$projectDirectory\TestResults"

$coverageFile = Get-ChildItem -Path $testResultsDirectory -Recurse -Filter "*.cobertura.xml" | Sort-Object LastWriteTime -Descending | Select-Object -First 1

if ($coverageFile) {
    Write-Host "Arquivo de cobertura encontrado: $($coverageFile.FullName)"
    
    Write-Host "Gerando relatorio de cobertura HTML em $outputDirectory..."
    reportgenerator "-reports:$($coverageFile.FullName)" "-targetdir:$outputDirectory" "-reporttypes:Html"
    
    if (Test-Path $outputDirectory) {
        Write-Host "Relatorio gerado com sucesso! Acesse o relatorio em $outputDirectory\index.html"
        
        $indexPath = "$outputDirectory\index.html"
        
        if (Test-Path $indexPath) {
            Write-Host "Abrindo o relatorio HTML..."
            Start-Process $indexPath
        } else {
            Write-Host "Erro: O arquivo index.html nao foi encontrado."
        }
    } else {
        Write-Host "Erro: O relatorio HTML nao foi gerado."
    }
} else {
    Write-Host "Erro: Arquivo de cobertura noo encontrado."
}
