using System;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace MarekNijaki.Utils.DebugSystem
{
    [AddComponentMenu(".Tools/Services/Debug/Logger", 1)]
    public class Logger : MonoBehaviour, ILogger
    {
        #if UNITY_EDITOR
        [Header("Settings:")]
        [SerializeField]
        private bool _isLoggingEnabled = true;
        #else
        private bool _isLoggingEnabled = false; 
        #endif
        [SerializeField]
        private string _prefix;
        [SerializeField]
        private Color _prefixColor = Color.white;

        private string _hexColor;

        private void OnValidate()
        {
            _hexColor = $"#{ColorUtility.ToHtmlStringRGB(_prefixColor)}";

            if(string.IsNullOrEmpty(_prefix))
                _prefix = gameObject.name;
        }

        public void Log(object message, Object sender = null)
        {
            if(!_isLoggingEnabled)
            {
                return;
            }
            
            Debug.Log($"<b><color={_hexColor}>[{_prefix}]:</color></b> {message}", sender);
        }
        
        public void LogWarning(object message, Object sender = null)
        {
            if(!_isLoggingEnabled)
            {
                return;
            }
            
            Debug.LogWarning($"<b>[{_prefix}]:</b> {message}", sender);
        }

        public void LogError(object message, Object sender = null)
        {
            if(!_isLoggingEnabled)
            {
                return;
            }
            
            Debug.LogError($"<b>[{_prefix}]:</b> {message}", sender);
        }

        public void LogException(Exception exception, Object sender = null)
        {
            if(!_isLoggingEnabled)
            {
                return;
            }
            
            Debug.LogException(exception, sender);
        }
    }
}
