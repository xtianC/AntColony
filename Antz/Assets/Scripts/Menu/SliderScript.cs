using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{

    public Slider m_Slider;
    public int m_MaxRange;
    public Text m_Ammount;

    /// <summary>
    /// Init SliderVaulues
    /// </summary>
    void Start()
    {
        m_Slider = GetComponent<Slider>();
        m_Slider.maxValue = m_MaxRange;
    }


    /// <summary>
    /// Set ammount from slider to Text
    /// </summary>
    public void SetAmmount()
    {
        m_Ammount.text = m_Slider.value.ToString();
    }

    /// <summary>
    /// Set ammount from slider with given Value
    /// </summary>
    public void SetAmmount(int _value)
    {
        m_Slider.value = _value;
        SetAmmount();
    }
}