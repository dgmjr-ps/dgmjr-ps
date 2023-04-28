@{
    ModuleVersion     = '0.0.1'
    GUID              = '270fdc85-f9a6-4285-9409-b1056558a54b'
    Author            = 'David G. Moore, Jr.'
    CompanyName       = 'Dgmjr'
    Copyright         = '© 2023 David G. Moore, Jr. <david@dgmjr.io>, All Rights Reserved'
    Description       = 'Initializes a repository with a .gitignore, README.md, LICENSE, and .github, .scripts, .config, and .nuget directories'
    ScriptsToProcess  = @(Join-Path $PSScriptRoot 'Initialize-Repo.ps1')
    CmdletsToExport   = @('Initialize-Repo')
    FunctionsToExport = @('Initialize-Repo')
    PrivateData       = @{
        PSData = @{
            ProjectUri = 'https://github.com/Dgmjr/InvokeBuild'
            LicenseUri = 'https://opensource.org/lienses/MIT'
            Tags       = @('repository', 'init', 'repo', 'git')
        }
    }
}
