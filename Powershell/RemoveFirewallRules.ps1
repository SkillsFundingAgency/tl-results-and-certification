<# 
.SYNOPSIS 
    Cleans up Azure SQL Server firewall rules (primarily IP adress whitelist) not in user designated safe list. 
 
.DESCRIPTION 
    This runbook is designed to purge Azure SQL Server firewall rules not specified on a defined safe list and logs
    deleted firewall rules in Application Insights for audit purposes. A runbook like this should be impleted to regularly 
    delete whitelisted IP addresses as standard security practice.
    
    For this runbook to work, the SQL Server must be accessible from the runbook worker running this runbook. 
    Make sure the SQL Server allows incoming connections from Azure services by selecting 'Allow Windows Azure Services' 
    on the SQL Server configuration page in Azure. Consequently, this rule that allows Azure IP adresses is 
    excluded from the purge. 
    
    An Azure "RunAsAccount" needs to be created for the Automation account. Documentation can be found here
    https://docs.microsoft.com/en-us/azure/automation/manage-runas-account#create-a-run-as-account-in-the-portal
    
    Finally, to log to Applications Insights, the Custom Events Module ("ApplicationInsightsCustomEvents.zip") must 
    be imported in the Automation account under the "Modules" pane. The zip file can be found here:
    https://gallery.technet.microsoft.com/scriptcenter/Log-Custom-Events-into-847900d7
 
.PARAMETER SqlServer 
    String name of the SQL Server you want to delete rule(s) for
 
.PARAMETER ResourceGroup 
    String name of Azure resource group the SQL Server is contained in
    
.PARAMETER Subscription 
    String name of the Azure subscription the SQL Server is located in
 
.PARAMETER RulesNotToDelete
    String array of rules not to be deleteted. Leave empty to delete all rules  

.PARAMETER InstrumentationKey
    Instrumentation key of application insights account to which logs will be stored
 
.EXAMPLE 
    Use-SqlCommandSample -SqlServer "SQLservername" -ResourceGroup "ResourceGroupname" -Subscription "SubscriptionName" -RulesNotToDelete ["Home","Remote"] -InstrumentationKey "123-***234"
#> 

param( 
    [parameter(Mandatory = $True)] 
    [string] $SqlServer, 
     
    [parameter(Mandatory = $True)] 
    [string] $ResourceGroup, 

    [parameter(Mandatory = $True)] 
    [string] $Subscription, 
     
    [parameter(Mandatory = $True)] 
    [string[]] $RulesNotToDelete,

    [parameter(Mandatory = $False)]
    [string] $InstrumentationKey
)

#Function to log to Applications Insight
function LogAppInsight ([string]$message) {
    $dictionary = New-Object 'System.Collections.Generic.Dictionary[string,string]' 
    $dictionary.Add('Message', "$message") | Out-Null 
    Log-ApplicationInsightsEvent -InstrumentationKey $InstrumentationKey -EventName "Azure Automation" -EventDictionary $dictionary
}

#Azure Authentication
function Login() {
    $connectionName = "AzureRunAsConnection"
    try {
        $servicePrincipalConnection = Get-AutomationConnection -Name $connectionName

        Write-Verbose "Logging in to Azure..." -Verbose
            
        Add-AzAccount `
            -ServicePrincipal `
            -TenantId $servicePrincipalConnection.TenantId `
            -ApplicationId $servicePrincipalConnection.ApplicationId `
            -CertificateThumbprint $servicePrincipalConnection.CertificateThumbprint | Out-Null 
    }
    catch {
        if (!$servicePrincipalConnection) {
            $ErrorMessage = "Connection $connectionName not found."
            throw $ErrorMessage
        }
        else { 
            Write-Error -Message $_.Exception
            throw $_.Exception
        }
    }
}

Write-Output "Excluding Windows AzureIp rule"
$RulesNotToDelete += "AllowAllWindowsAzureIps"

$PurgedRules = ""

try {

    Write-Output "Processing Subscription $Subscription..."
    Select-AzSubscription $Subscription
    
    Write-Output "Getting total list of firewall rules"
    $ServerRules = Get-AzSqlServerFirewallRule -ResourceGroupName $ResourceGroup -ServerName $SqlServer | Select-Object -Property FirewallRuleName
    
    $RulesToDelete = $ServerRules | Where-Object { $_.FirewallRuleName -notin $RulesNotToDelete }
    
    Write-Output "Removing selected rule"
    foreach ($RuleName in $RulesToDelete) {
        $PurgedRules = "$PurgedRules $RuleName"
        Remove-AzSqlServerFirewallRule -FirewallRuleName $RuleName.FirewallRuleName -ResourceGroupName $ResourceGroup -ServerName $SqlServer
    }
    if ($InstrumentationKey) {
        LogAppInsight "Success, following rules deleted: $PurgedRules"
    }
    Write-Output "Success, following rules deleted: $PurgedRules"

}
catch {
    $ErrorMessage = $_.Exception.Message
    if ($InstrumentationKey) {
        LogAppInsight "Failed: $ErrorMessage"
    }
    Write-Output "Failed: $Errormessage"
}
    