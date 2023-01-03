param environmentNameAbbreviation string
param resourceNamePrefix string
param sharedASPName string
param sharedEnvResourceGroup string
param sharedKeyVaultName string
param sharedSQLServerName string
param sqlDatabaseSkuName string
param sqlDatabaseTier string
param sqlserverlessAutoPauseDelay string

@secure()
param configurationStorageConnectionString string
param uiCustomHostName string
param uiCertificateName string
param internalApiCustomHostname string = ''
param internalApiCertificateName string = ''
param functionCustomHostname string = ''
param functionCertificateName string = ''

@description('An array of container names to create in the storage account.')
param storageAccountContainerArray array

param certificatePrintingBatchesCreateTrigger string
param certificatePrintingBatchSummaryTrigger string
param certificatePrintingRequestTrigger string
param certificatePrintingTrackBatchTrigger string
param learnerGenderTrigger string
param learnerVerificationAndLearningEventsTrigger string
param overallResultCalculationTrigger string
param ucasTransferAmendmentsTrigger string
param ucasTransferEntriesTrigger string
param ucasTransferResultsTrigger string

var uiAppName = '${resourceNamePrefix}-web'
var internalApiAppName = '${resourceNamePrefix}-internal-api'
var appInsightName = '${resourceNamePrefix}-ai'
var functionAppName = '${resourceNamePrefix}-func'
var sqlDatabaseName = '${resourceNamePrefix}-sqldb'
var IntTestSQLDatabaseName = '${resourceNamePrefix}-inttest-sqldb'
var storageAccountName = replace('${resourceNamePrefix}str', '-', '')

module storage_account_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/storage-account-arm.json' = {
  name: 'storage-account-${environmentNameAbbreviation}'
  params: {
    storageAccountName: storageAccountName
    storageKind: 'StorageV2'
    minimumTlsVersion: 'TLS1_2'
  }
}

module storage_account_container_storageAccountContainerArray 'tl-platform-building-blocks/ArmTemplates/storage-container.json' = [for item in storageAccountContainerArray: {
  name: 'storage-account-container${item}'
  params: {
    storageAccountName: storageAccountName
    containerName: item
    publicAccess: 'None'
  }
  dependsOn: [
    storage_account_environmentNameAbbreviation
  ]
}]

module app_insights_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/application-insights.json' = {
  name: 'app-insights-${environmentNameAbbreviation}'
  params: {
    appInsightsName: appInsightName
    attachedService: uiAppName
  }
}

module ui_app_service_certificate_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/app-service-certificate.json' = {
  name: 'ui-app-service-certificate-${environmentNameAbbreviation}'
  params: {
    keyVaultCertificateName: uiCertificateName
    keyVaultName: sharedKeyVaultName
    keyVaultResourceGroup: sharedEnvResourceGroup
    serverFarmId: resourceId(sharedEnvResourceGroup, 'Microsoft.Web/serverFarms', sharedASPName)
  }
}

module ui_app_service_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/app-service.json' = {
  name: 'ui-app-service-${environmentNameAbbreviation}'
  params: {
    appServiceName: uiAppName
    appServicePlanName: sharedASPName
    appServicePlanResourceGroup: sharedEnvResourceGroup
    appServiceAppSettings: [
      {
        name: 'EnvironmentName'
        value: toUpper(environmentNameAbbreviation)
      }
      {
        name: 'ConfigurationStorageConnectionString'
        value: configurationStorageConnectionString
      }
      {
        name: 'Version'
        value: '1.0'
      }
      {
        name: 'ServiceName'
        value: 'Sfa.Tl.ResultsAndCertification'
      }
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: app_insights_environmentNameAbbreviation.outputs.InstrumentationKey
      }
    ]
    appServiceConnectionStrings: []
    customHostName: uiCustomHostName
    certificateThumbprint: ui_app_service_certificate_environmentNameAbbreviation.outputs.certificateThumbprint
  }
}

module internal_api_app_service_certificate_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/app-service-certificate.json' = if (length(internalApiCustomHostname) > 0) {
  name: 'internal-api-app-service-certificate-${environmentNameAbbreviation}'
  scope: resourceGroup(resourceGroup().name)
  params: {
    keyVaultCertificateName: internalApiCertificateName
    keyVaultName: sharedKeyVaultName
    keyVaultResourceGroup: sharedEnvResourceGroup
    serverFarmId: resourceId(sharedEnvResourceGroup, 'Microsoft.Web/serverFarms', sharedASPName)
  }
}

module internal_api_app_service_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/app-service.json' = {
  name: 'internal-api-app-service-${environmentNameAbbreviation}'
  params: {
    appServiceName: internalApiAppName
    appServicePlanName: sharedASPName
    appServicePlanResourceGroup: sharedEnvResourceGroup
    appServiceAppSettings: [
      {
        name: 'EnvironmentName'
        value: toUpper(environmentNameAbbreviation)
      }
      {
        name: 'ConfigurationStorageConnectionString'
        value: configurationStorageConnectionString
      }
      {
        name: 'Version'
        value: '1.0'
      }
      {
        name: 'ServiceName'
        value: 'Sfa.Tl.ResultsAndCertification'
      }
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: app_insights_environmentNameAbbreviation.outputs.InstrumentationKey
      }
    ]
    appServiceConnectionStrings: []
    customHostName: internalApiCustomHostname
    certificateThumbprint: ((length(internalApiCustomHostname) > 0) ? internal_api_app_service_certificate_environmentNameAbbreviation.outputs.certificateThumbprint : '')
  }
}

