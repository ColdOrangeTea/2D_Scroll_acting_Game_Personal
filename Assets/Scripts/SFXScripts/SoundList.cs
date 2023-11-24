using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public static partial class AudioType { }

[CreateAssetMenu(menuName = "SO/AudioPair")]
public class SoundList : ScriptableObject
{
    public List<ClipTag> pairs = new List<ClipTag>();
    private void OnValidate()
    {
        string generatedTag = "public static partial class AudioType{";
        generatedTag += "public enum tags { ";
        for (int i = 0; i < pairs.Count; i++)
        {
            generatedTag += pairs[i].tag + ",";
        }

        generatedTag += "} }";


        string relativePath = Application.dataPath + @"/Scripts/Runtime/";
        string fileName = "AudioClipTags.cs";
        string tempPath = relativePath + fileName;

        if (!Directory.Exists(relativePath))
        {
            Directory.CreateDirectory(relativePath);
        }
        using (StreamWriter file = new StreamWriter(tempPath))
        {
            file.WriteLine(generatedTag);
        }

        AssetDatabase.ImportAsset(tempPath);
    }
}

[System.Serializable]
public class ClipTag
{
    public string tag;
    public AudioClip clip;
}