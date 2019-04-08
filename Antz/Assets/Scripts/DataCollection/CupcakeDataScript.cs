using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupcakeDataScript : MonoBehaviour
{
    
    public float m_DistanceToBase;
    public int m_TakenFood;
    public int m_ShortestPathToBaseByScout;
    public GameObject m_ShortestPathScout;

    public BaseScript m_Base;


    /// <summary>
    /// Init startVals
    /// </summary>
    void Start()
    {
        Init();
    }

    /// <summary>
    /// Init startVals
    /// </summary>
    public void Init()
    {
        m_TakenFood = 0;
        m_ShortestPathToBaseByScout = 0;
        m_DistanceToBase = Vector3.Distance(transform.position, Vector3.zero) / ColonyOptions.FieldSclaling;
        m_Base = FindObjectOfType<BaseScript>();
        m_Base.AddCupcakeData(this);
        Debug.Log(m_DistanceToBase);      
    }


    /// <summary>
    /// check for a shorter path to base by a new scout and set it as new resourceScout if true
    /// </summary>
    /// <param name="_pathLength"></param>
    /// <param name="_scout"></param>
    public void CheckPathLenght(int _pathLength, GameObject _scout)
    {
        
        if(_pathLength < m_ShortestPathToBaseByScout || m_ShortestPathToBaseByScout == 0)
        {
            m_ShortestPathToBaseByScout = _pathLength;
            m_ShortestPathScout = _scout;
        }
    }


    /// <summary>
    /// just clean up on quit
    /// </summary>
    private void OnDestroy()
    {
        if (m_Base)
        {
            m_Base.DeleteCupcake(this);
        }        
    }
}
