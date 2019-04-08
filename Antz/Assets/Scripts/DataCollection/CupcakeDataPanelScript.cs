using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CupcakeDataPanelScript : MonoBehaviour
{

    public CupcakeDataScript m_CupcakeData;
    public GameObject m_AntButtonPrefab;
    public GameObject m_AntButton = null;

    public Text m_DistanceToBase;
    public Text m_TakenFood;
    public Text m_ShortestPathToBase;


    /// <summary>
    /// just when active:
    /// refresh cupcakedata
    /// </summary>
    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        SetTakenFood();
        SetShortestPathToBase();
    }



    /// <summary>
    /// Updatable data
    /// </summary>
    public void UpdateData()
    {
        SetDistanceToBase();
        SetTakenFood();
        SetShortestPathToBase();
    }


    /// <summary>
    /// calculate distance to base
    /// </summary>
    public void SetDistanceToBase()
    {
        m_DistanceToBase.text = "Distance to Base: " + m_CupcakeData.m_DistanceToBase.ToString("f2");
    }


    /// <summary>
    /// set collected foodText
    /// </summary>
    public void SetTakenFood()
    {
        m_TakenFood.text = "Taken Food: " + m_CupcakeData.m_TakenFood;
    }


    /// <summary>
    /// check for a shorter path to base by a new scouter and instantiate an antButton if true
    /// </summary>
    public void SetShortestPathToBase()
    {
        m_ShortestPathToBase.text = "Shortest path to base: " + m_CupcakeData.m_ShortestPathToBaseByScout.ToString();

        if(m_AntButton && m_CupcakeData.m_ShortestPathScout == null)
        {
            Debug.Log("DestroyAntButton caused by scout is null");
            Destroy(m_AntButton);
            m_AntButton = null;
        }
        if(m_AntButton == null && m_CupcakeData.m_ShortestPathToBaseByScout > 0)
        {
            m_AntButton = Instantiate(m_AntButtonPrefab, this.gameObject.transform);
        }
        if (m_AntButton)
        {
            m_AntButton.GetComponent<AntButtonScript>().m_Ant = m_CupcakeData.m_ShortestPathScout;
        }
        
    }
}
