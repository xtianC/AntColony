using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnt : Ant
{
    //Pathstuff
    // known last path
    public Queue<Vector3> m_LastPath;
    // next Position ant will move
    public Vector3 m_CurrentTargetPosition;
    // last Position needed just when taking food
    public Vector3 m_LastPos;



    //Vars from Menu
    //what range they need to see food or base
    public float m_Perception;
    //how many waypoints backward they remember        
    public int m_MaxKnownPathLimit;
    //PlayAnimationSpeed
    public int m_PlaySpeed;
    //if ant is too far from base it will be destroyed and a new one will spawn instead at base
    public int m_MaxRangeToBase;                

 

    
    





    // Start is called before the first frame update
    private void Start()
    {
        LoadValues();
        InitAnt();
        gameObject.tag = "Ant";
    }

    /// <summary>
    /// Load Values entered in SimulationMenu
    /// </summary>
    private void LoadValues()
    {
        m_FieldScale = ColonyOptions.FieldSclaling;
        m_PlaySpeed = m_ColonyManager.m_PlaySpeed;
        m_Perception = (ColonyOptions.WorkerPerception * m_FieldScale) + 1;
        m_MaxKnownPathLimit = ColonyOptions.LastPathLength;
        m_MaxRangeToBase = (ColonyOptions.FieldSize * m_FieldScale) - 1;
    }


    /// <summary>
    /// Set startValues for ant and init PositionFinding
    /// </summary>
    private void InitAnt()
    {
        m_CarriedFood.SetActive(false);
        m_CurrentTargetPosition = transform.position;
        m_LastPath = new Queue<Vector3>(m_MaxKnownPathLimit);
        InitPositionFinding();
    }



    /// <summary>
    /// Delete last known path and gets a new position
    /// </summary>
    private void InitPositionFinding()
    {
        m_LastPath.Clear();
        m_LastPath.Enqueue(m_CurrentTargetPosition);
        FindNewPosition();
    }



    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (!m_ColonyManager.m_SimulationActive)
        {
            m_Animator.speed = 0;
            return;
        }
        m_Animator.speed = 1;

        //ant to far away ? -> destroy it -> spawn a new one at base
        if (Mathf.Abs(transform.position.x) > m_MaxRangeToBase || Mathf.Abs(transform.position.z) > m_MaxRangeToBase)
        {
            FindObjectOfType<ColonyManager>().SpawnNewWorker(Vector3.zero);
            Destroy(this.gameObject);
        }

        //saveLookAt at current position -> u will never know what happen XD
        transform.LookAt(m_CurrentTargetPosition);


        //move foreward -> allways!!!
        transform.position += transform.forward * Time.deltaTime * m_ColonyManager.m_PlaySpeed;

        if (IsInRadius(transform.position, m_CurrentTargetPosition, m_CollisionRange)) //ant is reaching new waypoint
        {
            SprayPheromone(m_CurrentTargetPosition);  //spawn a pheromone at current waypoint 
            
            if (!m_FoundFood) //no Food -> search for it
            {
                if (!CheckForFood(m_CurrentTargetPosition)) //still found no food  -> New Position
                {
                    FindNewPosition();
                }
            }
            else //There is Food What to do?
            {
                if (!m_HasFood && Vector3.Distance(m_CurrentTargetPosition, transform.position) > 5) // found food but its to far for one waypoint ?
                {                                                                                    // set a new waypoint there !!not offical waypointmap!!   
                    m_CurrentTargetPosition = transform.forward * m_FieldScale;
                    m_LastPos = m_CurrentTargetPosition;
                }
                else if(!m_HasFood) //reached food ? -> take it
                {
                    TakeFood();
                }
                else
                {
                    if (!CheckForBase(m_CurrentTargetPosition))//No Base -> New Position
                    {
                        FindNewPosition();
                    }
                    else 
                    {
                        if (!m_IsInBase)   // reached base ? -> deliver it
                        {
                            DeliverFood();
                        }
                    }
                }             
            }
        }
    }




    /// <summary>
    /// overrides
    /// load up food and move to last position
    /// </summary>
    private new void TakeFood()
    {
        base.TakeFood();

        m_LastPath.Clear();
        transform.LookAt(m_LastPos);
        m_CurrentTargetPosition = m_LastPos;      
        m_IsInBase = false;
    }



    /// <summary>
    /// checks for food at _position -> true if there is one
    /// also make ant lookAt foodPosition and set it as new currentTargetPosition
    /// </summary>
    /// <param name="_position"></param>
    private bool CheckForFood(Vector3 _position)
    {
        Collider[] foodCollider = Physics.OverlapSphere(_position, m_Perception, m_FoodMask);
        if (foodCollider.Length > 0)
        {
            m_FoundFood = true;
            m_LastPos = m_CurrentTargetPosition;
            transform.LookAt(foodCollider[0].transform.position);
            m_CurrentTargetPosition = foodCollider[0].transform.position;
            foodCollider[0].GetComponent<CupcakeDataScript>().m_TakenFood++;           
            return true;
        }
        return false;
    }


    /// <summary>
    /// unload food and search a new position, clears last known Path to give option moving back to food
    /// </summary>
    private new void DeliverFood()
    {
        base.DeliverFood();

        m_LastPath.Clear();
        InitPositionFinding();      
        m_IsInBase = true;
        m_FoundFood = false;
    }




    /// <summary>
    /// checks for the base at _position -> true if there is one
    /// also make ant lookAt basePosition and set it as new currentTargetPosition
    /// </summary>
    /// <param name="_position"></param>
    private bool CheckForBase(Vector3 _position)
    {
        Collider[] baseCollider = Physics.OverlapSphere(_position, 6, m_BaseMask);
        if (baseCollider.Length > 0)
        {           
            transform.LookAt(baseCollider[0].transform.position);
            m_CurrentTargetPosition = baseCollider[0].transform.position;
            return true;
        }
        return false;
    }





    /// <summary>
    /// ant searches for all possible waypoints arround a square of two waypoints
    /// it will move to the highest pheromone-conzentration 
    /// if there is more than one position with an equal conzentration it will choose per random one of it
    /// if no possible waypoint was found, positionfounding is init new
    /// make ant lookAt new position and set it as new currentTargetPosition
    /// </summary>
    private void FindNewPosition()
    {      
        Vector3 nextPos;         //nextPosition 
        List<Vector3> possibleNextPositions = new List<Vector3>(); //possiblenextPositions
        possibleNextPositions.Add(Vector3.zero); // add zero for NoWaypointFoundException

        int maxConzentration = -1;              //maxConzentration arround 

        for (int x = -2; x <= 2; x++)
        {
            for (int z = -2; z <= 2; z++)
            {
                Vector3 tmp = m_CurrentTargetPosition + new Vector3(x * m_FieldScale, 0, z * m_FieldScale); //searching arround

                
                if (m_LastPath.Contains(tmp) || CheckForBarrier(tmp) || !CheckForFieldSize(tmp)) // barrier or was in last moves ? -> lapse waypoint
                {                    
                    continue;
                }
                
                int pheromoneConzentration = PheromoneConzentration(tmp); //helpvar to match pheromoneconzentration

                if (maxConzentration < pheromoneConzentration) //found new high ? ->clear all possible and add it to list
                {
                    possibleNextPositions.Clear();
                    possibleNextPositions.Add(tmp);
                    maxConzentration = pheromoneConzentration;                             
                }
                else if (maxConzentration == pheromoneConzentration) // found an equal conzentration ? -> add it to list
                {
                    possibleNextPositions.Add(tmp);
                }               
            }
        }

        nextPos = possibleNextPositions[UnityEngine.Random.Range(0, possibleNextPositions.Count)]; //choose one in list and set it as new position
        m_LastPath.Enqueue(nextPos);
        transform.LookAt(nextPos);
        m_CurrentTargetPosition = nextPos;

        if(nextPos == Vector3.zero)
        {
            Debug.Log("ZeroPos");
            Debug.DrawRay(transform.position, Vector3.up * 20, Color.red, 20);
        }
        
    }



    /// <summary>
    /// checks for pheromoneConzentration at _position -> return conzentration at _postion
    /// </summary>
    /// <param name="_position"></param>
    private int PheromoneConzentration(Vector3 _position)
    {
        Collider[] pheromones = Physics.OverlapSphere(_position, .5f, m_PheromoneMask);
        
        return pheromones.Length;
    }
}
