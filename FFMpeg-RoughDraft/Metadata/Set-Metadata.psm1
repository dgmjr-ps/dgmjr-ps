
New-Alias -Name Get-Metadata -Value Get-Media -Option [System.Management.Automation.ScopedItemOptions]::AllScope -Scope "Global"

function Set-Title {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, ValueFromPipeline = $true, Position = 0)]
        [Alias("In", "Input", "InputTile")]
        [string]$InputPath,
        [Parameter(Mandatory = $false)]
        [string]$Title
    )
    
    process {
        if ($Title) {
            Set-Media -InputPath $InputPath -Property @{Title = $Title }
        }
    }
}

function Get-Title {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, ValueFromPipeline = $true, Position = 0)]
        [Alias("In", "Input", "InputTile")]
        [string]$InputPath
    )
    process {
        $Metadata = Get-Media -InputPath $InputPath
        $Metadata.Title ?? "Untitled"
    }
}

function Get-Genre {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, ValueFromPipeline = $true, Position = 0)]
        [Alias("In", "Input", "InputTile")]
        [string]$InputPath
    )
    process {
        $Metadata = Get-Media -InputPath $InputPath
        $Metadata.Genre ?? "No Genre"
    }
}

function Set-Genre {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, ValueFromPipeline = $true, Position = 0)]
        [Alias("In", "Input", "InputTile")]
        [string]$InputPath,
        [Parameter(Mandatory = $false, Position = 1)]
        # [ValidateSet("Porn", "Fisting", "Bareback", "Gangbang", "Interracial", "Black on White", "Power Bottoms", "PnP", "Clouds", "Slamming", "Rape")]
        [string[]]$Genre
    )
    
    process {
        if ($Genre) {
            $genresList = [System.Collections.Generic.List[string]]($Genre);
            $genresList.Sort()
            $singleStringGenre = [string]::Join(", ", $genresList)
            Set-Media -InputPath $InputPath -Property @{Genre = $singleStringGenre }
        }
    }
}

function Get-Director {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $true, ValueFromPipelineByPropertyName = $true, ValueFromPipeline = $true, Position = 0)]
        [Alias("In", "Input", "InputTile")]
        [string]$InputPath
    )
    process {
        $Metadata = Get-Media -InputPath $InputPath
        $Metadata.Director ?? "No Director"
    }
}

Export-ModuleMember -Function * -Alias *
Export-ModuleMember -Cmdlet * -Alias *
Export-ModuleMember -Function *-* -Alias *
