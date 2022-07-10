using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace MarekNijaki.Utils.Editor
{
    public static class Tools
    {
        private static bool _isToolInitialized;
        private static string directoryPathToCurrentlySelectedFile;
        private static string _nextUndoGroupName;
        private static int _undoQueueCounter;
        private static string _undoQueueKey = "";
        private static readonly Dictionary<string, List<string>> _undoQueue = new();

        [MenuItem("Tools/Setup/Create default folders")]
        public static void CreateDefaultFolders()
        {
            if(!_isToolInitialized)
            {
                _isToolInitialized = true;
                Undo.undoRedoPerformed += UndoRedoPerformed;
            }
            
            HandleCustomUndoBeforeOperation();
            GetDirectoryPathToCurrentlySelectedFile();
            CreateFolders(string.Empty, "Art", "Audio", "Data", "Prefabs", "Scripts");
            CreateFolders("Scripts", "Runtime", "Editor");
            CreateFolders("Audio", "AudioSFX", "Dialogues", "Music");
            CreateFolders("Art", "Materials", "Models", "Sprites", "Textures");
            HandleCustomUndoAfterOperation();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        
        private static void UndoRedoPerformed()
        {
            if(!string.IsNullOrWhiteSpace(_nextUndoGroupName) && _nextUndoGroupName.StartsWith("Undo CreateFolders"))
            {
                foreach(string path in _undoQueue[_undoQueueKey])
                    AssetDatabase.DeleteAsset(path); 
                _undoQueue.Remove(_undoQueueKey);
                _undoQueueCounter--;
                AssetDatabase.Refresh();
            }
            
            _nextUndoGroupName = Undo.GetCurrentGroupName();
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
                _undoQueue[_undoQueueKey].Add(Path.Combine(path, folderName));
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

        private static void HandleCustomUndoBeforeOperation()
        {
            _undoQueueCounter++;
            _undoQueueKey = "Undo CreateFolders" + _undoQueueCounter;
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(_undoQueueKey);
            _undoQueue.Add(_undoQueueKey, new List<string>());
        }

        private static void HandleCustomUndoAfterOperation()
        {
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("");
        }
    }
}
