using System.Collections.Generic;
using UnityEditor;

namespace MarekNijaki.Utils.Editor
{
    public static class DirUndo
    {
        private static bool _isInitialized;
        private const string _CREATE_DIRECTORY_PREFIX = "Undo CreateFolders";
        private static string _nextUndoGroupName;
        private static int _undoQueueCounter;
        private static string _undoQueueKey = "";
        private static readonly Dictionary<string, List<string>> _undoQueue = new();
        
        public static void PrepareForRecording()
        {
            TryInitializeDirectoryUndo();
            
            _undoQueueCounter++;
            _undoQueueKey = _CREATE_DIRECTORY_PREFIX + _undoQueueCounter;
            _undoQueue.Add(_undoQueueKey, new List<string>());
            
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName(_undoQueueKey);
        }

        public static void Record(string pathToCreatedDirectory)
        {
            if(string.IsNullOrEmpty(_undoQueueKey) || (!_undoQueue.ContainsKey(_undoQueueKey)))
            {
                return;
            }
            
            _undoQueue[_undoQueueKey].Add(pathToCreatedDirectory);
        }
        
        public static void FinishRecording()
        {
            // Additional incrementation of group, so user could Undo creation of directory just after he created directory.
            // Without this there will be no cached information(_nextUndoGroupName) for the first Undo operation.
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName("");
        }

        private static void TryInitializeDirectoryUndo()
        {
            if(_isInitialized)
            {
                return;
            }
            
            // Make sure to not subscribe twice to delegate.
            Undo.undoRedoPerformed -= UndoRedoPerformed;
            Undo.undoRedoPerformed += UndoRedoPerformed;

            _isInitialized = true;
        }
        
        private static void UndoRedoPerformed()
        {
            TryUndoDirectoryCreation();
            _nextUndoGroupName = Undo.GetCurrentGroupName();
        }

        private static bool IsInformationAboutUndoOperationAvailable()
        {
            return !string.IsNullOrWhiteSpace(_nextUndoGroupName);
        }

        private static bool IsDirectoryCreationOperationInUndoQueue()
        {
            return !string.IsNullOrWhiteSpace(_undoQueueKey) && (_undoQueue.ContainsKey(_undoQueueKey));
        }

        private static bool IsUndoOperationAssociatedWithDirectoryCreation()
        {
            return _nextUndoGroupName.StartsWith(_CREATE_DIRECTORY_PREFIX);
        }

        private static void TryUndoDirectoryCreation()
        {
            if(!IsInformationAboutUndoOperationAvailable())
            {
                return;
            }

            if(!IsDirectoryCreationOperationInUndoQueue())
            {
                return;
            }

            if(!IsUndoOperationAssociatedWithDirectoryCreation())
            {
                return;
            }
            
            foreach(string path in _undoQueue[_undoQueueKey])
                AssetDatabase.DeleteAsset(path); 
            
            _undoQueue.Remove(_undoQueueKey);
            _undoQueueCounter--;
            
            AssetDatabase.Refresh();
        }
    }
}
