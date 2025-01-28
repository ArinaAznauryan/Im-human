using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;
    public float amplitudeGain, frequencyGain;
    
    void Start() {
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();
    }
    
    public void Noise(/*float amplitudeGain, float frequencyGain*/) {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;    
    }

    void Update() {
        Noise();
    }
}
