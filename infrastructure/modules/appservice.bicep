param appServicePricing string = 'F1'
param resourcesLocation string

resource appServicePlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: 'tg-backend'
  location: resourcesLocation
  properties: {
    reserved: true
  }
  sku: {
    name: appServicePricing
  }
  kind: 'linux'
}

resource appService 'Microsoft.Web/sites@2022-09-01' = {
  name: 'tg-backend'
  location: resourcesLocation
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      linuxFxVersion: 'DOTNET|7'
    }
  }
}

