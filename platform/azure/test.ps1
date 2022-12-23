$ErrorActionPreference = "Stop"

if ((Get-AzContext).Subscription.Name -ne 's126-tlevelservice-development') {
    throw 'Azure Context references incorrect subscription'
}

$scriptRoot = $PSScriptRoot
if (($PSScriptRoot).Length -eq 0) { $scriptRoot = $PWD.Path}

$location = "westeurope"
$applicationPrefix = "resac"
$envPrefix = "s126d02"
$environmentNameAbbreviation = "dev"
$templateFilePrefix = "tlevels"
$certsToUpload = @{   
    "d01-manage-tlevel-results-tlevels-gov-uk" = '173E73EEF9FDB82488D00B49EC4578D556909F6B'
    "learnerrecordservicecertificate" = 'bae31aef5a8c6058a060ca053f6f9dd4292b1f57'
}

$sharedResourceGroupName = $envPrefix + "-$($applicationPrefix)-shared"
$envResourceGroupName = $envPrefix + "-$($applicationPrefix)-$($environmentNameAbbreviation)"

# purge the keyvault if it's in InRemoveState due to resource group deletion
# this is up front as it's such a common reason for the script to fail
if (Get-AzKeyVault -Vaultname "$($envPrefix)$($applicationPrefix)sharedkv" -Location $location -InRemovedState) { 
    Write-Host 'Purging vault'
    Remove-AzKeyVault -VaultName "$($envPrefix)$($applicationPrefix)sharedkv" -InRemovedState -Location $location -Force
}

Get-AzResourceGroup -Name $sharedResourceGroupName -ErrorVariable notPresent -ErrorAction SilentlyContinue
if ($notPresent) {
    $tags = @{
        "Environment" = "Dev"
        "Parent Business" = "Education and Skills Funding Agency"
        "Portfolio" = "Education and Skills Funding Agency"
        "Product" = "T-Levels"
        "Service" = "ESFA T Level Service"
        "Service Line" = "Professional and Technical Education"
        "Service Offering" = "ESFA T Level Service"
    }
    New-AzResourceGroup -Name $sharedResourceGroupName -Location $location -Tag $tags
}

$sharedDeploymentParameters = @{
    Name                    = "test-{0:yyyyMMdd-HHmmss}" -f (Get-Date)
    ResourceGroupName       = $sharedResourceGroupName
    Mode                    = "Complete"
    Force                   = $true
    TemplateFile            = "$($scriptRoot)/$($templateFilePrefix)-shared.bicep"
    TemplateParameterObject = @{
        environmentNameAbbreviation             = "$($envPrefix)-$($applicationPrefix)"
        sqlServerAdminUsername                  = "xxxServerAdminxxx"
        sqlServerAdminPassword                  = ([System.Web.Security.Membership]::GeneratePassword(16, 2))
        sqlServerActiveDirectoryAdminLogin      = "s126-tlevelservice-Managers USR"
        sqlServerActiveDirectoryAdminObjectId   = "56f27acd-6ea8-4526-a25c-29436d62826c"
        threatDetectionEmailAddress             = "noreply@example.com"
        appServicePlanTier                      = "Standard"
        appServicePlanSize                      = "1"
        appServicePlanInstances                 = 1
        azureWebsitesRPObjectId                 = "0b11c7a6-2868-4728-b83c-d14be9147a97"
        keyVaultReadWriteObjectIds              = @("0316d3ae-e503-4dae-9665-c999fca7cf10", "a6621090-e704-45ec-b65f-50257f9d4dcd")
        keyVaultFullAccessObjectIds             = @("b3b225a1-7c11-4698-9f15-32c345cf5bc2")  
        redisCacheSKU                           = @{name= "Basic"; family= "C"; capacity= 0}     
    }
}

$sharedDeployment = New-AzResourceGroupDeployment @sharedDeploymentParameters

foreach ($key in $certsToUpload.Keys) {
    $certPassword = ConvertTo-SecureString `
                        -AsPlainText `
                        -Force `
                        -String ([System.Web.Security.Membership]::GeneratePassword(32, 2))

    Export-PfxCertificate `
        -Password $certPassword `
        -FilePath "$($key).pfx" `
        -Cert "cert://CurrentUser/my/$($certsToUpload[$key])"

    Import-AzKeyVaultCertificate `
            -VaultName "$($envPrefix)$($applicationPrefix)sharedkv" `
            -Name $key `
            -FilePath "$($key).pfx" `
            -Password $certPassword   
    
    Remove-Item -Path "$($key).pfx"
    Clear-Variable -Name certPassword
}

Get-AzResourceGroup -Name $envResourceGroupName -ErrorVariable notPresent -ErrorAction SilentlyContinue
if ($notPresent) {
    $tags = @{
        "Environment" = "Dev"
        "Parent Business" = "Education and Skills Funding Agency"
        "Portfolio" = "Education and Skills Funding Agency"
        "Product" = "T-Levels"
        "Service" = "ESFA T Level Service"
        "Service Line" = "Professional and Technical Education"
        "Service Offering" = "ESFA T Level Service"
    }
    New-AzResourceGroup -Name $envResourceGroupName -Location $location -Tag $tags
}

