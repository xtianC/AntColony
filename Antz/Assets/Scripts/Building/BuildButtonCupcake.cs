using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButtonCupcake : BuildButton
{

    /// <summary>
    /// almost the same like StandardBuildingProcedure exept a check for builded cupcake at all -> >=9 no more buildings of foodresources are allowed 
    /// moving mouse to change buildingPosition 
    /// wheel for rotating
    /// left to build with check for resourcesCount
    /// right to cancel
    /// </summary>
    protected override void Update()
    {
        if (!m_AttemptToBuild)
        {
            return;
        }
        if (Input.GetKeyDown("mouse 2") && !m_IsRotating)
        {
            m_RotationPoint = Input.mousePosition;
            m_IsRotating = true;
        }
        if (Input.GetKey("mouse 2") && m_IsRotating)
        {
            m_preBuild.transform.rotation = Quaternion.Euler(-90f, (m_RotationPoint.x - Input.mousePosition.x), 0f);
        }
        if (Input.GetKeyUp("mouse 2") && m_IsRotating)
        {
            m_IsRotating = false;
        }
        if (!m_IsRotating)
        {
            m_preBuild.transform.position = GetMapHitPoint();
        }
        if (Input.GetKeyDown("mouse 0"))
        {
            CupcakeDataScript tmp = m_preBuild.GetComponent<CupcakeDataScript>();
            if (tmp != null)
            {
                if (FindObjectOfType<BaseScript>().m_CupcakeList.Count < 9)
                {
                    InitBuilding();
                    //tmp.Init();
                    Debug.Log("Init at cupcake");
                }
            }
        }
        if (Input.GetKeyDown("mouse 1"))
        {
            CancelAttemptBuilding();
        }
    }
}
