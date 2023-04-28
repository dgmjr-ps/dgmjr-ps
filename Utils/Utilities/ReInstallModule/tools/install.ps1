if (Get-Module -ListAvailable -Name Reinstall-Module) {
    Write-Output "Reinstall-Module already eists; uninstalling...";
    Remove-Module Reinstall-Module -Force -Verbose; 
}
else {
    Write-Output "Reinstall-Module does not exist; installing...";
    dotnet build "$PSScriptRoot/../ReinstallModule.csproj" --nologo -v:normal -nowarn:README.MD_NOT_FOUND
    Import-Module "$PSScriptRoot/../" -Verbose -Debug;
}
# Publish-Module -Path ./ -Verbose;
# Register-PSRepository -Name 'PSGallery' -SourceLocation 'https://www.powershellgallery.com/api/v2' -InstallationPolicy Trusted -Verbose;
