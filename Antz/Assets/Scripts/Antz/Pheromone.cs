using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pheromone : MonoBehaviour
{
    public ColonyManager m_ColonyManager;
    public float m_LifeTime;
    public string m_Tag;
    private int m_PlaySpeed;


    //Pehromone is a standalone Object and will destroy himself after setted time
    void Start()
    {
        m_ColonyManager = FindObjectOfType<ColonyManager>();
        m_LifeTime = ColonyOptions.PheromoneDuration;
        gameObject.tag = m_Tag;
    }

    // check if simulation is stopped and lifetime is over zero
    void Update()
    {
        if (!m_ColonyManager.m_SimulationActive)
        {
            return;
        }
        m_LifeTime -= Time.deltaTime * m_ColonyManager.m_PlaySpeed;
        if (m_LifeTime <= 0.0f)
        {
            Destroy(this.gameObject);           
        }
    }
}
