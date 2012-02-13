param($installPath, $toolsPath, $package, $project)

function Resolve-ProjectName {
    param(
        [parameter(ValueFromPipelineByPropertyName = $true)]
        [string]$ProjectName
    )
    
    if($ProjectName) {
        Get-Project $ProjectName
    }
    else {
        Get-Project
    }
}

function Get-Folders {
    Process {
        if ($_.Kind -eq "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}") {
            $_
            $_.ProjectItems | Get-Folders
        }
    }
}

function Get-RelativePath {
    param($projectPath, $file) 
    $filePath = $file.Properties.Item("FullPath").Value
    
    $index = $filePath.IndexOf($projectPath)
    if ($index -ge 0) {
        $filePath.Substring($projectPath.Length)
    }
}

function Get-TinyMCEFolders { 

    (((Resolve-ProjectName $ProjectName).ProjectItems | where { $_.Name -eq "Scripts" }).ProjectItems | Where { $_.Name -eq "tinymce" }).ProjectItems | Get-Folders
}

$solutionExplorer = ($dte.Windows | Where { $_.Type -eq "vsWindowTypeSolutionExplorer" }).Object
$project = (Resolve-ProjectName $projectName)
$projectPath = [IO.Path]::GetDirectoryName($project.FullName)
$ProjectName = $project.Name
$SolutionName = [IO.Path]::GetFileNameWithoutExtension($dte.Solution.FullName)

Get-TinyMCEFolders | % {
    
    $relativePath = Get-RelativePath $projectPath $_
    if ($relativePath) {
        try {
            $solutionExplorer.GetItem("$SolutionName\$ProjectName$relativePath").UIHierarchyItems.Expanded = $false
        } Catch  [system.exception] {
            
            Write-Host "Your package successfully installed but there was a problem while collapsing a folder:"
            Write-Host "Path: $SolutionName\$ProjectName$relativePath"
        }
    }
}