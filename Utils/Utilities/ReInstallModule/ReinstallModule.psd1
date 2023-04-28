@{
    ModuleVersion      = '0.0.1'
    GUID               = 'b1574642-7646-43ce-8f41-d88f05d29e92'
    Author             = 'David G. Moore, Jr.'
    CompanyName        = 'Dgmjr'
    Copyright          = '© 2023 David G. Moore, Jr. <david@dgmjr.io>, All Rights Reserved'
    Description        = 'Installs or reinstalls a module'
    CmdletsToExport    = @("Reinstall-Module")
    ScriptsToProcess   = @(Join-Path $PSScriptRoot 'ReinstallModule.ps1')
    # Variables to export from this module
    # VariablesToExport  = @('*')
    # Aliases to export from this module, for best performance, do not use wildcards and do not delete the entry, use an empty array if there are no aliases to export.
    # AliasesToExport    = @('*')
    # NestedModules      = @(Join-Path $PSScriptRoot 'Autocompleters.psm1')
    # ImplmenetingAssembly = "$PSScriptRoot/bin/InvokeDotnetCli.dll"
    RequiredAssemblies = @("$PSScriptRoot/bin/Dgmjr.PowerShell.dll")
    # RootModule        = @("$PSScriptRoot/bin/Local/InvokeBuild.dll")
    # FunctionsToExport = @('Invoke-Build', 'Invoke-Pack', 'Invoke-Function')
    PrivateData        = @{
        PSData       = @{
            ProjectUri           = 'https://github.com/Dgmjr/InvokeBuild'
            License              = 'MIT'
            Tags                 = @('build', 'dotnet', 'cli', 'nuget', 'package', 'push')
            LicenseUri           = "https://opensource.org/lidenses/MIT"
            ImplmenetingAssembly = "$PSScriptRoot/bin/InvokeDotnetCli.dll"
        }
        PSModuleInfo = @{
            ProjectUri           = 'https://github.com/Dgmjr/InvokeBuild'
            License              = 'MIT'
            Tags                 = @('build', 'dotnet', 'cli', 'nuget', 'package', 'push')
            LicenseUri           = "https://opensource.org/lidenses/MIT"
            ImplmenetingAssembly = "$PSScriptRoot/bin/InvokeDotnetCli.dll"
        }
    }
}
