WARNING: This project is no longer being maintained, as there is [a good alternative that is maintained by Microsoft]

---

(https://github.com/microsoft/typechat.net) now.

![publish to nuget](https://github.com/hermanussen/TypeChatSharp/workflows/Build%20and%20Publish%20Nuget%20Package/badge.svg) [![Nuget](https://img.shields.io/nuget/v/TypeChatSharp)](https://www.nuget.org/packages/TypeChatSharp/) [![Nuget](https://img.shields.io/nuget/dt/TypeChatSharp?label=nuget%20downloads)](https://www.nuget.org/packages/TypeChatSharp/) [![Twitter URL](https://img.shields.io/twitter/url?style=social&url=https%3A%2F%2Ftwitter.com%2Fknifecore%2F)](https://twitter.com/knifecore)

# TypeChatSharp

TypeChatSharp is a library that makes it easy to build natural language interfaces using types in C#. It is a direct port from [TypeChat](https://github.com/microsoft/TypeChat), but is not maintained by Microsoft.

> Much of the text below and in other places in this repository is a direct copy or slightly modified version of text in the original [TypeChat](https://github.com/microsoft/TypeChat) repository. The copyright for these texts belongs to Microsoft.

Building natural language interfaces has traditionally been difficult. These apps often relied on complex decision trees to determine intent and collect the required inputs to take action. Large language models (LLMs) have made this easier by enabling us to take natural language input from a user and match to intent. This has introduced its own challenges including the need to constrain the model's reply for safety, structure responses from the model for further processing, and ensuring that the reply from the model is valid. Prompt engineering aims to solve these problems, but comes with a steep learning curve and increased fragility as the prompt increases in size.

TypeChatSharp, like TypeChat, replaces _prompt engineering_ with _schema engineering_.

Simply define types that represent the intents supported in your natural language application. That could be as simple as a type for categorizing sentiment or more complex examples like types for a shopping cart or music application. For example, to add additional intents to a schema, a developer can add additional types.

After defining your types, TypeChat takes care of the rest by:

1. Constructing a prompt to the LLM using types.
2. Validating the LLM response conforms to the schema. If the validation fails, repair the non-conforming output through further language model interaction.
3. Summarizing succinctly (without use of a LLM) the instance and confirm that it aligns with user intent.

Types are all you need!

# Getting Started

Install TypeChatSharp:

```
dotnet add package TypeChatSharp
```

You can also build TypeChatSharp from source:

```
cd TypeChatSharp
dotnet build
```

To see TypeChat in action, we recommend exploring the [Example projects](./examples).

To learn more about TypeChat, visit the [documentation](https://microsoft.github.io/TypeChat) which includes more information on TypeChat and how to get started. Much of it will also apply to TypeChatSharp.