/*
 * BuildException.cs
 *
 *   Created: 2023-01-29-10:16:59
 *   Modified: 2023-01-29-10:17:00
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */

using System.Runtime.Serialization;

public class BuildException : Exception
{
    public BuildException(string message) : base(message) { }
    public BuildException(string message, Exception innerException) : base(message, innerException) { }

    public BuildException()
    {
    }

    protected BuildException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
