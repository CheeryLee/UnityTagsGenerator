# UnityTagsGenerator

[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This tool allows to generate compile time source file with tag names of your Unity project.

Tested with Unity 2019+, but it should work from 2017.3+ too.

## How to use

By default there are some tags that Unity provides out-of-box. They will be created even if you don't set something in Tag Manager.

1. after every asset related operation there would be automatically writed a new file called Tags.cs in folder **Assets/generated** with **Tags** class inside:

```csharp
public static class Tags
{
	public const string Untagged = "Untagged";
	public const string Respawn = "Respawn";
	public const string Finish = "Finish";
	public const string EditorOnly = "EditorOnly";
	public const string MainCamera = "MainCamera";
	public const string Player = "Player";
	public const string GameController = "GameController";
}
```

2. use it wherever you want instead of string literals:

```csharp
void OnTriggerEnter(Collider other)
{
    // whoa, what a mess ...
    if (other.gameObject.CompareTag("Player")) { ... }

    // let's do it with our new killing spree feature
    if (other.gameObject.CompareTag(Tags.Player)) { ... }
}
```

## How does it work?

Everything is pretty simple. After every asset importing operation (save and load are also included) the generator will create an appropriate source file that contains tag IDs. The variables names correspond to real tag names, except spaces and some service chars.

## Pros & cons

The main reason why it was created is take into account the proof-of-concept that this feature is possible to do.

### Pros:

1. it works like the ID generator in Android native Java projects. It means you should not use string literals to work with tags functionality in Unity. Just take what you've got from development environment;
2. based on the previous one, you now can see compile time errors if there are missing tags after they were changed in Tag Manager;
3. IDE hints (who doesn't love IntelliSense? :upside_down_face: );
4. small memory footprint: every variables are const like.

### Cons:

1. tags are still strings under the hood. There is no any magic way to change the basic behaviour of Unity. The rule of using **CompareTag** method instead of direct comparing is still actual.
2. even if you can type every tag name what you want, you can't use it as variable name. That's why we have restrictions to letters, digits and underscore char.