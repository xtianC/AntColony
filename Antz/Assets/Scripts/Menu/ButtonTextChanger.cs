using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextChanger : MonoBehaviour
{

    ////////////////////////////////////////////////////////////////////////////////////////////
    ///changing between to possible texts for a button with textA as starttext
    ///


    public string m_TextA;
    public string m_TextB;
    public Text m_Text;

    /// <summary>
    /// Start 
    /// init m_TextA for displaiying on button
    /// </summary>
    void Start()
    {
        m_Text.text = m_TextA;
    }


    /// <summary>
    /// change text to starttext 
    /// </summary>
    public void SetStartTextBack()
    {
        m_Text.text = m_TextA;
    }

    /// <summary>
    /// change text to other
    /// </summary>
    public void ChangeText()
    {
        m_Text.text = (m_Text.text.Equals(m_TextA)) ? m_TextB : m_TextA;
    }

}
