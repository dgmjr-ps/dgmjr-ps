@{
    ModuleVersion     = '0.0.1'
    GUID              = 'a177a1e3-44c4-48ec-a237-aa8a5a8fa985'
    Author            = 'David G. Moore, Jr.'
    CompanyName       = 'Dgmjr'
    Copyright         = '© 2023 David G. Moore, Jr. <david@dgmjr.io>, All Rights Reserved'
    Description       = 'Builds a project using the dotnet cli'
    # ScriptsToProcess  = @(Join-Path $PSScriptRoot 'Invoke-Build.ps1')
    CmdletsToExport   = @('Get-CustomerInfoNew')
    # Variables to export from this module
    VariablesToExport = @('*')
    # Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
    AliasesToExport   = @('*')
    NestedModules     = @("0PSScriptRoot..\Classes\AutoCompleteAttribute.ps1")
    # FunctionsToExport = @('Invoke-Build', 'Invoke-Pack', 'Invoke-Function')
    PrivateData       = @{
        PSData = @{
            ProjectUri = 'https://github.com/Dgmjr/InvokeBuild'
            License    = 'MIT'
            Tags       = @('build', 'dotnet', 'cli', 'nuget', 'package', 'push')
        }
    }
}
