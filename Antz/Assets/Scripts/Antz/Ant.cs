using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{

    //Prefabs and Gameobjects
    public GameObject m_PheromonePrefab;
    public GameObject m_CarriedFood;
    public ColonyManager m_ColonyManager;
    public Animator m_Animator;

    //LayerMasks
    public LayerMask m_PheromoneMask;
    public LayerMask m_BaseMask;
    public LayerMask m_BarrierMask;
    public LayerMask m_FoodMask;

    //constants and helpVars
    public bool m_FoundFood;
    public bool m_HasFood;
    public bool m_IsInBase;
    public const float m_CollisionRange = .5f;  // is ant at currentTargetPosition ? -> helps to except a fail with lookAt 
    public const float m_BarrierCollisionRange = 6.0f;
    public int m_FieldScale;


    /// <summary>
    /// load up food
    /// </summary>
    protected void TakeFood()
    {
        m_CarriedFood.SetActive(true);
        m_HasFood = true;
    }



    /// <summary>
    /// unload food
    /// </summary>
    protected void DeliverFood()
    {
        m_CarriedFood.SetActive(false);
        m_HasFood = false;
        BaseScript.IncreaseTotalFoodIncome();
    }

    protected bool CheckForFieldSize(Vector3 _position)
    {
        if(Mathf.Abs(_position.x) > (ColonyOptions.FieldSize-1) * m_FieldScale || Mathf.Abs(_position.z) > (ColonyOptions.FieldSize - 1) * m_FieldScale)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// checks for a barrier at _position -> true if there is one
    /// </summary>
    /// <param name="_position"></param>
    /// <returns></returns>
    protected bool CheckForBarrier(Vector3 _position)
    {
        Collider[] barrierCollider = Physics.OverlapSphere(_position, m_BarrierCollisionRange, m_BarrierMask);
        if (barrierCollider.Length > 0)
        {
            return true;
        }
        return false;
    }



    /// <summary>
    /// CollisionDetection of point _a, point _b and and a _radius
    /// returns true if point _a is in radius of point _b
    /// </summary>
    /// <param name="_a"></param>
    /// <param name="_b"></param>
    /// <param name="_radius"></param>
    /// <returns></returns>
    protected bool IsInRadius(Vector3 _a, Vector3 _b, float _radius)
    {
        if (_a.x > _b.x - _radius && _a.x < _b.x + _radius &&
           _a.z > _b.z - _radius && _a.z < _b.z + _radius)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// instantiates a pheromone at given position
    /// </summary>
    /// <param name="_position"></param>
    protected void SprayPheromone(Vector3 _position)
    {
        GameObject tmp = Instantiate(m_PheromonePrefab, _position, Quaternion.identity);
        tmp.GetComponent<Pheromone>().m_ColonyManager = m_ColonyManager;
    }

}
