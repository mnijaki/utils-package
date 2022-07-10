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
            DirUndo.PrepareForRecording();
            
            GetDirectoryPathToCurrentlySelectedFile();
            CreateFolders(string.Empty, "Art", "Audio", "Data", "Prefabs", "Scripts");
            CreateFolders("Scripts", "Runtime", "Editor");
            CreateFolders("Audio", "AudioSFX", "Dialogues", "Music");
            CreateFolders("Art", "Materials", "Models", "Sprites", "Textures");
            
            DirUndo.FinishRecording();
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private static void CreateFolders(string rootFolder, params string[] foldersToCreate)
        {
            if(foldersToCreate == null || foldersToCreate.Length < 1)
            {
                return;
            }
            
            string path = Path.Combine(directoryPathToCurrentlySelectedFile, rootFolder);
            foreach(string folderName in foldersToCreate)
            {
                Directory.CreateDirectory(Path.Combine(path, folderName));
                DirUndo.Record(Path.Combine(path, folderName));
            }
        }

        private static void GetDirectoryPathToCurrentlySelectedFile()
        {
            directoryPathToCurrentlySelectedFile = "Assets";
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
