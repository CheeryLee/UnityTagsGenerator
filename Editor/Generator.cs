// MIT License
//
// Copyright (c) 2022 - 2023 Alexander Pluzhnikov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;
using UnityEngine;

namespace UnityTagsGenerator.Editor
{
    internal static class Generator
    {
        internal static void Run()
        {
            var tags = UnityEditorInternal.InternalEditorUtility.tags;
            var layers = GetLayers();
            
            CreateSourceFile(tags, layers);
            CompilationPipeline.RequestScriptCompilation();
        }
        
        private static void CreateSourceFile(IEnumerable<string> tags, IReadOnlyDictionary<int, string> layers)
        {
            var currentDirectory = Path.GetDirectoryName(GetCallerPath());

            if (string.IsNullOrEmpty(currentDirectory))
                throw new Exception("Can't get parent directory of caller path");
            
            var generatedDirectoryPath = Path.Combine(Application.dataPath, "generated");
            var templateText = File.ReadAllText(Path.Combine(currentDirectory, "../Template~/Tags.template.cs"));
            var compiledTags = "";
            var compiledLayers = "";

            if (!Directory.Exists(generatedDirectoryPath))
                Directory.CreateDirectory(generatedDirectoryPath);

            foreach (var tag in tags)
            {
                var varName = ValidateVariableName(tag);
                compiledTags += $"\tpublic const string {varName} = \"{tag}\";\n";
            }
            
            foreach (var pair in layers)
            {
                var varName = ValidateVariableName(pair.Value);
                compiledLayers += $"\tpublic const int {varName} = {pair.Key};\n";
            }
            
            var compiledTemplateText = string.Format(templateText, compiledTags, compiledLayers);
            File.WriteAllText(Path.Combine(generatedDirectoryPath, "Tags.cs"), compiledTemplateText);
        }

        private static string ValidateVariableName(string name)
        {
            var varName = "";
            var matches = Regex.Matches(name, "[A-Za-z0-9_]+");

            for (var i = 0; i < matches.Count; i++)
            {
                if (i > 0)
                    varName += "_";
                varName += matches[i].Value;
            }

            return varName;
        }
        
        private static IReadOnlyDictionary<int, string> GetLayers()
        {
            const int layersCount = 32;
            var layers = new Dictionary<int, string>();
            var equalKeys = new List<int>();

            for (var i = 0; i < layersCount; i++)
            {
                var name = LayerMask.LayerToName(i);
                
                if (!string.IsNullOrEmpty(name))
                    layers.Add(i, name);
            }

            foreach (var pair in layers)
            {
                var equalFound = false;

                if (equalKeys.Contains(pair.Key))
                    continue;
                
                foreach (var innerPair in layers)
                {
                    if (pair.Key == innerPair.Key)
                        continue;

                    if (pair.Value != innerPair.Value)
                        continue;
                    
                    if (!equalFound)
                    {
                        equalKeys.Add(pair.Key);
                        equalFound = true;
                    }
                        
                    equalKeys.Add(innerPair.Key);
                }
            }

            foreach (var key in equalKeys)
                layers[key] += $"_{key}";

            return layers;
        }

        private static string GetCallerPath([CallerFilePath] string path = "")
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("Caller path can't be empty or null");

            return path;
        }
    }
}