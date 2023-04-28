using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management.Automation;

namespace Dgmjr.PowerShell;

[Cmdlet("Invoke", "Build", DefaultParameterSetName = "WithoutCommand")]
[Alias(new string[] { "ib", "build", "invokebuild" })]
public class InvokeBuild : InvokeDotnet
{
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, Mandatory = true, Position = 0, HelpMessage = "The path to the project file to build. Defaults to the first .*proj file in the current directory.")]
    [ValidatePattern("^(?:(?:.*\\.*proj)|(?:.*\\.*props)|(?:.*\\.*targets)|(?:.*\\.*usings)|(?:.*\\.*tasks)|(?:.*\\.*items))$")]
    [Alias(new string[] { "proj", "project", "path", "projpath" })]
    public override string? ProjectPath { get; set; } = "./*.*proj";

    /// <summary>True if you do NOT want to clean the output directory before building. False otherwise.</summary>
    [Parameter(ValueFromPipeline = true, ValueFromPipelineByPropertyName = true, HelpMessage = "True if you do NOT want to clean the output directory before building. False otherwise.")]
    [Alias(new string[] { "no-clean", "nc" })]
    public bool NoClean { get; set; } = false;

    public override DotnetCommand Command
    {
        get => DotnetCommand.build.Instance;
        set { }
    }

    protected override void BeginProcessing()
    {
        WriteVerbose("BeginProcessing() called");
        if ((bool)base.Help)
        {
            GetHelp();
            return;
        }
        try
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (base.Version != null)
            {
                dictionary.Add("Version", base.Version);
                dictionary.Add("PackageVersionOverride", base.Version);
            }
            if (base.AssemblyVersion != null)
            {
                dictionary.Add("AssemblyVersion", base.AssemblyVersion);
            }
            if (base.Property != null)
            {
                string[] property = base.Property;
                foreach (string text in property)
                {
                    string[] source = text.Split('=');
                    dictionary.Add(source.First(), source.Skip(1).FirstOrDefault() ?? "");
                }
            }
            List<string> list = new List<string>();
            if (base.Target != null)
            {
                list.AddRange(base.Target);
            }
            List<string> list2 = new List<string>
            {
                Command.ToString(),
                ProjectPath ?? "*.*proj",
                "--configuration:" + base.Configuration,
                $"--verbosity:{base.Verbosity}"
            };
            if ((bool)base.NoRestore)
            {
                list2.Add("--no-restore");
            }
            if ((bool)base.BinaryLogger)
            {
                list2.Add("--binaryLogger" + (base.BinaryLogger.HasValue ? "" : $":{base.BinaryLogger}"));
            }
            if ((bool)base.PrintTargets)
            {
                list2.Add("--targets");
            }
            if (base.Target != null)
            {
                list2.Add(string.Join(" ", base.Target.Select((string t) => "-t:" + t)));
            }
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
            System.Management.Automation.PowerShell powerShell = System.Management.Automation.PowerShell.Create();
            powerShell = powerShell.AddCommand("Start-Process");
            powerShell = powerShell.AddParameter("FilePath", "dotnet");
            powerShell = powerShell.AddParameter("ArgumentList", list2);
            powerShell = powerShell.AddParameter("Wait", true);
            powerShell = powerShell.AddParameter("PassThru", true);
            powerShell = powerShell.AddParameter("RedirectStandardOutput", false);
            powerShell = powerShell.AddParameter("RedirectStandardError", false);
            powerShell = powerShell.AddParameter("NoNewWindow", true);
            powerShell = powerShell.AddParameter("UseNewEnvironment", true);
            WriteInformation(new InformationRecord(string.Format("Invoking process: {0} {1} with arguentd {2}", "dotnet", Command, list2), "invokeBase.cs")
            {
                TimeGenerated = DateTime.Now,
                User = Environment.UserName,
                Tags = { "Build" }
            });
            Collection<PSObject> source2 = powerShell.Invoke(null, new PSInvocationSettings
            {
                Host = base.Host,
                AddToHistory = true
            });
            WriteInformation(new InformationRecord(string.Format("Finished process: {0} {1} {2}", "dotnet", Command, list2), "invokeBase.cs")
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

    protected override void EndProcessing()
    {
        WriteVerbose("EndProcessing() called");
    }

    protected override void StopProcessing()
    {
        WriteVerbose("StopProcessing() called");
    }
}
