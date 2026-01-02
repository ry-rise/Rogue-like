using UnityEngine;

public class ScreenSizeInit : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(1024, 576, FullScreenMode.Windowed, new RefreshRate { numerator = 60, denominator = 1 });
    }

}
