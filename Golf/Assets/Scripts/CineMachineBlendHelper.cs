using UnityEngine;
using Cinemachine;
using System;
public class CineMachineBlendHelper : MonoBehaviour
{
    public event Action<ICinemachineCamera> onCameraBlendStarted;
    public event Action<ICinemachineCamera> onCameraBlendFinished;
    [SerializeField] CinemachineBrain cineMachineBrain;
    private bool wasBlendingLastFrame;

    void Update()
    {
        if (cineMachineBrain.IsBlending)
        {
            if (!wasBlendingLastFrame)
            {
                onCameraBlendStarted?.Invoke(cineMachineBrain.ActiveVirtualCamera);
            }
            wasBlendingLastFrame = true;
        }
        else
        {
            if (wasBlendingLastFrame)
            {
                onCameraBlendFinished?.Invoke(cineMachineBrain.ActiveVirtualCamera);
                wasBlendingLastFrame = false;
            }
        }
    }
}

