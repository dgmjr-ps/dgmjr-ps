using System.Management.Automation;

namespace Dgmjr.PowerShell;

[Cmdlet("Publish", "Project", DefaultParameterSetName = "Build")]
[Alias(new string[] { "publish" })]
public class Publish : InvokeBuild
{
    public override DotnetCommand Command
    {
        get => DotnetCommand.publish.Instance;
        set { }
    }
}
