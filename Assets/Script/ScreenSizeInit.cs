using UnityEngine;

public class ScreenSizeInit : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(1024, 576, false, 60);
    }

}
