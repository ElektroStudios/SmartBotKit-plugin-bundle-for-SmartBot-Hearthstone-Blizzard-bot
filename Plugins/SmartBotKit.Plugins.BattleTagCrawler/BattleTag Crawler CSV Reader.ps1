# ---------------------------------
# BattleTag Crawler CSV Reader v1.1
# ---------------------------------

Function Select-File($initialDirectory) {
    [System.Reflection.Assembly]::LoadWithPartialName("System.Windows.Forms") | Out-Null
    $ofd = New-Object System.Windows.Forms.OpenFileDialog
    $ofd.InitialDirectory = $initialDirectory
    $ofd.AutoUpgradeEnabled = $true
    $ofd.DereferenceLinks = $true
    $ofd.Filter = "CSV files (*.csv)| *.csv"
    $ofd.FilterIndex = 1
    $ofd.Multiselect = $false
    $ofd.RestoreDirectory = $true
    $ofd.ShowHelp = $false
    $ofd.SupportMultiDottedExtensions =  $true
    $ofd.Title = "Select a BattleTag Crawler CSV file"
    $ofd.ShowDialog() | Out-Null
    $ofd.Filename
}

$filepath = Select-File (Resolve-Path (Join-Path (Get-Location) ("..\..\Logs\BattleTag Crawler\")))
If([String]::IsNullOrEmpty($filepath)) {
    Write-host "No file was selected. Exiting..."
    Exit(1)
}
$csv      = Import-Csv -Path ([Management.Automation.WildcardPattern]::Escape($filepath)) -Delimiter "," -Encoding "Unicode"
$columns  = ($csv | select "Date", "Game Mode", "Standard Rank", "Wild Rank", "BattleTag")

$columns | Out-GridView -PassThru -Title ([System.IO.Path]::GetFileName($filepath))
Exit(0)
 
# ------------------
# EXTRACT BATTLETAGS
# ------------------
#
# $battletags = (($columns).BattleTag | Sort-Object)
# ForEach ($battletag in $battletags){ 
#   Write-host $battletag
# }
# Exit(0)


# -----------
# End Of File
# -----------
