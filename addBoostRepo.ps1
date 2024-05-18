param (
    [string]$repoLastPartOfName
)

$tag = "boost-1.85.0"
$projFilePathPart = "\SandboxCpp\SandboxCpp.vcxproj"
$projFilePath = (Get-Location).Path + $projFilePathPart

$repo = "git@github.com:boostorg/" + $repoLastPartOfName
$submodulePath = "submodules/" + $repoLastPartOfName

git submodule add -b develop $repo $submodulePath
Set-Location $submodulePath
git checkout $tag
Set-Location ../..

$xml = [xml](Get-Content -Path $projFilePath)
$nsmgr = new-object Xml.XmlNamespaceManager($xml.NameTable)
$nsmgr.AddNameSpace("x", $xml.Project.xmlns)
$node = $xml.SelectSingleNode("//x:AdditionalIncludeDirectories", $nsmgr)

$dirs = $node.InnerText.Split(";");
$elementToInsert = "C:\repos\2024\interval.NET\submodules\" + $repoLastPartOfName + "\include"
$index = $dirs.Length - 2;
$newDirs = $dirs[0..($index-1)] + $elementToInsert + $dirs[$index..($dirs.Length-1)]

$newText = $newDirs -join ";"
$node.InnerText = $newText

$xml.Save($projFilePath)

$text = "Succesfully checked out " + $repoLastPartOfName + " " + $tag + "into " + $submodulePath + " and added to " + $projFilePath

Write-Output $text