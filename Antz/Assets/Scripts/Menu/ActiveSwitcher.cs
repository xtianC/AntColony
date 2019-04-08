using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSwitcher : MonoBehaviour
{


    public GameObject m_SwitchObject;

    void Start()
    {
        m_SwitchObject.SetActive(false);
    }

    /// <summary>
    /// Switch activeState of a GameObject
    /// </summary>
    public void SwitchActiveState()
    {
        if (!m_SwitchObject.activeSelf)
        {
            m_SwitchObject.SetActive(true);
        }
        else
        {
            m_SwitchObject.SetActive(false);
        }
        
    }
}
