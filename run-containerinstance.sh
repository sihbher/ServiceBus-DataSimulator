#This scripts shows how to run a container instance in Azure with the image from the previous script

export resourceGroup='[resource group name]'
export subscriptionId='[subscription id]'
export ciName='[container instance name]'
export image='[image name]' #for example sihbher/sb-datasimulator:0.0.1
export topicName='[topic name]'
export interval='[interval in seconds]'
export connectionString='[connection string]'

#Set working subscription
az account set -s $subscriptionId

az container create \
  --resource-group $resourceGroup \
  --name $ciName \
  --image $image \
  --restart-policy OnFailure \
  --environment-variables 'Topic'=$topicName 'Interval'=$interval 'ConnectionString'=$connectionString


#Get the logs
az container logs --resource-group $resourceGroup --name $ciName