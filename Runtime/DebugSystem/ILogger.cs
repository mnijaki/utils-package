using System;
using Object = UnityEngine.Object;

namespace MarekNijaki.Utils.DebugSystem
{
	public interface ILogger
	{
		void Log(object message, Object sender = null);

		void LogWarning(object message, Object sender = null);

		void LogError(object message, Object sender = null);

		void LogException(Exception exception, Object sender = null);
	}
}