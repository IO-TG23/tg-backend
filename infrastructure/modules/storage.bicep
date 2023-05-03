param storagePricing string = 'Standard_LRS'
param resourcesLocation string

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: uniqueString('tg-backend')
  kind: 'BlobStorage'
  location: resourcesLocation
  sku: {
    name: storagePricing
  }
  properties:{
    accessTier: 'Hot'
  }
}
