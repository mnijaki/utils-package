using UnityEditor;

namespace MarekNijaki.Utils.Editor
{
	public static class Tools
	{
		[MenuItem("Tools/Setup/Create default folders")]
		public static void CreateDefaultFolders()
		{
			DirUndo.PrepareForRecording();

			Folders.SetRootFolderAsCurrentSelection();
			Folders.CreateFolders(string.Empty, "Art", "Audio", "Data", "Prefabs", "Scripts");
			Folders.CreateFolders("Scripts", "Runtime", "Editor");
			Folders.CreateFolders("Audio", "AudioSFX", "Dialogues", "Music");
			Folders.CreateFolders("Art", "Materials", "Models", "Sprites", "Textures");

			DirUndo.FinishRecording();

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		[MenuItem("Tools/Setup/Packages/Load URP 3D manifest template file from Gist")]
		public static async void SetDefaultManifest()
		{
			await Packages.ReplaceDefaultManifestWithTemplateFromGist("58db83bc2ffeece1639ad1d38f25d6eb");
		}
		
		[MenuItem("Tools/Setup/Packages/Install Cinemachine")]
		public static void InstallCinemachine()
		{
			Packages.InstallUnityPackage("cinemachine");
		}

		[MenuItem("Tools/Setup/Packages/Install New Input System")]
		public static void InstallNewInputSystem()
		{
			Packages.InstallUnityPackage("inputsystem");
		}
		
		[MenuItem("Tools/Setup/Packages/Install Post Processing")]
		public static void InstallPostProcessing()
		{
			Packages.InstallUnityPackage("postprocessing");
		}
	}
}