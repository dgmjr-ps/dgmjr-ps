using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;
using System.Text;

namespace Dgmjr.PowerShell;

[Cmdlet("Invoke", "Dotnet", DefaultParameterSetName = "Build")]
public class InvokeDotnet : PSCmdlet
{
    protected const string Dotnet = "Dotnet";

    protected const string dotnet = "dotnet";

    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 1, ParameterSetName = "Build", HelpMessage = "The path to the project file to build. Defaults to the first .*proj file in the current directory.")]
    [ValidatePattern("^(?:(?:.*\\.*proj)|(?:.*\\.*props)|(?:.*\\.*targets)|(?:.*\\.*usings)|(?:.*\\.*tasks)|(?:.*\\.*items))$")]
    [Alias(new string[] { "proj", "project", "path", "projpath" })]

    public virtual string? ProjectPath { get; set; } = "./*.*proj";


    [Parameter(ValueFromPipeline = true, Position = 0, ValueFromPipelineByPropertyName = true, Mandatory = false, HelpMessage = "The command to run. Defaults to 'build'.", ParameterSetName = "Build")]
    [ValidateSet(new string[] { "build", "test", "pack", "publish", "clean", "restore", "run", "help" })]
    [Alias(new string[] { "cmd", "command" })]

    public virtual DotnetCommand Command { get; set; } = DotnetCommand.build.Instance;


    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "True if you want to push the build package to the Local feed; false otherwise. Defaults to true.", ParameterSetName = "Push")]
    [Alias(new string[] { "pl", "pshloc", "pushlocl", "pushloc", "plocal" })]

    public SwitchParameter PushLocal { get; set; } = true;


    /// <summary>True if you want to push the build package to the GitHub NuGet feed; false otherwise. Defaults to false.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "True if you want to push the build package to the GitHub NuGet feed; false otherwise. Defaults to false.", ParameterSetName = "Push")]
    [Alias(new string[] { "pgh", "pshgh", "pushgh" })]

    public SwitchParameter PushGitHub { get; set; } = false;


    /// <summary>True if you want to push the build package to the Azure Artifacts NuGet feed; false otherwise. Defaults to false.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Push")]
    [Alias(new string[] { "paz", "pshaz", "pushaz" })]

    public SwitchParameter PushAzure { get; set; } = false;


    /// <summary>True if you want to push the build package to the NuGet.org feed; false otherwise. Defaults to false.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Push")]
    [Alias(new string[] { "pn", "png", "pnu", "pushng" })]

    public SwitchParameter PushNuGet { get; set; } = false;


    /// <summary>True if you want to push the build package to the NuGet.org feed; false otherwise. Defaults to false.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Push")]
    [Alias(new string[] { "nl", "no-logo" })]

    public SwitchParameter NoLogo { get; set; } = false;


    /// <summary>True if you want to restore the build package; false otherwise. Defaults to false.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "True if you want to restore the build package; false otherwise. Defaults to false.", ParameterSetName = "Build")]
    [Alias(new string[] { "no-restore" })]

    public SwitchParameter NoRestore { get; set; } = false;


    /// <summary>True if you want to restore the build package; false otherwise. Defaults to false.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "Allows the command to stop and wait for user input or action (for example to complete authentication).", ParameterSetName = "Build")]
    [Alias(new string[] { "inter" })]

    public SwitchParameter Interactive { get; set; } = false;


    /// <summary>The configuration to build with. Defaults to "Local"</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "The configuration to build with. Defaults to \"Local.\"", ParameterSetName = "Build")]
    [Alias(new string[] { "c", "config" })]

    public string Configuration { get; set; } = "Local";


    /// <summary>The verbosity of the build. Defaults to "minimal"</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Parameter(Mandatory = false, HelpMessage = "The verbosity of the build. Defaults to \"minimal.\"", ParameterSetName = "Build")]
    [ValidateSet(new string[] { "q", "quiet", "m", "minimal", "n", "normal", "d", "detailed", "diag", "diagnostic" })]

    public Verbosity Verbosity { get; set; } = Verbosity.Minimal.Instance;


    /// <summary>The targets to build. Defaults to "Build"</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Parameter(Mandatory = false, HelpMessage = "The targets to build. Defaults to \"Build.\"", ParameterSetName = "Build")]
    [Alias(new string[] { "t" })]

    public string[] Target { get; set; } = new string[1] { "Build" };


    /// <summary>The version of the built package. Defaults to "0.0.1-Local"</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Parameter(Mandatory = false, HelpMessage = "The version of the built package. Defaults to \"0.0.1-Local\"", ParameterSetName = "Build")]
    [Alias(new string[] { "v" })]

    public string? Version { get; set; } = null;


    /// <summary>The version of the built assembly file. Defaults to "0.0.1"</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Parameter(Mandatory = false, HelpMessage = "The version of the built assembly file. Defaults to \"0.0.1\".", ParameterSetName = "Build")]
    [Alias(new string[] { "av", "asmv" })]

    public string? AssemblyVersion { get; set; } = null;


    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias(new string[] { "targets", "show-targets", "ts" })]
    [Parameter(Mandatory = false, HelpMessage = "Prints a list of available targets without executing the actual build process. By default, the output is written to the console window. If the path to an output file is provided that will be used instead.", ParameterSetName = "Build")]
    public StringSwitch PrintTargets { get; set; } = default;


    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias(new string[] { "prop", "properties" })]
    [Parameter(Mandatory = false, HelpMessage = "Set or override these project-level properties. <n> is the property name, and <v> is the property value. Use a semicolon or a comma to separate multiple properties, or specify each property separately.", ParameterSetName = "Build")]
    public string[] Property { get; set; } = Array.Empty<string>();


    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Parameter(Mandatory = false, HelpMessage = "The command to run. Defaults to 'build'.", ParameterSetName = "Build")]
    [Alias(new string[] { "bl", "binlog" })]
    public StringSwitch BinaryLogger { get; set; } = false;


    /// <summary>Prints the help text</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true)]
    [Alias(new string[] { "?", "h" })]
    public SwitchParameter Help { get; set; } = false;


    /// <summary>Adds tags to the build</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, ParameterSetName = "Build")]
    [Alias(new string[] { "tag" })]
    public string[] Tags { get; set; } = new string[1] { "Build" };



    public int ExitCode { get; private set; } = -1;


    protected IEnumerable<object?> Arguments
    {
        get
        {
            yield return Command;
            yield return ProjectPath;
            if (PrintTargets.IsPresent)
            {
                if (PrintTargets == (StringSwitch)"")
                {
                    yield return "-targets";
                }
                else
                {
                    yield return "-targets";
                    yield return PrintTargets;
                }
            }
            if (Target != null)
            {
                string[] target2 = Target;
                foreach (string target in target2)
                {
                    yield return "-t";
                    yield return target;
                }
            }
            if (Version != null)
            {
                yield return "-p:Version=" + Version;
            }
            if (AssemblyVersion != null)
            {
                yield return "-p:AsssemblyVersion=" + AssemblyVersion;
            }
            if (Property != null)
            {
                string[] property2 = Property;
                foreach (string property in property2)
                {
                    yield return "-p";
                    yield return property;
                }
            }
            if ((bool)NoRestore)
            {
                yield return "-no-restore";
            }
            if ((bool)NoLogo)
            {
                yield return "-norestore";
            }
            if ((bool)BinaryLogger)
            {
                yield return "-bl";
                if (BinaryLogger != (StringSwitch)"")
                {
                    yield return BinaryLogger;
                }
            }
            _ = Verbosity;
            if (true)
            {
                yield return "-v";
                yield return Verbosity;
            }
        }
    }

    protected virtual void GetHelp()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("Builds a .NET project.");
        stringBuilder.AppendLine("Usage: dotnet build [options] [project]");
        stringBuilder.AppendLine("Options:");
        stringBuilder.AppendLine("  -h, --help, -?, /?  Prints the help text");
        stringBuilder.AppendLine("  -t, --target        The targets to build. Defaults to \"Build.\"");
        stringBuilder.AppendLine("  -v, --version       The version of the built package. Defaults to \"0.0.1-Local\"");
        stringBuilder.AppendLine("  -av, --asmv         The version of the built assembly file. Defaults to \"0.0.1\"");
        stringBuilder.AppendLine("  -ts, --targets      Prints a list of available targets without executing the actual build process. By default, the output is written to the console window. If the path to an output file is provided that will be used instead.");
        stringBuilder.AppendLine("  -prop, --properties Set or override these project-level properties. <n> is the property name, and <v> is the property value. Use a semicolon or a comma to separate multiple properties, or specify each property separately.");
        stringBuilder.AppendLine("  -bl, --binlog       Serializes all build events to a compressed binary file. By default the file is in the current directory and named \"msbuild.binlog\". The binary log is a detailed description of the build process that can later be used to reconstruct text logs and used by other analysis tools. A binary log is usually 10-20x smaller than the most detailed text diagnostic-level log, but it contains more information");
        stringBuilder.AppendLine("  -tag                Adds tags to the build");
        stringBuilder.AppendLine("  -c, --configuration The configuration to build. Defaults to \"Release\".");
        stringBuilder.AppendLine("  -o, --output        The output directory. Defaults to \"./bin/Release\".");
        stringBuilder.AppendLine("  -f, --framework     The target framework to build. Defaults to \"netstandard2.0\".");
        stringBuilder.AppendLine("  -r, --runtime       The target runtime to build. Defaults to \"win-x64\".");
        stringBuilder.AppendLine("  -p, --project       The project to build. Defaults to the current directory.");
        stringBuilder.AppendLine("  -n, --no-restore    Do not restore the project before building.");
        stringBuilder.AppendLine("  -nr, --no-restore   Do not restore the project before building.");
        stringBuilder.AppendLine("  -d, --diagnostics   Enable diagnostic output.");
        stringBuilder.AppendLine("  -l, --log           The log file to write to. Defaults to \"msbuild.log\" in the current directory.");
        stringBuilder.AppendLine("  -w, --warnaserror   Treat warnings as errors.");
        stringBuilder.AppendLine("  -q, --quiet         Do not log anything to the console.");
        stringBuilder.AppendLine("  -vq, --verbosity    Set the verbosity level. Defaults to \"minimal\". Allowed values are q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic].");
    }

    protected override void ProcessRecord()
    {
        WriteVerbose("ProcessRecord() started");
        if ((bool)Help)
        {
            GetHelp();
            return;
        }
        try
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (Version != null)
            {
                dictionary.Add("Version", Version);
                dictionary.Add("PackageVersionOverride", Version);
            }
            if (AssemblyVersion != null)
            {
                dictionary.Add("AssemblyVersion", AssemblyVersion);
            }
            if (Property != null)
            {
                string[] property = Property;
                foreach (string text in property)
                {
                    string[] source = text.Split('=');
                    dictionary.Add(source.First(), source.Skip(1).FirstOrDefault() ?? "");
                }
            }
            List<string> list = new List<string>();
            if (Target != null)
            {
                list.AddRange(Target);
            }
            List<string> list2 = new List<string>
            {
                "build",
                ProjectPath ?? "*.*proj",
                "--configuration:" + Configuration,
                $"--verbosity:{Verbosity}"
            };
            if ((bool)NoRestore)
            {
                list2.Add("--no-restore");
            }
            if ((bool)BinaryLogger)
            {
                list2.Add("--binaryLogger" + (BinaryLogger.HasValue ? "" : $":{BinaryLogger}"));
            }
            if ((bool)PrintTargets)
            {
                list2.Add("--targets");
            }
            if (Target != null)
            {
                list2.Add(string.Join(" ", Target.Select((string t) => "-t:" + t)));
            }
            WriteVerbose("Args: " + string.Join(" ", list2));
            List<string> list3 = new List<string>();
            List<string> list4 = new List<string>();
            List<string> list5 = new List<string>();
            List<string> list6 = new List<string>();
            List<string> list7 = new List<string>();
            List<string> list8 = new List<string>();
            WriteInformation(new InformationRecord(string.Format("Starting process: {0} {1} {2}", "dotnet", Command, list2), "invokeBase.cs")
            {
                TimeGenerated = DateTime.Now,
                User = Environment.UserName,
                Tags = { "Build" }
            });
            System.Management.Automation.PowerShell ps = System.Management.Automation.PowerShell.Create(RunspaceMode.CurrentRunspace);
            ps = ps.AddCommand("Start-Process");
            ps = ps.AddParameter("FilePath", "dotnet");
            ps = ps.AddParameter("ArgumentList", list2);
            ps = ps.AddParameter("Wait", true);
            ps = ps.AddParameter("PassThru", true);
            ps = ps.AddParameter("NoNewWindow", true);
            ps = ps.AddParameter("UseNewEnvironment", false);
            ps.Streams.Error.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                ErrorRecord sendToPipeline2 = ps.Streams.Error[e.Index];
                WriteObject(sendToPipeline2);
            };
            ps.Streams.Warning.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                WarningRecord warningRecord = ps.Streams.Warning[e.Index];
                WriteObject(warningRecord.ToString());
            };
            ps.Streams.Information.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                InformationRecord sendToPipeline = ps.Streams.Information[e.Index];
                WriteObject(sendToPipeline);
            };
            ps.Streams.Verbose.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                VerboseRecord verboseRecord = ps.Streams.Verbose[e.Index];
                WriteObject(verboseRecord.ToString());
            };
            ps.Streams.Debug.DataAdded += delegate (object sender, DataAddedEventArgs e)
            {
                DebugRecord debugRecord = ps.Streams.Debug[e.Index];
                WriteObject(debugRecord.ToString());
            };
            WriteObject(new InformationRecord(string.Format("Invoking process: {0} {1} with arguents {2}", "dotnet", Command, string.Join(" ", list2)), "invokeBase.cs")
            {
                TimeGenerated = DateTime.Now,
                User = Environment.UserName,
                Tags = { "Build" }
            });
            Collection<PSObject> source2 = ps.Invoke();
            WriteObject(new InformationRecord(string.Format("Finished process: {0} {1} {2}", "dotnet", Command, string.Join(" ", list2)), "invokeBase.cs")
            {
                TimeGenerated = DateTime.Now,
                User = Environment.UserName,
                Tags = { "Build" }
            });
            Array.ForEach<PSObject>(source2.ToArray(), (Action<PSObject>)delegate (PSObject p)
            {
                WriteObject(p);
            });
            if (list3 != null)
            {
                foreach (string item in list3)
                {
                    WriteError(new ErrorRecord(new Exception(item), "Error", ErrorCategory.NotSpecified, null));
                }
            }
            if (list4 != null)
            {
                foreach (string item2 in list4)
                {
                    WriteWarning(item2);
                }
            }
            if (list8 == null)
            {
                return;
            }
            foreach (string item3 in list8)
            {
                WriteInformation(new InformationRecord(item3, "invokeBase.cs")
                {
                    TimeGenerated = DateTime.Now,
                    User = Environment.UserName,
                    Tags = { "Build" }
                });
            }
        }
        catch (Exception ex)
        {
            WriteError(new ErrorRecord(ex, "BuildError: " + ex.Message, ErrorCategory.NotSpecified, ProjectPath)
            {
                ErrorDetails = new ErrorDetails(ex.Message)
            });
        }
    }

    private Queue<InformationRecord> InformationMessages = new Queue<InformationRecord>();

    private void InformationReceived(object sender, DataReceivedEventArgs e)
    {
        InformationMessages.Enqueue(new InformationRecord(e.Data, "invokeBase.cs")
        {
            TimeGenerated = DateTime.Now,
            User = Environment.UserName,
            ProcessId = (uint)((Process)sender).Id
        });
    }

    private Queue<WarningRecord> Warnings = new Queue<WarningRecord>();

    private void WarningReceived(object sender, DataReceivedEventArgs e)
    {
        Warnings.Enqueue(new WarningRecord(e.Data));
    }

    private Queue<ErrorRecord> Errors = new Queue<ErrorRecord>();
    private void ErrorReceived(object sender, DataReceivedEventArgs e)
    {
        Errors.Enqueue(new ErrorRecord(new BuildException(e.Data), "BuildError: " + e.Data, ErrorCategory.NotSpecified, ProjectPath)
        {
            ErrorDetails = new ErrorDetails(e.Data)
        });
    }

    protected override void EndProcessing()
    {
        WriteVerbose("EndProcessing() called");
    }

    protected override void StopProcessing()
    {
        WriteVerbose("StopProcessing() called");
    }
}
