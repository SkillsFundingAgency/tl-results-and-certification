$server = "jf-test-001-svr"

$resourceGroup = "tf-test-001"
$subscription = "83140706-7c33-427a-a373-27883c159e91"
$instrumentationKey = ""#"a1bbb9b8-6238-4b64-a9fe-56d70913c0f2"
$rulesnotToDelete = @('DfE (Coventry CH Internet NAT)','DfE (London Internet NAT)','DfE (London Peering)','DfE (Sheffield Internet NAT 1)','DfE (Sheffield Internet NAT 2)','DfE (Sheffield Peering)','GovWifi | Purple | Guest','DfE (London VPN)')

$ip=  (Invoke-WebRequest -uri "http://ifconfig.me/ip").Content


$rule = "ClientIPAddress_2020-6-2_23-46-23"
New-AzSqlServerFirewallRule -ServerName $server -ResourceGroupName $resourceGroup -FirewallRuleName $rule -StartIpAddress $ip -EndIpAddress $ip | Out-Null

$rule = "Jay_Home"
New-AzSqlServerFirewallRule -ServerName $server -ResourceGroupName $resourceGroup -FirewallRuleName $rule -StartIpAddress $ip -EndIpAddress $ip | Out-Null

$rule = "DfE (Coventry CH Internet NAT)"
New-AzSqlServerFirewallRule -ServerName $server -ResourceGroupName $resourceGroup -FirewallRuleName $rule -StartIpAddress $ip -EndIpAddress $ip | Out-Null

$rule = "DfE (London Internet NAT)"
New-AzSqlServerFirewallRule -ServerName $server -ResourceGroupName $resourceGroup -FirewallRuleName $rule -StartIpAddress $ip -EndIpAddress $ip | Out-Null

$rule = "DfE (London VPN)"
New-AzSqlServerFirewallRule -ServerName $server -ResourceGroupName $resourceGroup -FirewallRuleName $rule -StartIpAddress $ip -EndIpAddress $ip | Out-Null


$rule = "GovWifi | Purple | Guest"
New-AzSqlServerFirewallRule -ServerName $server -ResourceGroupName $resourceGroup -FirewallRuleName $rule -StartIpAddress $ip -EndIpAddress $ip | Out-Null

.\Powershell\RemoveFirewallRules.ps1 -SqlServer $server -ResourceGroup $resourceGroup -Subscription $subscription -RulesNotToDelete $rulesnotToDelete -InstrumentationKey $instrumentationKey