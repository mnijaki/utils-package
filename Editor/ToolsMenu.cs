using System.IO;
using UnityEditor;
using UnityEngine;

namespace MarekNijaki.Utils.Editor
{
    public static class Tools
    {
        private static string directoryPathToCurrentlySelectedFile;
        
        [MenuItem("Tools/Setup/Create default folders")]
        public static void CreateDefaultFolders()
        {
            GetDirectoryPathToCurrentlySelectedFile();
            CreateFolders(string.Empty, "Art", "Audio", "Data", "Prefabs", "Scripts");
            CreateFolders("Scripts", "Runtime", "Editor");
            CreateFolders("Audio", "AudioSFX", "Dialogues", "Music");
            CreateFolders("Art", "Materials", "Models", "Sprites", "Textures");
            // Refresh is needed so new folders appear in editor.
            AssetDatabase.Refresh();
        }

        private static void CreateFolders(string rootFolder, params string[] foldersToCreate)
        {
            string fullPath = Path.Combine(directoryPathToCurrentlySelectedFile, rootFolder);
            foreach(string folderName in foldersToCreate)
            {
                Directory.CreateDirectory(Path.Combine(fullPath, folderName));
            }
        }

        private static void GetDirectoryPathToCurrentlySelectedFile()
        {
            // "Application.dataPath" returns path to "Assets" directory.
            directoryPathToCurrentlySelectedFile = Application.dataPath;    
            
            Object[] selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            foreach(Object selectedObject in selectedObjects)
            {
                string path = AssetDatabase.GetAssetPath(selectedObject);
                if(string.IsNullOrEmpty(path))
                {
                    continue;
                }
                
                directoryPathToCurrentlySelectedFile = File.Exists(path) ? Path.GetDirectoryName(path) : path;
                break;
            }
        }
    }
}
