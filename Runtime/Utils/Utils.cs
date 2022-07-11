using MarekNijaki.Utils.DebugSystem;
using ILogger = MarekNijaki.Utils.DebugSystem.ILogger;
using UnityEngine;

namespace MarekNijaki.Utils
{
	public class Utils : MonoBehaviour
	{
		[SerializeField]
		private LogProvider _logProvider;

		private ILogger _logger;

		private void Awake()
		{
			_logger = LogProvider.GetLogger(_logProvider);
			
			_logger.Log("Some message");
		}
	}
}
