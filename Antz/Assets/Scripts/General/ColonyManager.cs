using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColonyManager : MonoBehaviour
{

    //Prefabs and GameObjects
    public GameObject m_WorkerAntPrefab;
    public GameObject m_ScouterAntPrefab;
    public GameObject m_PheromonePrefab;
    public GameObject m_RockBarrierPrefab;
    public Text m_TimeText;
    public Text m_SpeedText;
    public GameObject m_Playground;


    //Vars from SimultaionMenu
    //PlayAnimationSpeed
    public int m_PlaySpeed;
    //Size of buildable World
    public int m_FieldSize;
    //ants overall
    public int m_SpawnCount;
    //how many scout-ants
    public int m_ScoutCount;
    //spawn worker randomized at the same time or over time from base
    public bool m_SpawnTogether;            



    //Constants and helpVars
    public float m_Timer;
    public int m_Minutes;
    public int m_Seconds;
    //period pheromones and ants will spawn
    public const int m_SpawnInterval = 2;   
    private float m_SpawnTimer = 0.0f;        
    private int m_Spawns = 0;
    private bool m_SpawnPeriodFinished;
    public bool m_SimulationActive;
    



    // Start is called before the first frame update
    void Start()
    {
        LoadValues();                         //SimulationVals from Menu
        m_Playground.transform.localScale *= m_FieldSize;
        InitOuterBarrier();
        m_SpawnTimer = m_SpawnInterval;
        m_SpawnPeriodFinished = false;
        m_SpeedText.text = "x" + m_PlaySpeed.ToString();
    }

    private void InitOuterBarrier()
    {
        int halfSideLength = m_FieldSize * 10/2;
        for(int x=-halfSideLength;x<=halfSideLength; x += 5)
        {
            Instantiate(m_RockBarrierPrefab, new Vector3(x, 0, -halfSideLength), Quaternion.identity);
            Instantiate(m_RockBarrierPrefab, new Vector3(x, 0, halfSideLength), Quaternion.identity);
            Instantiate(m_RockBarrierPrefab, new Vector3(-halfSideLength, 0, x), Quaternion.Euler(0,90,0));
            Instantiate(m_RockBarrierPrefab, new Vector3(halfSideLength, 0, x), Quaternion.Euler(0, 90, 0));
           
        }
    }


    /// <summary>
    /// Load Values entered in SimulationMenu
    /// </summary>
    private void LoadValues()
    {
        m_PlaySpeed = ColonyOptions.PlaySpeedDefault;
        m_FieldSize = ColonyOptions.FieldSize;
        m_SpawnCount = ColonyOptions.ScoutCount + ColonyOptions.WorkerCount;
        m_ScoutCount = ColonyOptions.ScoutCount;
        m_SpawnTogether = (ColonyOptions.SpawnTogether == 0) ? false : true;
    }

    /// <summary>
    /// Spawning of BasePheromones
    /// Spawning of setted ants
    /// </summary>
    void Update()
    {
        if (!m_SimulationActive)
        {
            return;
        }

        m_SpawnTimer += Time.deltaTime * m_PlaySpeed;
        m_Timer += Time.deltaTime * m_PlaySpeed;
        m_TimeText.text = "Time: " + Mathf.Floor(m_Timer / 60) + ":" + (m_Timer % 60).ToString("f2");

        if (m_SpawnTimer > m_SpawnInterval)
        {
            SpawnBasePheromone();               

            if (!m_SpawnPeriodFinished)
            {
                if (m_SpawnTogether)           //spawn worker randomized at the same time ?  
                {
                    SpawnAllAntsRandomized();
                }

                if (m_ScoutCount > 0)
                {
                    SpawnNewScout(Vector3.zero);
                    m_ScoutCount--;
                    m_Spawns++;
                }
                else if (m_Spawns < m_SpawnCount)    //are there spawns left -> spawn workers
                {
                    SpawnNewWorker(Vector3.zero);
                    m_Spawns++;
                }
                else                            //all ants spawned ? -> finish spawn-period
                {
                    m_SpawnPeriodFinished = true;
                }
            }
            m_SpawnTimer = 0.0f;
        }
    }



    /// <summary>
    /// Spawn a Worker-Ant at given position
    /// </summary>
    /// <param name="_pos"></param>
    public void SpawnNewScout(Vector3 _pos)
    {
        GameObject tmp = Instantiate(m_ScouterAntPrefab, _pos, Quaternion.identity);
        tmp.GetComponent<ScouterAnt>().m_ColonyManager = this;
    }


    /// <summary>
    /// Spawn a Scouter-Ant at given position
    /// </summary>
    /// <param name="_pos"></param>
    public void SpawnNewWorker(Vector3 _pos)
    {
        GameObject tmp = Instantiate(m_WorkerAntPrefab, _pos, Quaternion.identity);
        tmp.GetComponent<WorkerAnt>().m_ColonyManager = this;     
    }



    /// <summary>
    /// Spawns all WorkerAnts at the same time -> randomized, centered in half of fieldSize
    /// </summary>
    void SpawnAllAntsRandomized() {

        int workerCount = m_SpawnCount - m_ScoutCount;

        for (int i = 0; i < workerCount ; i++)
        {
            SpawnNewWorker(new Vector3(((int)UnityEngine.Random.Range(-m_FieldSize/2, m_FieldSize/2) * ColonyOptions.FieldSclaling), 0, ((int)UnityEngine.Random.Range(-m_FieldSize / 2, m_FieldSize / 2) * ColonyOptions.FieldSclaling)));
        }
        m_Spawns += workerCount;
        m_SpawnTogether = false;        
    }



    /// <summary>
    /// Simulates a "full" Base of Ants
    /// in each spawnInterval randomly a pheromone is spawn in a 3x3 square arround the base
    /// </summary>
    void SpawnBasePheromone()
    {
        GameObject tmp = Instantiate(m_PheromonePrefab, new Vector3(((int)UnityEngine.Random.Range(-3, 3) * ColonyOptions.FieldSclaling), 0, ((int)UnityEngine.Random.Range(-3, 3) * ColonyOptions.FieldSclaling)), Quaternion.identity);
        tmp.GetComponent<Pheromone>().m_ColonyManager = this;      
    }



    /// <summary>
    /// Start or Stop for Simualtion
    /// changes m_SimulationActive to inverse
    /// </summary>
    public void StartStopSimulation()
    {
        m_SimulationActive = (m_SimulationActive) ? false : true;
    }


    public void Reset()
    {       
        LoadValues();
        m_SimulationActive = false;
        m_SpawnPeriodFinished = false;        
        m_Spawns = 0;
        m_Timer = 0;
        m_TimeText.text = "Time: " + Mathf.Floor(m_Timer / 60) + ":" + (m_Timer % 60).ToString("f2");
        m_PlaySpeed = 3;
        m_SpeedText.text = "x" + m_PlaySpeed.ToString();
    }

    /// <summary>
    /// quit simualtion and go back to mainMenu
    /// </summary>
    public void LeaveSimulation()
    {
        SceneManager.LoadScene("SimulationMenu");
    }



    /// <summary>
    /// increase SimulationSpeed to a max of x10
    /// </summary>
    public void IncreaseSimulationSpeed()
    {
        m_PlaySpeed++;
        if (m_PlaySpeed > 10)
        {
            m_PlaySpeed = 10;
        }
        m_SpeedText.text = "x" + m_PlaySpeed.ToString();
    }



    /// <summary>
    /// decrease SimulationSpeed to a min of x1
    /// </summary>
    public void DecreaseSimulationSpeed()
    {
        m_PlaySpeed--;
        if (m_PlaySpeed < 1)
        {
            m_PlaySpeed = 1;
        }
        m_SpeedText.text = "x" + m_PlaySpeed.ToString();
    }

}
