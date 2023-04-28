using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Dgmjr.PowerShell;

public static class PowerShellExtensions
{
    public const RunspaceMode DefaultRunspaceMode = RunspaceMode.CurrentRunspace;

    public static readonly InitialSessionState InitialSessionState = System.Management.Automation.Runspaces.InitialSessionState.CreateDefault();

    public static System.Management.Automation.PowerShell CreateCommand(string command, string[] arguments, RunspaceMode runspaceMode = RunspaceMode.CurrentRunspace)
    {
        return System.Management.Automation.PowerShell.Create(runspaceMode).AddCommand(command).AddParameters(arguments.ToList());
    }

    public static System.Management.Automation.PowerShell CreateCommand(string command, IDictionary<string, object> arguments, InitialSessionState initialSessionState)
    {
        return System.Management.Automation.PowerShell.Create(initialSessionState).AddCommand(command).AddParameters(arguments as IDictionary);
    }

    public static System.Management.Automation.PowerShell CreateCommand(string command, string[] arguments)
    {
        return CreateCommand(command, arguments);
    }

    public static System.Management.Automation.PowerShell CreateCommand(string command, IDictionary<string, object> arguments)
    {
        return CreateCommand(command, arguments);
    }
}
