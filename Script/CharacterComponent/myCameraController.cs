using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class myCameraController : MonoBehaviour
{

    E_CAMERA_TYPE m_CameraType;

    GameObject m_TargetObject;

    [SerializeField]
    float m_UpSize = 4.0f;

    [SerializeField]
    float m_DistanceSize = 7.5f;

    [SerializeField]
    Vector3 m_shopOffset = Vector3.one;


    public void SetCameraMode(E_CAMERA_TYPE CamMode)
    {
        m_CameraType = CamMode;
    }

    public void SetTarget(GameObject target)
    {
        m_TargetObject = target;
        transform.SetParent(target.transform);
        transform.localPosition = new Vector3(0, 9, -5);
        transform.localRotation = Quaternion.Euler(60, 0, 0);
        transform.SetParent(null);
    }

    private void Update()
    {
        transform.position = m_TargetObject.transform.position +  new Vector3(0, 9, -5) * 0.8f;
    }


    public void PlusDistance(float height, float distance)
    {
        m_UpSize += height;
        m_DistanceSize += distance;
    }

    public void SetDistance(float height, float distance)
    {
        m_UpSize = height;
        m_DistanceSize = distance;
    }

}
