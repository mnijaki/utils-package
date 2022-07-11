using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;

namespace MarekNijaki.Utils.PackageSystem.Editor
{
	public static class Packages
	{
		public static async Task ReplaceDefaultManifestWithTemplateFromGist(string id, string user = "mnijaki")
		{
			string gistUrl = GetGistUrl(id, user);
			string manifestFileContent = await GetContentOfWebFile(gistUrl);
			ReplaceManifestFile(manifestFileContent);
		}

		public static void InstallUnityPackage(string packageName)
		{
			UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
		}

		private static string GetGistUrl(string id, string user = "mnijaki")
		{
			return $"https://gist.githubusercontent.com/{user}/{id}/raw";
		}

		private static async Task<string> GetContentOfWebFile(string fileURL)
		{
			using HttpClient connection = new();
			HttpResponseMessage response = await connection.GetAsync(fileURL);
			string fileContent = await response.Content.ReadAsStringAsync();
			return fileContent;
		}

		private static void ReplaceManifestFile(string manifestFileContent)
		{
			string pathToManifestFile = Path.Combine(Application.dataPath, "../Packages/manifest.json");
			File.WriteAllText(pathToManifestFile, manifestFileContent);
			// Force PackageManager to resolve packages (install new packages and uninstall removed).
			UnityEditor.PackageManager.Client.Resolve();
		}
	}
}