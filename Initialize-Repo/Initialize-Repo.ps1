<#
.SYNOPSIS
    Initializes a repo for a given environment.

.DESCRIPTION
    Initializes a repo for a given environment.

.EXAMPLE
    # Initialize the repo for the Local environment
    Initialize-Repo -Environment "Local"

.EXAMPLE
    # Initialize the repo for the Development environment
    Initialize-Repo -Environment "Debug"

.EXAMPLE
    # Initialize the repo for the Staging environment
    Initialize-Repo -Environment "Staging"

.EXAMPLE
    # Initialize the repo for the Production environment
    Initialize-Repo -Environment "Release"
#>

function Initialize-Repo {
    [CmdletBinding()]
    [Alias("init", "init-repo", "initrepo", "ir")]
    param(
        # The environment to initialize the repo for. Defaults to "Local".
        [Parameter(ValueFromPipeline = $true,
            ValueFromPipelineByPropertyName = $true,
            HelpMessage = "The environment to initialize the repo for. Defaults to 'Local'.")]
        [ValidateSet("Local", "Debug", "Staging", "Release", "Testing")]
        [string]$environment = "Local",
        # The root of the repo. Defaults to the current directory.
        [string]$gitRoot = $PSScriptRoot
    )

    $env:ASPNETCORE_ENVIRONMENT = $environment

    Write-Output "Initializing repo at $gitRoot for $environment environment"

    Push-Location $PSScriptRoot
    Set-Location $gitRoot && dotnet file add https://github.com/Dgmjr/JwcRepoInitSdk/tree/$environment
    Pop-Location

    # Register-PSRepository -Name GitHubPS -SourceLocation https://nuget.pkg.github.com/Dgmjr/index.json -InstallationPolicy Trusted

    rm ./.netconfig;
}