module function_app_certificate_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/app-service-certificate.json' = if (length(functionCertificateName) > 0) {
  name: 'function-app-certificate-${environmentNameAbbreviation}'
  scope: resourceGroup(resourceGroup().name)
  params: {
    keyVaultCertificateName: functionCertificateName
    keyVaultName: sharedKeyVaultName
    keyVaultResourceGroup: sharedEnvResourceGroup
  }
}

module function_app_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/function-app.json' = {
  name: 'function-app-${environmentNameAbbreviation}'
  params: {
    functionAppName: functionAppName
    appServicePlanName: sharedASPName
    appServicePlanResourceGroup: sharedEnvResourceGroup
    functionAppAppSettings: [
      {
        name: 'EnvironmentName'
        value: toUpper(environmentNameAbbreviation)
      }
      {
        name: 'ConfigurationStorageConnectionString'
        value: configurationStorageConnectionString
      }
      {
        name: 'Version'
        value: '1.0'
      }
      {
        name: 'ServiceName'
        value: 'Sfa.Tl.ResultsAndCertification'
      }
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: app_insights_environmentNameAbbreviation.outputs.InstrumentationKey
      }
      {
        name: 'BlobStorageConnectionString'
        value: storage_account_environmentNameAbbreviation.outputs.storageConnectionString
      }
      {
        name: 'AzureWebJobsStorage'
        value: storage_account_environmentNameAbbreviation.outputs.storageConnectionString
      }
      {
        name: 'AzureWebJobsDashboard'
        value: storage_account_environmentNameAbbreviation.outputs.storageConnectionString
      }
      {
        name: 'WEBSITE_TIME_ZONE'
        value: 'GMT Standard Time'
      }
      {
        name: 'FUNCTIONS_EXTENSION_VERSION'
        value: '~4'
      }
      {
        name: 'LearnerVerificationAndLearningEventsTrigger'
        value: learnerVerificationAndLearningEventsTrigger
      }
      {
        name: 'LearnerGenderTrigger'
        value: learnerGenderTrigger
      }
      {
        name: 'CertificatePrintingBatchesCreateTrigger'
        value: certificatePrintingBatchesCreateTrigger
      }
      {
        name: 'CertificatePrintingRequestTrigger'
        value: certificatePrintingRequestTrigger
      }
      {
        name: 'CertificatePrintingBatchSummaryTrigger'
        value: certificatePrintingBatchSummaryTrigger
      }
      {
        name: 'CertificatePrintingTrackBatchTrigger'
        value: certificatePrintingTrackBatchTrigger
      }
      {
        name: 'UcasTransferEntriesTrigger'
        value: ucasTransferEntriesTrigger
      }
      {
        name: 'UcasTransferResultsTrigger'
        value: ucasTransferResultsTrigger
      }
      {
        name: 'UcasTransferAmendmentsTrigger'
        value: ucasTransferAmendmentsTrigger
      }
      {
        name: 'OverallResultCalculationTrigger'
        value: overallResultCalculationTrigger
      }
      {
        name: 'WEBSITE_LOAD_CERTIFICATES'
        value: '1'
      }
    ]
    customHostName: functionCustomHostname
    certificateThumbprint: ((length(functionCertificateName) > 0) ? reference('function-app-certificate-${environmentNameAbbreviation}', '2018-11-01').outputs.certificateThumbprint.value : '')
    systemAssignedIdentity: 'SystemAssigned'
  }
  dependsOn: [
    function_app_certificate_environmentNameAbbreviation

  ]
}

module sql_database_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/sql-database.json' = {
  name: 'sql-database-${environmentNameAbbreviation}'
  scope: resourceGroup(sharedEnvResourceGroup)
  params: {
    databaseName: sqlDatabaseName
    sqlServerName: sharedSQLServerName
    databaseSkuName: sqlDatabaseSkuName
    databaseTier: sqlDatabaseTier
    serverlessAutoPauseDelay: sqlserverlessAutoPauseDelay
  }
}

module inttest_sql_database_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/sql-database.json' = {
  name: 'inttest-sql-database-${environmentNameAbbreviation}'
  scope: resourceGroup(sharedEnvResourceGroup)
  params: {
    databaseName: IntTestSQLDatabaseName
    sqlServerName: sharedSQLServerName
    databaseSkuName: sqlDatabaseSkuName
    databaseTier: sqlDatabaseTier
    serverlessAutoPauseDelay: sqlserverlessAutoPauseDelay
  }
}

module sql_server_firewall_rules_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/sql-server-firewall-rules.json' = {
  name: 'sql-server-firewall-rules-${environmentNameAbbreviation}'
  scope: resourceGroup(sharedEnvResourceGroup)
  params: {
    firewallRuleNamePrefix: 'AZURE_IP-'
    ipAddresses: union(ui_app_service_environmentNameAbbreviation.outputs.possibleOutboundIpAddresses, function_app_environmentNameAbbreviation.outputs.possibleOutboundIpAddresses)
    serverName: sharedSQLServerName
  }
}

output sqlDatabaseName string = sqlDatabaseName
output IntTestSQLDatabaseName string = IntTestSQLDatabaseName
output uiAppName string = uiAppName
output internalApiAppName string = internalApiAppName
output functionAppName string = functionAppName
output blobStorageAccountName string = storageAccountName
