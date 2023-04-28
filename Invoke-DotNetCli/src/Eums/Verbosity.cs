using System.Management.Automation;

namespace Dgmjr.PowerShell.Enums;

[GenerateEnumerationRecordStruct("Verbosity", "Dgmjr.PowerShell")]
public enum Verbosity
{
    [Alias(new string[] { "q" })]
    Quiet,
    [Alias(new string[] { "m" })]
    Minimal,
    [Alias(new string[] { "n" })]
    Normal,
    [Alias(new string[] { "d" })]
    Detailed,
    [Alias(new string[] { "diag" })]
    Diagnostic
}