$envDeploymentParameters = @{
    Name                    = "test-{0:yyyyMMdd-HHmmss}" -f (Get-Date)
    ResourceGroupName       = $envResourceGroupName
    Mode                    = "Incremental"
    TemplateFile            = "$($scriptRoot)/$($templateFilePrefix)-environment.bicep"
    TemplateParameterObject = @{
        environmentNameAbbreviation                 = $environmentNameAbbreviation
        resourceNamePrefix                          = ("$($envPrefix)-$($applicationPrefix)-" + $environmentNameAbbreviation)
       # logAnalyticsWorkspaceFQRId                  = ($sharedDeployment.Outputs.logAnalyticsWorkspaceFQRId.Value)
        sharedASPName                               = "$($envPrefix)-$($applicationPrefix)-shared-asp"
        sharedEnvResourceGroup                      = $sharedResourceGroupName
        sharedKeyVaultName                          = "$($envPrefix)$($applicationPrefix)sharedkv"
        sharedSQLServerName                         = "$($sharedDeployment.Outputs.sharedSQLServerName.Value)"
        sqlDatabaseSkuName                          = 'S0'
        sqlDatabaseTier                             = 'Standard'
        sqlserverlessAutoPauseDelay                 = '-1'
        configurationStorageConnectionString        = ($sharedDeployment.Outputs.configStorageConnectionString.Value)
        uiCustomHostname                            = "d01.manage-tlevel-results.tlevels.gov.uk"                     
        uiCertificateName                           = "d01-manage-tlevel-results-tlevels-gov-uk"
        storageAccountContainerArray                = @()
        learnerVerificationAndLearningEventsTrigger = "0 0 1 1 2"
        learnerGenderTrigger                        = "1 0 1 1 2"
        certificatePrintingBatchesCreateTrigger     = "2 0 1 1 2"
        certificatePrintingRequestTrigger           = "3 0 1 1 2"
        certificatePrintingBatchSummaryTrigger      = "4 0 1 1 2"
        certificatePrintingTrackBatchTrigger        = "5 0 1 1 2"
        ucasTransferEntriesTrigger                  = "6 0 1 1 2"
        ucasTransferResultsTrigger                  = "7 0 1 1 2"
        ucasTransferAmendmentsTrigger               = "8 0 1 1 2"
        overallResultCalculationTrigger             = "9 0 1 1 2"
    }
}

$envDeployment = New-AzResourceGroupDeployment @envDeploymentParameters -ErrorVariable errorOutput
if ($envDeployment.ProvisioningState -eq "Succeeded") {
    Write-Output "Yippee!!"
}

<#
# you have to remove the diagnostic settings separately as they hang around if you don't and mess things up badly
$subscriptionId = (Get-AzContext).Subscription.Id
$diagnosticResourceIds = @(
    "/subscriptions/$($subscriptionId)/resourceGroups/$($sharedResourceGroupName)/providers/Microsoft.KeyVault/vaults/$($envPrefix)$($applicationPrefix)sharedkv",
    "/subscriptions/$($subscriptionId)/resourceGroups/$($envResourceGroupName)/providers/Microsoft.Web/sites/$($envPrefix)-$($applicationPrefix)-$($environmentNameAbbreviation)-web",
    "/subscriptions/$($subscriptionId)/resourceGroups/$($envResourceGroupName)/providers/Microsoft.Web/sites/$($envPrefix)-$($applicationPrefix)-$($environmentNameAbbreviation)-func"
    "/subscriptions/$($subscriptionId)/resourceGroups/$($envResourceGroupName)/providers/Microsoft.Web/sites/$($envPrefix)-$($applicationPrefix)-$($environmentNameAbbreviation)-internal-api"
)
foreach ($diagnosticResourceId in $diagnosticResourceIds) {
    Write-Host "Finding settings in $($diagnosticResourceId) to remove"
    foreach ($setting in (Get-AzDiagnosticSetting -ResourceId $diagnosticResourceId -ErrorAction SilentlyContinue)) {
        Write-Host "Removing $(($setting).Name)"
        Remove-AzDiagnosticSetting -ResourceId $diagnosticResourceId -Name $setting.Name
    }   
}

Remove-AzOperationalInsightsWorkspace -ResourceGroupName $sharedResourceGroupName -Name "$($sharedResourceGroupName)-log" -ForceDelete -Force -ErrorAction SilentlyContinue

Remove-AzResourceGroup -ResourceGroupName $envResourceGroupName -Force -ErrorAction SilentlyContinue
Remove-AzResourceGroup -ResourceGroupName $sharedResourceGroupName -Force -ErrorAction SilentlyContinue
#>

