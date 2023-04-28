using System;
/*
 * StringSwitch.cs
 *
 *   Created: 2023-01-29-09:43:00
 *   Modified: 2023-01-29-09:43:00
 *
 *   Author: David G. Moore, Jr. <david@dgmjr.io>
 *
 *   Copyright © 2022-2023 David G. Moore, Jr., All Rights Reserved
 *      License: MIT (https://opensource.org/licenses/MIT)
 */


namespace Dgmjr.PowerShell;

using System.Management.Automation;



public struct StringSwitch
{
    public StringSwitch() => Value = bool.FalseString;
    public StringSwitch(bool isPresent) : this(isPresent.ToString() ?? bool.TrueString) { }
    public StringSwitch(string value) => Value = value ?? bool.FalseString;

    public bool IsPresent => !string.IsNullOrEmpty(Value) && !Value.Equals(bool.FalseString, StringComparison.OrdinalIgnoreCase);
    public static readonly StringSwitch Present = new(true);
    public static readonly StringSwitch Empty = new(false);
    public string Value { get; private set; }
    public bool HasValue => IsPresent;



    public override bool Equals(object obj) => obj is StringSwitch other && Equals(other);

    public bool Equals(StringSwitch other) => Value?.Equals(other.Value, StringComparison.OrdinalIgnoreCase) ?? false;

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public static bool operator ==(StringSwitch first, StringSwitch second) => first.Equals(second);
    public static bool operator ==(StringSwitch first, bool second) => first.IsPresent == second;
    public static bool operator ==(bool first, StringSwitch second) => first == second.IsPresent;
    public static implicit operator bool(StringSwitch switchParameter) => switchParameter.IsPresent;
    public static implicit operator StringSwitch(bool value) => new(value);
    public static bool operator !=(StringSwitch first, StringSwitch second) => !(first == second);
    public static bool operator !=(StringSwitch first, bool second) => !(first == second);
    public static bool operator !=(bool first, StringSwitch second) => !(first == second);
    public static implicit operator StringSwitch(SwitchParameter switchParameter) => switchParameter.IsPresent;

    public bool ToBool() => IsPresent;

    public override string ToString() => Value;

    public static StringSwitch FromString(string value) => new(value);

    public static StringSwitch FromBool(bool value) => new(value);

    public static StringSwitch FromObject(object value) => new(value?.ToString());

    public static implicit operator StringSwitch(string value) => new(value);

    public static implicit operator StringSwitch(int value) => new(value.ToString());

    public static implicit operator string(StringSwitch value) => value.Value;


    // public bool IsPresent => false;

    // public static SwitchParameter Present => default(SwitchParameter);

    // public SwitchParameter(bool isPresent)
    // {
    // }

    // public override bool Equals(object obj)
    // {
    //     return false;
    // }

    // public override int GetHashCode()
    // {
    //     return 0;
    // }

    // public static bool operator ==(SwitchParameter first, SwitchParameter second)
    // {
    //     return false;
    // }

    // public static bool operator ==(SwitchParameter first, bool second)
    // {
    //     return false;
    // }

    // public static bool operator ==(bool first, SwitchParameter second)
    // {
    //     return false;
    // }

    // public static implicit operator bool(SwitchParameter switchParameter)
    // {
    //     return false;
    // }

    // public static implicit operator SwitchParameter(bool value)
    // {
    //     return default(SwitchParameter);
    // }

    // public static bool operator !=(SwitchParameter first, SwitchParameter second)
    // {
    //     return false;
    // }

    // public static bool operator !=(SwitchParameter first, bool second)
    // {
    //     return false;
    // }

    // public static bool operator !=(bool first, SwitchParameter second)
    // {
    //     return false;
    // }

    // public bool ToBool()
    // {
    //     return false;
    // }

    // public override string ToString()
    // {
    //     return null;
    // }
}
