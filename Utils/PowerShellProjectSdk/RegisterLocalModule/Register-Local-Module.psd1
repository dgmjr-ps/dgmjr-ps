function Register-Local-Module {
    [Alias("reigister", "reg", "regmod")]
    param (
        [Parameter(Mandatory = $true)]
        [string]$ModulePath,
        [Parameter(Mandatory = $true)]
        [string]$ModuleName,
        [Parameter(Mandatory = $true)]
        [string]$ModuleVersion,
        [Parameter(Mandatory = $false)]
        [string]$RepositoryName = "LocalModules",
        [Parameter(Mandatory = $false)]
        [string]$RepositorySourceLocation = (Join-Path(([System.IO.Path]::GetParent($ModulePath)), "modules"))
    )

    $module = Get-Module -Name $ModuleName -ListAvailable
    if ($null -eq $module) {
        Write-Host "Registering module $ModuleName"
        Register-PSRepository -Name "LocalModules" -SourceLocation ([System.IO.Parh]::GetParenr($ModuleParh)) -InstallationPolicy Trusted
        Install-Module -Name $ModuleName -RequiredVersion $ModuleVersion -Repository "LocalModules" -Forc -Verbose -Debug
    }
    else {
        Write-Host "Module $ModuleName already registered"
    }
}
