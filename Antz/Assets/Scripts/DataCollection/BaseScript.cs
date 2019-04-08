using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseScript : MonoBehaviour
{
    /////////////////////////////////////////////////////////////////////////////////
    ///Vars
    ///

    ///static
    public List<CupcakeDataScript> m_CupcakeList;
    public List<GameObject> m_CupcakeButtonList;
    public List<GameObject> m_CupcakeDataPanelList;

    public static int m_TotalFoodIncome = 0;

    ///GameObjects and Prefabs
    public GameObject m_BasePanel;
    public GameObject m_DataPanel;
    public GameObject CupcakePanel;
    public GameObject CupcakeButtonPrefab;
    public GameObject m_CupcakeDataPanelPrefab;

    public GameObject m_CameraRig;

    ///Layermasks
    public LayerMask m_BaseMask;
    public LayerMask m_FoodMask;



    ///data
    public int m_LastFoodIncomeRatio;

    public Text m_TotalFoodIncomeText;
    public Text m_LastFoodIncomeText;





    /// <summary>
    /// Set BaseDataPanel deactive
    /// </summary>
    private void Start()
    {
        m_BasePanel.SetActive(false);
    }

    /// <summary>
    /// handle of dataPanelView
    /// click on Base or Cupcake sets their dataPanel active 
    /// </summary>
    private void Update()
    {
        m_TotalFoodIncomeText.text = "Total Food-Income: " + m_TotalFoodIncome.ToString();

        if (Input.GetKey("mouse 0") && !ColonyOptions.IsInBuildMode)
        {
            CupcakeDataScript cds;
            if (IsBuildingAtHitPoint(GetMapHitPoint(), m_BaseMask))
            {
                m_BasePanel.SetActive(true);
                DeactiveAllDataPanel();
                m_DataPanel.SetActive(true);
            }         
            else if(IsBuildingAtHitPoint(GetMapHitPoint(), m_FoodMask, out cds))
            {
                m_BasePanel.SetActive(true);
                DeactiveAllDataPanel();
                int index = m_CupcakeList.IndexOf(cds);
                m_CupcakeDataPanelList[index].GetComponent<CupcakeDataPanelScript>().UpdateData();
                m_CupcakeDataPanelList[index].SetActive(true);
            }
        }
    }



    /// <summary>
    ///  Get the mousepostion on MapGround as Vector3
    /// </summary>
    /// <returns></returns>
    private Vector3 GetMapHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.positiveInfinity;
    }



    /// <summary>
    /// checks for a building at given pos and return true if there is one
    /// returns as out the cupcakedataScript
    /// </summary>
    /// <param name="_pos">pos for check</param>
    /// <param name="_layermask">Layer for checking Building</param>
    /// <param name="_resource"> returned object</param>
    /// <returns>true when there is a building with given _layermask at _pos</returns>
    private bool IsBuildingAtHitPoint(Vector3 _pos, LayerMask _layermask, out CupcakeDataScript _resource)
    {

        Collider[] c = Physics.OverlapSphere(_pos, 1, _layermask);
        if (c.Length > 0)
        {
            _resource = c[0].gameObject.GetComponent<CupcakeDataScript>();
            return true;
        }
        _resource = null;
        return false;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_layermask"></param>
    /// <returns></returns>
    private bool IsBuildingAtHitPoint(Vector3 _pos, LayerMask _layermask)
    {
        Collider[] c = Physics.OverlapSphere(_pos, 1, _layermask);
        if (c.Length > 0)
        {
            return true;
        }
        return false;       
    }



    /// <summary>
    /// Just counting collected food
    /// </summary>
    public static void IncreaseTotalFoodIncome()
    {
        m_TotalFoodIncome++;      
    }

    

    /// <summary>
    /// Add a Cupcake for DataCollection
    /// Creates a DataPanel for showing specific Cupcakedata
    /// Creates a Button for joining DataPanel and get cameraPosition to this Cupcake
    /// </summary>
    /// <param name="_cupcake"></param>
    public void AddCupcakeData(CupcakeDataScript _cupcake)
    {
        m_CupcakeList.Add(_cupcake);
        GameObject btn = Instantiate(CupcakeButtonPrefab, CupcakePanel.transform);
        GameObject pnl = Instantiate(m_CupcakeDataPanelPrefab, m_BasePanel.transform);
        pnl.SetActive(false);
        pnl.GetComponent<CupcakeDataPanelScript>().m_CupcakeData = _cupcake; //reference cupcakeData to cupcakeDataPanel
        btn.GetComponent<CupcakeButtonScript>().m_CupcakeData = _cupcake;   //reference cupcakeData to cupcakeButton
        btn.GetComponent<CupcakeButtonScript>().m_CameraRig = m_CameraRig;  //just for camerapositionchanging
        btn.GetComponent<CupcakeButtonScript>().m_CupcakeDataPanel = pnl;   //reference cupcakeDataPanel to cupcakeButton

        m_CupcakeDataPanelList.Add(pnl);
        m_CupcakeButtonList.Add(btn);        
    }




    /// <summary>
    /// set data of all resources to zero -> called when restarting ants
    /// </summary>
    public void ResetCupcakeData()
    {
        foreach(CupcakeDataScript cds in m_CupcakeList)
        {
            cds.m_TakenFood = 0;
            cds.m_ShortestPathScout = null;
            cds.m_ShortestPathToBaseByScout = 0;
        }
        m_TotalFoodIncome = 0;
    }




    /// <summary>
    /// Sets DataPanelData back to BaseData
    /// </summary>
    public void DeactiveAllDataPanel()
    {
        foreach(GameObject g in m_CupcakeDataPanelList)
        {
            g.SetActive(false);
        }
        m_DataPanel.SetActive(false);
    }


    

    /// <summary>
    /// Destroy a CupkakeButton from Datapanel -> is called when a foodresource is empty or destroyed by user
    /// removing GameOject from ButtonList and DataPanelList
    /// </summary>
    /// <param name="_cupcake"> CupcakeDataScript of destroyed Building</param>
    public void DeleteCupcake(CupcakeDataScript _cupcake)
    {
        int index = m_CupcakeList.IndexOf(_cupcake);
        //destroy Button
        GameObject btn = m_CupcakeButtonList[index];
        m_CupcakeButtonList.Remove(btn);               
        //detroy Panel
        GameObject pnl = m_CupcakeDataPanelList[index];
        m_CupcakeDataPanelList.Remove(pnl);       
        //destroy CupcakeScript
        m_CupcakeList.Remove(_cupcake);
        Destroy(pnl);
        Destroy(btn);
    }
}
