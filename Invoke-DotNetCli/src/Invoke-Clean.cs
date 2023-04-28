using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using Microsoft.Build.Evaluation;

namespace Dgmjr.PowerShell;

[Cmdlet("Invoke", "Clean", DefaultParameterSetName = "WithoutCommand")]
[Alias(new string[] { "clean", "cln", "limpiar", "invokeclean" })]
public class InvokeClean : InvokeDotnet
{
    private const string IntermediateOutputPath = "IntermediateOutputPath";

    private const string OutputPath = "OutputPath";

    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 0, HelpMessage = "The path to the project file to build. Defaults to the first .*proj file in the current directory.")]
    [ValidatePattern("^(?:(?:.*\\.*proj)|(?:.*\\.*props)|(?:.*\\.*targets)|(?:.*\\.*usings)|(?:.*\\.*tasks)|(?:.*\\.*items))$")]
    [Alias(new string[] { "proj", "project", "path", "projpath" })]

    public override string? ProjectPath { get; set; } = "./*.*proj";


    public override DotnetCommand Command
    {
        get => DotnetCommand.clean.Instance;
        set { }
    }

    protected override void BeginProcessing() => base.BeginProcessing();

    protected override void ProcessRecord()
    {
        var project = new Project(ProjectPath);
        var intermediateOutputPath = project.AllEvaluatedProperties.FirstOrDefault((ProjectProperty p) => p.Name == "IntermediateOutputPath");
        var outputPath = project.AllEvaluatedProperties.FirstOrDefault((ProjectProperty p) => p.Name == "OutputPath");
        WriteVerbose("Deleting obj dir: " + intermediateOutputPath?.EvaluatedValue + "...");
        WriteVerbose("Deleting bin dir: " + outputPath?.EvaluatedValue + "...");
        var powerShell = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace)
            .AddCommand("Remove-Item")
            .AddParameter("Path", intermediateOutputPath?.EvaluatedValue ?? "obj")
            .AddParameter("Recurse")
            .AddParameter("Force")
            .AddCommand("Remove-Item")
            .AddParameter("Path", outputPath?.EvaluatedValue ?? "bin")
            .AddParameter("Recurse")
            .AddParameter("Force");
        _ = powerShell.Invoke();
    }

    protected override void EndProcessing() => base.EndProcessing();

    protected override void StopProcessing() => base.StopProcessing();
}
