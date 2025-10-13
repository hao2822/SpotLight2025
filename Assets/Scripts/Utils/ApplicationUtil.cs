using System.Linq;
using UnityEngine;

namespace Utils
{
    public class ApplicationUtil
    {
        public static readonly string[] DesktopResolutions = new string[]
        {
            "1280x720",
            "1920x1080",
            "2560x1440",
        };
        public static void SwitchToFullScreen(bool fullScreen)
        {
#if UNITY_STANDALONE_WIN
            Screen.fullScreenMode = fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.fullScreen = fullScreen;
#endif
        }
        
        public static void SetScreenResolution(int index, bool fullScreen)
        {
#if UNITY_STANDALONE_WIN
            // Screen.fullScreenMode = FullScreenMode.Windowed;
            if (index < 0 || index >= DesktopResolutions.Length)
            {
                Debug.LogError($"Invalid resolution index: {index}");
                return;
            }

            string[] resolution = DesktopResolutions[index].Split('x');
            if (resolution.Length != 2)
            {
                return;
            }
            int width = int.Parse(resolution[0]);
            int height = int.Parse(resolution[1]);
            Screen.SetResolution(width, height, fullScreen);
            LocalStorageUtil.SetInt(LocalStorageKeys.ScreenResolution,index);
#endif
        }
        public static string Base26Encode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError($"Invalid input string: {input}");
                return null;
            }

            return input.Select(x => x + 26).Aggregate("", (current, x) => current + (char) x);
        }

        public static string Base26Decode(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                Debug.LogError($"Invalid input string");
                return null;
            }

            return input.Select(x => x - 26).Aggregate("", (current, x) => current + (char) x);
        }
        public static void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
    
    
}