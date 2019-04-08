using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {

	public Camera m_Camera;
	public Transform m_CameraRig;
    public GameObject m_Target;

	public float m_ZoomSpeed = 40f;
	public float m_RotatingSpeed = 40f;
	public float m_MoveSpeed = 40f;

	public float m_defaultZoom = 15;
	public float m_MaxZoomIn = 5f;
	public float m_MaxZoomOut = 50f;


    Vector3 m_LocalRotation;

	private bool m_CanRotate = true;

    private void Start () {

		m_Camera.orthographicSize = m_defaultZoom;
        m_LocalRotation = m_CameraRig.transform.rotation.eulerAngles;
	}

    // handle cameramovement and rotation
    private void Update () {

        if (m_Target != null) //follow given target
        {
            m_CameraRig.transform.position = m_Target.transform.position;
            if (Input.anyKey)
            {
                m_Target = null;
            }
        }
        else
        {

            //Movement
            //Horizontal
            if (Input.GetAxisRaw("Horizontal") < 0 || Input.mousePosition.x == 0)
            {
                m_CameraRig.transform.Translate(-m_MoveSpeed * Time.deltaTime, 0f, 0f);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0 || Input.mousePosition.x == Screen.width - 1)
            {
                m_CameraRig.transform.Translate(m_MoveSpeed * Time.deltaTime, 0f, 0f);
            }
            //Vertical
            if (Input.GetAxisRaw("Vertical") < 0 || Input.mousePosition.y == 0)
            {
                m_CameraRig.transform.Translate(0f, 0f, -m_MoveSpeed * Time.deltaTime);
            }
            else if (Input.GetAxisRaw("Vertical") > 0 || Input.mousePosition.y == Screen.height - 1)
            {
                m_CameraRig.transform.Translate(0f, 0f, m_MoveSpeed * Time.deltaTime);
            }

            m_CameraRig.transform.position = new Vector3(m_CameraRig.transform.position.x, 0, m_CameraRig.transform.position.z);

            //Zooming in/out Perspective
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                if (transform.position.y < m_MaxZoomOut)
                {
                    transform.Translate(0f, 0f, -m_ZoomSpeed * Time.deltaTime);
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (transform.position.y > m_MaxZoomIn)
                {
                    transform.Translate(0f, 0f, m_ZoomSpeed * Time.deltaTime);
                }
            }

            //Rotating
            if (Input.GetKey("mouse 2") && m_CanRotate)
            {

                m_LocalRotation.x += m_RotatingSpeed * Input.GetAxis("Mouse X") * Time.deltaTime;
                m_LocalRotation.y += m_RotatingSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;

                if (m_LocalRotation.y < -15f)
                    m_LocalRotation.y = -15f;
                else if (m_LocalRotation.y > 35f)
                    m_LocalRotation.y = 35f;



                m_CameraRig.rotation = Quaternion.Euler(m_LocalRotation.y, m_LocalRotation.x, 0);

                float rotationZ = m_CameraRig.transform.eulerAngles.z;
                m_CameraRig.transform.Rotate(0, 0, -rotationZ);
            }
        }
	}


    /// <summary>
    /// turn on/off rotation
    /// </summary>
    /// <param name="_CanRotate"></param>
	public void SetRotationAllowed(bool _CanRotate){
		m_CanRotate = _CanRotate;
	}


    /// <summary>
    /// set target for camera
    /// </summary>
    /// <param name="_target"></param>
    public void SetTarget(GameObject _target)
    {
        m_Target = _target;
    }
}
