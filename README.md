# AzureML-WebServices
Helper code to automate web service creation.

To use this tool:

1. Run the Predictive Experiment in Azure ML Studio.

2. Copy the Predictive Experiment URL

3. Get your workspace auth token (Studio -> Settings -> Auth Token).

4. Call GetWSD from the commandline with the Experiment URL and Workspace Auth token

Example (Shown as an example of input to the call. This URL will not work for you. You need to replace the expeirment URL with our experiment url, and use your workspace auth token): 

c:\\yourfoldername\\GetWSD https://studio.azureml.net/Home/ViewWorkspaceCached/c79283d685684d84823473fd3ec49a7f#Workspaces/Experiments/Experiment/c79283d685684d8234gc73fd3ec49a7f.f-id.c75a7792c95c451sd09c7e922fe7fbe0b0/ViewExperiment 81e6731ce987dfb8cadd72067d15599 >"c:\yourfoldername\webservicedefintion_output.json"

5. Edit the json output by adding the Storage Account and CommitmentPlan under the Properties node. For example:

"StorageAccount": {
            "name": "YourStorageAccountName",
            "key": "YourStorageAccountKey"
        },
        "CommitmentPlan": {
            "id": "subscriptions/<yoursubscriptionId>/resourceGroups/<yourresroucename>/providers/Microsoft.MachineLearning/commitmentPlans/<YourPlanName">
        }

You can get the Commitment Plan ID from the Web Services UI by clicking on the plan name.
