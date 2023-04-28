[Alias("reinstalmod", "reinstall")]
param (
    [Parameter(
        Mandatory = $false,
        ValueFromPipeline = $true,
        ValueFromPipelineByPropertyName = $true
    )]
    [string]$Path = "$PSScriptRoot/../../",
    [Parameter(
        Mandatory = $false,
        ValueFromPipeline = $true,
        ValueFromPipelineByPropertyName = $true
    )]
    [string]$ProjectName
)
function Reinstall-Module {
    if (Get-Module -ListAvailable -Name $ProjectName) {
        Write-Output "$ProjectName already eists; uninstalling...";
        Remove-Module $ProjectName -Force -Verbose; 
    }
    else {
        Write-Output "$ProjectName does not exist; installing...";
        dotnet build "$PSScriptRoot/../$ProjectName.psproj" --nologo -v:normal -nowarn:README.MD_NOT_FOUND
        Import-Module "$PSScriptRoot/$Path" -Verbose -Debug;
    }
}
