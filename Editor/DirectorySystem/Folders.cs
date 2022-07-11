using System.IO;
using UnityEditor;
using UnityEngine;

namespace MarekNijaki.Utils.DirectorySystem.Editor
{
	public static class Folders
	{
		private static string _rootFolder;

		public static void CreateFolders(string parentFolder, params string[] foldersToCreate)
		{
			if(foldersToCreate == null || foldersToCreate.Length < 1)
			{
				return;
			}

			string path = Path.Combine(_rootFolder, parentFolder);
			foreach(string folderName in foldersToCreate)
			{
				Directory.CreateDirectory(Path.Combine(path, folderName));
				DirUndo.Record(Path.Combine(path, folderName));
			}
		}

		public static void SetRootFolderAsCurrentSelection()
		{
			_rootFolder = "Assets";
			Object[] selectedObjects = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
			foreach(Object selectedObject in selectedObjects)
			{
				string path = AssetDatabase.GetAssetPath(selectedObject);
				if(string.IsNullOrEmpty(path))
				{
					continue;
				}

				_rootFolder = File.Exists(path) ? Path.GetDirectoryName(path) : path;
				break;
			}
		}
	}
}