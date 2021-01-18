using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject FreeLookCamera, LockTargetCamera;
    private float LanCuoiChuyenCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSet.Player.LockTarget == false)
        {
            LockTargetCamera.active = false;
            if (Camera.main.GetComponent<Cinemachine.CinemachineBrain>().IsBlending == false)
            {
                FreeLookCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_InputAxisName = "Mouse X";
                FreeLookCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_InputAxisName = "Mouse Y";
            }
            else
            {
                FreeLookCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.Value = 0;
                FreeLookCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.Value = .5f;
            }
            FreeLookCamera.active = true;
        }
        else
        {
            LockTargetCamera.active = true;
            FreeLookCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_XAxis.m_InputAxisName = null;
            FreeLookCamera.GetComponent<Cinemachine.CinemachineFreeLook>().m_YAxis.m_InputAxisName = null;
            FreeLookCamera.active = false;
        }
    }
}
