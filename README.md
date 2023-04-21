# UnityTagsGenerator

[![Made with Unity](https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity)](https://unity3d.com)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/CheeryLee/UnityTagsGenerator?display_name=tag&include_prereleases)

This tool allows to generate compile time source file with tag names and layer values of your Unity project.

Tested with Unity 2019+, but it should work from 2017.3+ too.

## Getting started
You can install the package directly from GitHub by using Package Manager:

![image](https://user-images.githubusercontent.com/11297752/233580399-45635f58-c37d-447d-b497-99cf05fc87d1.png)

![image](https://user-images.githubusercontent.com/11297752/233580798-0097314f-98b8-42a6-8cd5-7731c5e059c5.png)

... or add the URL to `Packages/manifest.json`:

`"com.cheerylee.unity-tags-generator": "https://github.com/CheeryLee/UnityTagsGenerator.git#1.0.0"`

This project uses the `*.*.*` release tags. So after the hashtag in URL you can specify a target version to install. For example, it may be `#1.0.0`.

## How to use

By default there are some tags and layers that Unity provides out-of-box. They will be created even if you don't set something in Tag Manager.

After every asset related operation there would be automatically writed a new file called Tags.cs in folder **Assets/generated** with **Tags** and **Layers** classes inside:

```csharp
public static class Tags
{
    public const string Untagged = "Untagged";
    public const string Respawn = "Respawn";
    ...
}

public static class Layers
{
    public const int Default = 0;
    public const int TransparentFX = 1;
    ...
}
```

Use it wherever you want instead of string literals:

```csharp
void OnTriggerEnter(Collider other)
{
    // whoa, what a mess ...
    if (other.gameObject.CompareTag("Player")) { ... }

    // let's do it with our new killing spree feature
    if (other.gameObject.CompareTag(Tags.Player)) { ... }
}
```

... or instead of bitwise operations for layer mask:

```csharp
// Layers.UI returns an appropriate value
if (Physics.Raycast(transform.position, Vector3.forward, out hit, 10, Layers.UI))
{
    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    Debug.Log("Did Hit");
}
```

In addition there are some tricky useful methods in **LayerExtensions** class:
```csharp
// don't write 1 << 3 | 1 << 5, just do this:
LayersExtensions.Where(Layers.Third, Layers.Fifth);
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