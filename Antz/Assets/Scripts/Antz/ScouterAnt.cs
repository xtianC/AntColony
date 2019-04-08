
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScouterAnt : Ant
{


    //GameObjects
    public CupcakeDataScript m_CurrentFoodResource;


    //Pathstuff
    //path to a foodsource limited by maxPathLimit
    public List<Vector3> m_Path = new List<Vector3>();
    // known last path limited by 6 to prevent a scout is walking between a few waypoints
    public Queue<Vector3> m_LastPath;
    // next Position ant will move
    public Vector3 m_CurrentTargetPosition;          
    // last Position
    public Vector3 m_LastPos;
    //currentPathIndex
    public int m_CurrentPathPosition = 0;

    //Vars from Menu
    //what range they need to see food
    public float m_Perception;
    //how many waypoints for a way to food        
    public int m_MaxPathLength;                 

    public int m_LastPathLength;
    //PlayAnimationSpeed
    public int m_PlaySpeed;
    //if ant is too far from base it will be destroyed and a new one will spawn instead at base
    public int m_MaxRangeToBase;               



    //Debug
    public Material debugMaterial;
   



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
        m_Perception = ColonyOptions.ScoutPerception * m_FieldScale;
        m_MaxPathLength = ColonyOptions.MaxPathLimit;
        m_LastPathLength = ColonyOptions.LastPathLength;
        m_PlaySpeed = m_ColonyManager.m_PlaySpeed;
        m_MaxRangeToBase = (ColonyOptions.FieldSize * m_FieldScale) - 1;
    }

    /// <summary>
    /// Set startValues for ant and init for PositionFinding
    /// </summary>
    private void InitAnt()
    {
        m_LastPath = new Queue<Vector3>(m_LastPathLength);
        m_Path.Add(transform.position);
        m_CarriedFood.SetActive(false);
    }

    private void InitNewPositionFinding()
    {
        if (m_CurrentFoodResource != null)
        {
            if (m_CurrentFoodResource.m_ShortestPathScout == this.gameObject)
            {
                m_CurrentFoodResource.m_ShortestPathToBaseByScout = 0;
                m_CurrentFoodResource.m_ShortestPathScout = null;
            }
        }
        m_Path.Clear();
        m_LastPath.Clear();
        m_CurrentPathPosition = 0;
        m_Path.Add(Vector3.zero);
        transform.LookAt(Vector3.zero);       
        m_CurrentFoodResource = null;
        m_CarriedFood.SetActive(false);
        m_HasFood = false;
        m_FoundFood = false;
    }

    // Update is called once per frame
    private void Update()
    {

        if (!m_ColonyManager.m_SimulationActive)
        {
            m_Animator.speed = 0;
            return;
        }
        m_Animator.speed = 1;

        //ant to far away ? -> destroy it -> spawn a new one at base
        if (Mathf.Abs(transform.position.x) > m_MaxRangeToBase || Mathf.Abs(transform.position.z) > m_MaxRangeToBase || m_Path.Count > m_MaxPathLength)
        {
            FindObjectOfType<ColonyManager>().SpawnNewScout(Vector3.zero);
            Destroy(this.gameObject);
        }

        //move foreward -> allways!!!
        transform.position += transform.forward * Time.deltaTime * m_ColonyManager.m_PlaySpeed;

        if (!m_FoundFood)   //no food found -> searchAlgorithm
        {
            if (IsInRadius(transform.position, m_Path[m_Path.Count - 1], m_CollisionRange))    //reached new waypoint
            {              
                CheckForFood(m_Path[m_Path.Count - 1]); //search for food arround perception
                if (!m_FoundFood)   // still found no food -> new waypoint 
                {                    
                    NextMove();
                    m_CurrentPathPosition++;
                }
            }
        }
        else
        {
            if (m_HasFood)// hasfood loaded up -> go path backward until base and deliver Food
            {
                if(m_CurrentPathPosition == 0 && IsInRadius(transform.position, m_Path[m_CurrentPathPosition], m_CollisionRange)) // is at base with food -> deliver it
                {
                    DeliverFood();
                    SprayPheromone(m_Path[m_CurrentPathPosition]);
                }
                else if (IsInRadius(transform.position, m_Path[m_CurrentPathPosition], m_CollisionRange)) // is on the way with food -> walk path back
                {
                    if (CheckForBarrier(m_Path[m_CurrentPathPosition - 1])) //Suddenly Barrier -> search for new path
                    {                       
                        InitNewPositionFinding();
                        
                    }
                    else
                    {
                        SprayPheromone(m_Path[m_CurrentPathPosition]);
                        transform.LookAt(m_Path[m_CurrentPathPosition - 1]);
                        m_CurrentPathPosition--;
                    }                         
                }               
            }
            else  // found food and is unloaded or has delivered -> go Path foreward until foodSource and take food
            {
                if (m_CurrentPathPosition == m_Path.Count-1 && IsInRadius(transform.position, m_Path[m_CurrentPathPosition], m_CollisionRange))
                {
                    if (!CheckForFood(m_Path[m_Path.Count-1], true)) //food is gone ? -> search for new 
                    {
                        InitNewPositionFinding();
                    }
                    else
                    {
                        TakeFood();
                        SprayPheromone(m_Path[m_CurrentPathPosition]);
                    }                    
                }
                else if (IsInRadius(transform.position, m_Path[m_CurrentPathPosition], m_CollisionRange))
                {
                    if (CheckForBarrier(m_Path[m_CurrentPathPosition + 1])) //Suddenly Barrier -> search for new path
                    {
                        InitNewPositionFinding();                       
                    }
                    else
                    {
                        SprayPheromone(m_Path[m_CurrentPathPosition]);
                        transform.LookAt(m_Path[m_CurrentPathPosition + 1]);
                        m_CurrentPathPosition++;
                    }
                }
            }           
        }
    }



    


    /// <summary>
    /// ant searches for all possible waypoints arround a square of one waypoint
    /// can not go through a barrier or the last 6 waypoints 
    /// if there is more than one possible position per random one of it
    /// if no possible waypoint was found, positionfounding is init new
    /// make ant lookAt new position and set it as new currentTargetPosition
    /// </summary>
    private void NextMove()
    {
        Vector3 nextPathPoint = Vector3.zero;
        List<Vector3> possibleNextPositions = new List<Vector3>();
        

        for (int x = -1; x <= 1; x++) ///search for new position around
        {
            for (int z = -1; z <= 1; z++)
            {
                Vector3 tmp = m_Path[m_Path.Count - 1] + new Vector3(x * m_FieldScale, 0, z * m_FieldScale);

                if (m_LastPath.Contains(tmp) || CheckForBarrier(tmp) || !CheckForFieldSize(tmp))
                {
                    continue;
                }
                possibleNextPositions.Add(tmp);
            }
        }
        if(possibleNextPositions.Count != 0)
        {
            nextPathPoint = possibleNextPositions[UnityEngine.Random.Range(0, possibleNextPositions.Count)]; //choose one
            m_LastPath.Enqueue(nextPathPoint);
        }
        else
        {            
            Debug.Log("ZeroPosForScout"); //starting new search from base
            Debug.DrawRay(transform.position, Vector3.up*20, Color.green, 20);
            InitNewPositionFinding();
        }
       
        m_Path.Add(nextPathPoint);
        transform.LookAt(nextPathPoint);
    }



    /// <summary>
    /// checks for food at _position 
    /// also make ant lookAt foodPosition and set it as new currentTargetPosition
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_hasfood"></param>
    /// <returns></returns>
    private void CheckForFood(Vector3 _position)
    {
        Collider[] foodCollider = Physics.OverlapSphere(_position, m_Perception, m_FoodMask);
        if (foodCollider.Length > 0)
        {
            m_FoundFood = true;
            m_CurrentFoodResource = foodCollider[0].GetComponent<CupcakeDataScript>();
            m_CurrentFoodResource.CheckPathLenght(m_Path.Count, this.gameObject);
            m_Path.Add(foodCollider[0].transform.position);
            m_CurrentPathPosition++;
            transform.LookAt(foodCollider[0].transform.position);
            foodCollider[0].GetComponent<CupcakeDataScript>().m_TakenFood++;
        }       
    }


    /// <summary>
    /// checks for food at _position -> true if there is one
    /// </summary>
    /// <param name="_position"></param>
    /// <param name="_hasfood"></param>
    /// <returns></returns>
    private bool CheckForFood(Vector3 _position, bool _hasfood)
    {
        Collider[] foodCollider = Physics.OverlapSphere(_position, m_Perception, m_FoodMask);
        if (foodCollider.Length > 0)
        {
            return true;
        }
        return false;
    }
}
