using System.Diagnostics;
using System.Management.Automation;

namespace Dgmjr.PowerShell;

[Cmdlet("Invoke", "Pack", DefaultParameterSetName = "WithoutCommand")]
[Alias(new string[] { "pk", "pack", "invokepack" })]
public class InvokePack : InvokeDotnet
{
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 0, HelpMessage = "The path to the project file to build. Defaults to the first .*proj file in the current directory.")]
    [ValidatePattern("^(?:(?:.*\\.*proj)|(?:.*\\.*props)|(?:.*\\.*targets)|(?:.*\\.*usings)|(?:.*\\.*tasks)|(?:.*\\.*items))$")]
    [Alias(new string[] { "proj", "project", "path", "projpath" })]

    public override string? ProjectPath { get; set; } = "./*.*proj";


    public override DotnetCommand Command
    {
        get => DotnetCommand.pack.Instance;
        set { }
    }
}
