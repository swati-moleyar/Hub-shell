# Infrastructure

An ARM template and file for the Function proxy.

## Deploy

To deploy locally from your machine (until Pipelines are setup):

1. Deploy the Azure resources
```powershell
## Replace resource-group, subscription and parameters as appropriate
az deployment group create \
   --template-file .\template.json \
   --resource-group hub-int \
   --subscription SharedMicroservices-Development \
   --parameters .\parameters.int.json
```

2. Perform the manual steps in Azure Portal if firs time deploying (these cannot be done via ARM template)
  - Turn on Static Website feature on the Storage account
  - Turn on HTTPS for the Custom Domain on the CDN endpoint
  - Add secrets to Key Vault 

3. Update the parameter files as appropriate
