using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyButton : MonoBehaviour
{

    public GameObject m_DestroyIndicatorPrefab;
    public GameObject m_DestroyIndicator;
    bool m_AttemptDestroy = false;
    public LayerMask m_FoodMask;
    public LayerMask m_BarrierMask;
    public Transform m_CameraRig;
    public float m_MovingTimer = 0;
    public bool m_MoveForeward;

    /// <summary>
    /// just executed when in destroymode:
    /// moving destroyindicator(mousecursor
    /// get mousePos and possible building 
    /// 
    /// </summary>    
    private void Update()
    {
        if (!m_AttemptDestroy)
        {
            return;
        }
        Vector3 possiblePoint;
        if (GetBuildingHitPoint(out possiblePoint))
        {
            if (!m_MoveForeward)
            {
                m_MovingTimer += Time.deltaTime * 3;
                if (m_MovingTimer >= 1)
                {
                    m_MoveForeward = true;
                }
            }
            else
            {
                m_MovingTimer -= Time.deltaTime * 3;
                if (m_MovingTimer <= 0)
                {
                    m_MoveForeward = false;
                }
            }
            

            m_DestroyIndicator.transform.position = possiblePoint + Vector3.up * 5;
            m_DestroyIndicator.transform.localScale = new Vector3(1, 1, 1);

            m_DestroyIndicator.transform.position = Vector3.Lerp(m_DestroyIndicator.transform.position, m_DestroyIndicator.transform.position + Vector3.up * .5f , m_MovingTimer);
            
            if (Input.GetKey("mouse 0") && possiblePoint != Vector3.zero)
            {
                Destroy(GetBuildingFromHitPoint(possiblePoint));             
            }
        }
        else
        {          
            m_DestroyIndicator.transform.position = GetMapHitPoint();
            m_DestroyIndicator.transform.localScale = new Vector3(.5f, .5f, .5f);            
        }
        if (Input.GetKey("mouse 1"))
        {         
            Destroy(m_DestroyIndicator);
            m_AttemptDestroy = false;
            ColonyOptions.IsInBuildMode = false;
            Cursor.visible = true;
        }

    }


    /// <summary>
    /// check if a Building is at current mousePosition -> true if there is one 
    /// out current mouseposition
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    private bool GetBuildingHitPoint(out Vector3 _pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;      
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.gameObject.layer == 10 || hit.collider.gameObject.layer == 12)
            {
                _pos = hit.point;
                return true;
            }         
        }
        _pos = Vector3.zero;
        return false;
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
        return Vector3.one;
    }



    /// <summary>
    /// returning of a gameobject at given position
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    private GameObject GetBuildingFromHitPoint(Vector3 _pos)
    {       
        Collider[] food = Physics.OverlapSphere(_pos, .5f, m_FoodMask);
        if(food.Length > 0)
        {
            return food[0].gameObject;
        }
        Collider[] barrier = Physics.OverlapSphere(_pos, .5f, m_BarrierMask);
        if (barrier.Length > 0)
        {
            return barrier[0].gameObject;
        }
        return null;
    }



    /// <summary>
    /// cahnge gamestatus to destroymode
    /// </summary>
    public void AttemptDestroyBuilding()
    {
        m_AttemptDestroy = true;
        ColonyOptions.IsInBuildMode = true;
        m_DestroyIndicator = Instantiate(m_DestroyIndicatorPrefab);
        m_DestroyIndicator.transform.Rotate(0, 70 + float.Parse(m_CameraRig.localRotation.eulerAngles.y.ToString()), 0);
        Debug.Log(m_CameraRig.localRotation.eulerAngles.y.ToString());
        m_DestroyIndicator.transform.localScale = new Vector3(.5f, .5f, .5f);
        Cursor.visible = false;
    }

}
