using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeButtonScript : MonoBehaviour
{
    public CupcakeDataScript m_CupcakeData;
    public GameObject m_CupcakeDataPanel;
    public GameObject m_CameraRig;

    /// <summary>
    /// set this Cupcake to center of camera
    /// </summary>
    public void GoToCupcake()
    {
        m_CameraRig.transform.position = m_CupcakeData.transform.position;
    }


    /// <summary>
    /// Set the datapanel of this cupcake active
    /// </summary>
    public void SetDataPanelActive()
    {
        m_CupcakeData.m_Base.DeactiveAllDataPanel();
        m_CupcakeDataPanel.SetActive(true);
        m_CupcakeDataPanel.GetComponent<CupcakeDataPanelScript>().SetDistanceToBase();
        m_CupcakeDataPanel.GetComponent<CupcakeDataPanelScript>().SetTakenFood();
    }

}
