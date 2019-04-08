using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{

    /// /////////////////////////////////////////////////////////////////////
    /// Options
    /// 
    /// General
    public SliderScript m_FieldSize;
    public SliderScript m_PheromoneDuration;
    public SliderScript m_SpawnInterval;
    public SliderScript m_SpawnTogether;

    ///Worker
    public SliderScript m_WorkerCount;
    public SliderScript m_WorkerPerception;
    public SliderScript m_WorkerPathLength;

    ///Scout
    public SliderScript m_ScoutCount;
    public SliderScript m_ScoutPerception;
    public SliderScript m_ScoutPathLimit;


    private void Start()
    {       
        LoadDefaultValues();
    }

    /// <summary>
    /// Save Options to ColonyOptions
    /// </summary>
    public void SaveOptions()
    {
        if (m_FieldSize)
        {
            ColonyOptions.FieldSize = int.Parse(m_FieldSize.m_Ammount.text);
        }
        
        ColonyOptions.PheromoneDuration = int.Parse(m_PheromoneDuration.m_Ammount.text);
        ColonyOptions.SpawnInterval = int.Parse(m_SpawnInterval.m_Ammount.text);
        ColonyOptions.SpawnTogether = int.Parse(m_SpawnTogether.m_Ammount.text);

        ColonyOptions.WorkerCount = int.Parse(m_WorkerCount.m_Ammount.text);
        ColonyOptions.WorkerPerception = int.Parse(m_WorkerPerception.m_Ammount.text);
        ColonyOptions.LastPathLength = int.Parse(m_WorkerPathLength.m_Ammount.text);


        ColonyOptions.ScoutCount = int.Parse(m_ScoutCount.m_Ammount.text);
        ColonyOptions.ScoutPerception = int.Parse(m_ScoutPerception.m_Ammount.text);
        ColonyOptions.MaxPathLimit = int.Parse(m_ScoutPathLimit.m_Ammount.text);
    }




    /// <summary>
    /// LoadDefault Values from Colonymanager
    /// </summary>
    public void LoadDefaultValues()
    {
        if (m_FieldSize)
        {
            m_FieldSize.SetAmmount(ColonyOptions.FieldSizeDefault);
        }
        m_PheromoneDuration.SetAmmount(ColonyOptions.PheromoneDurationDefault);
        m_SpawnInterval.SetAmmount(ColonyOptions.SpawnIntervalDefault);
        m_SpawnTogether.SetAmmount(ColonyOptions.SpawnTogetherDefault);

        m_WorkerCount.SetAmmount(ColonyOptions.WorkerCountDefault);
        m_WorkerPerception.SetAmmount(ColonyOptions.WorkerPerceptionDefault);
        m_WorkerPathLength.SetAmmount(ColonyOptions.LastPathLengthDefault);


        m_ScoutCount.SetAmmount(ColonyOptions.ScoutCountDefault);
        m_ScoutPerception.SetAmmount(ColonyOptions.ScoutPerceptionDefault);
        m_ScoutPathLimit.SetAmmount(ColonyOptions.MaxPathLimitDefault);
    }


}
