using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    // Start is called before the first frame update
    public static CameraShake Instance { get; private set; }
    private CinemachineVirtualCamera cvc;
    private float shakeTimer;
    private float shakeTimerTotal;

    private float startIntensity;

    private bool isRough;
    private void Awake()
    {
        Instance = this;
        cvc = GetComponent<CinemachineVirtualCamera>();

    }
    

    // Update is called once per frame
    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cbmcp = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            if (isRough)
            {
                if (shakeTimer <= 0f)
                {

                    cbmcp.m_AmplitudeGain = 0f;
                }
            } else
            {
                cbmcp.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0, 1- (shakeTimer / shakeTimerTotal));

            }
        }
    }

    public void ShakeCamera(float intensity, float time, bool isRough)
    {
        CinemachineBasicMultiChannelPerlin cbmcp = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        shakeTimer = time;
        shakeTimerTotal = time;
        cbmcp.m_AmplitudeGain = intensity;
        startIntensity = intensity;
        this.isRough = isRough;
    }


}
