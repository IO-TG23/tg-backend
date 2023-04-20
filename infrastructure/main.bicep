@allowed([ 'F1' ])
@description('App service plan pricing for the app')
param appServicePricing string = 'F1'

@allowed([ 'Standard_LRS' ])
@description('Storage pricing plan')
param storagePricing string = 'Standard_LRS'

@allowed([ 'DEV'
  'PROD' ])
@description('Environment')
param env string = 'DEV'

param resourcesLocation string = resourceGroup().location

module appservice 'modules/appservice.bicep' = {
  name: 'appservice'
  params: {
    appServicePricing: appServicePricing
    resourcesLocation: resourcesLocation
  }
}

module storage 'modules/storage.bicep' = if (env == 'PROD') {
  name: 'storage'
  params: {
    storagePricing: storagePricing
    resourcesLocation: resourcesLocation
  }
}
