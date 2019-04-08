using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntButtonScript : MonoBehaviour
{

    public GameObject m_Ant;
    public CameraControll m_Camera;
    public GameObject m_PathPointPrefab;
    List<GameObject> m_Path;

    private bool m_PathIsActive = false;



    /// <summary>
    /// turn on/off path to resource and set ant as cameraTarget
    /// </summary>
    public void SwitchAntPath()
    {        
        if (!m_PathIsActive)
        {
            m_Camera = FindObjectOfType<CameraControll>();
            m_Camera.SetTarget(m_Ant);
            ShowAntPath();
            
        }
        else
        {
            HideAntPath();

        }
    }


    /// <summary>
    /// Hide Antpath
    /// </summary>
    private void HideAntPath()
    {
        if (m_Path != null)
        {
            for(int i = 0; i < m_Path.Count; i++)
            {
                Destroy(m_Path[i]);
            }
            m_Path = null;
            m_PathIsActive = false;
        }
    }


    /// <summary>
    /// show Antpath
    /// </summary>
    public void ShowAntPath()
    {
        List<Vector3> waypoints = m_Ant.GetComponent<ScouterAnt>().m_Path;
        m_Path = new List<GameObject>();

        for(int i=0; i<waypoints.Count;i++)
        {
            GameObject tmp = Instantiate(m_PathPointPrefab, waypoints[i], Quaternion.identity);
            if (i < waypoints.Count - 1)
            {
                tmp.transform.LookAt(waypoints[i + 1]);
            }
            m_Path.Add(tmp);
        }
        m_PathIsActive = true;
    }


    /// <summary>
    /// if this gameobject is destroyed, destroy the possible shown antpath too
    /// </summary>
    private void OnDestroy()
    {
        HideAntPath();
    }
}
