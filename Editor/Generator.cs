using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEditor.Compilation;

namespace UnityTagsGenerator
{
    [InitializeOnLoad]
    internal static class Generator
    {
        static Generator()
        {
            CompilationPipeline.compilationFinished -= Run;
            CompilationPipeline.compilationFinished += Run;
        }

        private static void Run(object value)
        {
            CreateSourceFile(UnityEditorInternal.InternalEditorUtility.tags);
        }
        
        private static void CreateSourceFile(IEnumerable<string> tags)
        {
            var generatedPath = Path.Combine(Application.dataPath, "generated");
            var templateText = File.ReadAllText(Path.Combine(GetCallerPath(), "../Template~/Tags.template.cs"));
            var compiledTags = "";

            if (!Directory.Exists(generatedPath))
                Directory.CreateDirectory(generatedPath);

            foreach (var tag in tags)
                compiledTags += $"\tpublic const string {tag} = \"{tag}\",\n";

            var compiledTemplateText = string.Format(templateText, compiledTags);
            File.WriteAllText(Path.Combine(generatedPath, "Tags.cs"), compiledTemplateText);
        }

        private static string GetCallerPath([CallerFilePath] string path = "")
        {
            if (string.IsNullOrEmpty(path))
                throw new Exception("Caller path can't be empty or null");

            return path;
        }
    }
}