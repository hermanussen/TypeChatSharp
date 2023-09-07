
To see TypeChatSharp in action, check out the examples found in this directory.

Please note that results may not always be equivalent to the examples in [the original TypeChat examples](https://github.com/microsoft/TypeChat/tree/main/examples). Generally speaking, it seems to be a bit more difficult to specify a schema in C# than it is using TypeScript. But the examples that can be found here do work, so it is doable.

We recommend reading each example in the following order.

| Name | Description |
| ---- | ----------- |
| [Sentiment](https://github.com/hermanussen/TypeChatSharp/tree/main/Examples/TypeChatSharp.Example.Sentiment) | A sentiment classifier which categorizes user input as negative, neutral, or positive. This is TypeChatSharp's "hello world!" |
| [Calendar](https://github.com/hermanussen/TypeChatSharp/tree/main/Examples/TypeChatSharp.Example.Calendar) | An intelligent scheduler. This sample translates user intent into a sequence of actions to modify a calendar. |
| [Restaurant](https://github.com/hermanussen/TypeChatSharp/tree/main/Examples/TypeChatSharp.Example.Restaurant) | An intelligent agent for taking orders at a restaurant. |

## Step 1: Clone repository and open in Visual Studio

You should also be able to run the examples from the command line or using a different IDE.

Ensure that you use the `TypeChatSharp.sln` solution.

## Step 2: Set startup project to the example project you want to run

The example projects are all listed in the `Examples` solution folder. 

## Step 3: Configure secrets

Currently, the examples are running on Azure OpenAI endpoints. Adding OpenAI API support should not be too difficult, but I don't have an account right now.

To use an Azure OpenAI endpoint, add the following secrets (or add them by selecting "Manage user secrets"):
```
cd ./Examples/TypeChatSharp.Example.EXAMPLE_NAME
dotnet user-secrets set "AZURE_OPENAI_ENDPOINT" "https://YOUR_RESOURCE_NAME.openai.azure.com/openai/deployments/YOUR_DEPLOYMENT_NAME/chat/completions?api-version=2023-05-15"
dotnet user-secrets set "AZURE_OPENAI_API_KEY" "YOUR_AZURE_OPENAI_API_KEY"
```

All example projects share the same user secrets ID. So you only have to set them up once to be able to run all the examples.

## Step 4: Run the examples

Note that there are various sample "prose" files (e.g. `input.txt`) provided in each `src` directory that can give a sense of what you can run.