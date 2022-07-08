using System.IO;
using UnityEditor;
using UnityEngine;

namespace MarekNijaki.Utils.Editor
{
    public static class Tools
    {
        [MenuItem("Tools/Setup/Create default folders")]
        public static void CreateDefaultFolders()
        {
            CreateFolders("_DefaultFolders", "Art", "Audio", "Data", "Prefabs", "Scripts");
            CreateFolders("_DefaultFolders/Audio", "AudioSFX", "Dialogues", "Music");
            CreateFolders("_DefaultFolders/Art", "Materials", "Models", "Sprites", "Textures");
            // Refresh is needed so new folders appear in editor.
            AssetDatabase.Refresh();
        }

        private static void CreateFolders(string rootFolder, params string[] foldersToCreate)
        {
            // Application.dataPath returns path to "Assets" directory.
            string fullPath = Path.Combine(Application.dataPath, rootFolder);
            foreach(string folderName in foldersToCreate)
            {
                Directory.CreateDirectory(Path.Combine(fullPath, folderName));
            }
        }
    }
}
