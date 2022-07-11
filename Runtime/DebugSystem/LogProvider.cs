using UnityEngine;

namespace MarekNijaki.Utils.DebugSystem
{
	public class LogProvider : MonoBehaviour
	{
		private ILogger _logger;
		private static ILogger _defaultLogger;

		public static ILogger GetLogger(LogProvider logProvider)
		{
			if(logProvider != null)
			{
				if(logProvider.GetLoggerComponent())
					return logProvider._logger;
			}
			
			if(_defaultLogger == null)
			{
				GameObject _defaultLoggerGO = new("Default Logger");
				_defaultLoggerGO.AddComponent<Logger>();
				_defaultLogger = _defaultLoggerGO.GetComponent<ILogger>();
			}
			
			return _defaultLogger;
		}
		
		private bool GetLoggerComponent()
		{
			if(_logger == null)
				_logger = GetComponent<ILogger>();
			
			return _logger != null;
		}
	}
}