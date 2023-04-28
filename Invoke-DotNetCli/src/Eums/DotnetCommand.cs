/*
 * DotnetCommand.cs
 *
 *   Created: 2023-03-07-02:42:09
 *   Modified: 2023-03-07-02:42:09
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.ComponentModel.DataAnnotations;

namespace Dgmjr.PowerShell.Enums;

[GenerateEnumerationRecordStruct("DotnetCommand", "Dgmjr.PowerShell")]
public enum DotnetCommand
{
    [Display(Name = "None", Description = "Don't use this.")]
    _,
    [Display(Name = "Build", Description = "Builds a project and its dependencies.")]
    build,
    [Display(Name = "Test", Description = "Runs unit tests in a project.")]
    test,
    [Display(Name = "Pack", Description = "Creates a NuGet package.")]
    pack,
    [Display(Name = "Publish", Description = "Publishes a project for deployment.")]
    publish,
    [Display(Name = "Clean", Description = "Cleans the output of a project.")]
    clean,
    [Display(Name = "Restore", Description = "Restores the dependencies and tools of a project.")]
    restore,
    [Display(Name = "Run", Description = "Runs an application without any explicit compile or launch commands.")]
    run,
    [Display(Name = "Help", Description = "Displays help for a command.")]
    help
}
