param environmentNameAbbreviation string
param sqlServerAdminUsername string

@secure()
param sqlServerAdminPassword string
param sqlServerActiveDirectoryAdminLogin string
param sqlServerActiveDirectoryAdminObjectId string
param threatDetectionEmailAddress string

@allowed([
  'Standard'
  'Premium'
  'PremiumV2'
])
param appServicePlanTier string

@allowed([
  '1'
  '2'
  '3'
])
param appServicePlanSize string

@minValue(1)
param appServicePlanInstances int
param azureWebsitesRPObjectId string
param keyVaultReadWriteObjectIds array
param keyVaultFullAccessObjectIds array

@description('')
param redisCacheSku object = {
  name: 'Basic'
  family: 'C'
  capacity: 0
}

var resourceNamePrefix = toLower(environmentNameAbbreviation)
var sqlServerName = '${resourceNamePrefix}-shared-sql'
var sharedStorageAccountName = replace('${resourceNamePrefix}sharedstr', '-', '')
var appServicePlanName = '${resourceNamePrefix}-shared-asp'
var configStorageAccountName = replace('${resourceNamePrefix}configstr', '-', '')
var keyVaultName = replace('${resourceNamePrefix}sharedkv', '-', '')
var redisCacheName = '${resourceNamePrefix}-redis'
var keyVaultAccessPolicies = [
  {
    objectId: azureWebsitesRPObjectId
    tenantId: subscription().tenantId
    permissions: {
      secrets: [
        'Get'
      ]
    }
  }
]
var readWriteAccessPoliciesInner = [for item in keyVaultReadWriteObjectIds: {
  objectId: item
  tenantId: subscription().tenantId
  permissions: {
    secrets: [
      'Get'
      'List'
      'Set'
    ]
  }
}]
var readWriteAccessPolicies = {
  readWriteAccessPolicies: readWriteAccessPoliciesInner
}

var fullAccessPoliciesInner = [for item in keyVaultFullAccessObjectIds: {
  objectId: item
  tenantId: subscription().tenantId
  permissions: {
    keys: [
      'Get'
      'List'
      'Update'
      'Create'
      'Import'
      'Delete'
      'Recover'
      'Backup'
      'Restore'
    ]
    secrets: [
      'Get'
      'List'
      'Set'
      'Delete'
      'Recover'
      'Backup'
      'Restore'
    ]
    certificates: [
      'Get'
      'List'
      'Update'
      'Create'
      'Import'
      'Delete'
      'Recover'
      'Backup'
      'Restore'
      'ManageContacts'
      'ManageIssuers'
      'GetIssuers'
      'ListIssuers'
      'SetIssuers'
      'DeleteIssuers'
    ]
  }
}]
var fullAccessPolicies = {
  fullAccessPolicies: fullAccessPoliciesInner
}

module shared_storage_account_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/storage-account-arm.json' = {
  name: 'shared-storage-account-${environmentNameAbbreviation}'
  params: {
    storageAccountName: sharedStorageAccountName
    storageKind: 'StorageV2'
    minimumTlsVersion: 'TLS1_2'
  }
}

module sql_server_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/sql-server.json' = {
  name: 'sql-server-${environmentNameAbbreviation}'
  params: {
    sqlServerName: sqlServerName
    sqlServerAdminUserName: sqlServerAdminUsername
    sqlServerAdminPassword: sqlServerAdminPassword
    sqlServerActiveDirectoryAdminLogin: sqlServerActiveDirectoryAdminLogin
    sqlServerActiveDirectoryAdminObjectId: sqlServerActiveDirectoryAdminObjectId
    threatDetectionEmailAddress: threatDetectionEmailAddress
    sqlStorageAccountName: sharedStorageAccountName
  }
  dependsOn: [
    shared_storage_account_environmentNameAbbreviation
  ]
}

module app_service_plan_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/app-service-plan.json' = {
  name: 'app-service-plan-${environmentNameAbbreviation}'
  params: {
    appServicePlanName: appServicePlanName
    nonASETier: appServicePlanTier
    aspSize: appServicePlanSize
    aspInstances: appServicePlanInstances
  }
}

module config_storage_account_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/storage-account-arm.json' = {
  name: 'config-storage-account-${environmentNameAbbreviation}'
  params: {
    storageAccountName: configStorageAccountName
    storageKind: 'StorageV2'
    minimumTlsVersion: 'TLS1_2'
  }
}

module key_vault_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/keyvault.json' = {
  name: 'key-vault-${environmentNameAbbreviation}'
  params: {
    keyVaultName: keyVaultName
    enabledForTemplateDeployment: true
    enableSoftDelete: true
    keyVaultAccessPolicies: concat(keyVaultAccessPolicies, readWriteAccessPolicies.readWriteAccessPolicies, fullAccessPolicies.fullAccessPolicies)
  }
}

module redisCache_environmentNameAbbreviation 'tl-platform-building-blocks/ArmTemplates/redis.json' = {
  name: 'redisCache-${environmentNameAbbreviation}'
  params:{
    redisCacheName: redisCacheName
    redisCacheSKU: redisCacheSku.name
    redisCacheFamily: redisCacheSku.family
    redisCacheCapacity: redisCacheSku.capacity
  }
}

output sharedASPName string = appServicePlanName
output sharedKeyVaultName string = keyVaultName
output sharedKeyVaultUri string = key_vault_environmentNameAbbreviation.outputs.KeyVaultUri
output sharedSQLServerName string = sqlServerName
output sharedStorageConnectionString string = shared_storage_account_environmentNameAbbreviation.outputs.storageConnectionString
output configStorageConnectionString string = config_storage_account_environmentNameAbbreviation.outputs.storageConnectionString
output SharedStorageAccountName string = sharedStorageAccountName
output ConfigStorageAccountName string = configStorageAccountName
output RedisCacheName string = redisCacheName
