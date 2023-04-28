function Convert-RelativeToAbsolutePath {
    [OutputType([System.Management.Automation.PathInfo], [System.String])]
    [Alias("ToAbsolutePth", "ToAbsolute")]
    [OutputType([string])]
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true)]
        [Alias('Fullname', "in", "i", "Input", "InputFile")]
        [string]$RelativePath
    )

    begin {
        $AbsolutePath = $null
    }
    process {
        $AbsolutePath = Resolve-Path -Path $RelativePath
    }
    end {
        return $AbsolutePath
    }
}
