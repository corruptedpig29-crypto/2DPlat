using UnityEngine;

public class FPSLimiter : MonoBehaviour

{

    public int FPS = 60;

    void Start()

    {

        QualitySettings.vSyncCount = 1;

        Application.targetFrameRate = FPS;

    }
    void Update()

    {

        QualitySettings.vSyncCount = 1;

        Application.targetFrameRate = FPS;

    }

}