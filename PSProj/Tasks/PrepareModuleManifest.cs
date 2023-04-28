/* 
 * PrepareModuleManifest.cs
 * 
 *   Created: 2023-03-30-02:16:47
 *   Modified: 2023-03-30-02:18:30
 * 
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *   
 *   Copyright © 2022 - 2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

namespace Dgmjr.PsProj.Tasks;
using Dgmjr.MSBuild.Extensions;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using static Path;
public class PrepareModuleManifest : MSBTask
{
    private string ProjectFileFullPath => this.TryGetProject().FullPath;
    private string ProjectFileDirectory => GetDirectoryName(this.ProjectFileFullPath);
    private string ProjectFileName => GetFileNameWithoutExtension(this.ProjectFileFullPath);
    private string PsdManifestFileName => Combine(this.ProjectFileDirectory, this.ProjectFileName + ".psd1");
    private IEnumerable<ProjectItem> PsdManifestItems => this.GetAllEvaluatedItems().Where(i => i.ItemType == "PsdManifest");

    public override bool Execute()
    {
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "pwsh",
            Arguments = $"-NoProfile -NonInteractive -Command \"New-ModuleManifest -Path '{PsdManifestFileName}'\" {PsdManifestItems.Select(i => $"-{i.EvaluatedInclude} '{i.GetMetadataValue("Value")}'").Join(" ")}",
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        var ps = new Process { StartInfo = processStartInfo };
        ps.Start();
        ps.WaitForExit();
        return true;
    }
}
