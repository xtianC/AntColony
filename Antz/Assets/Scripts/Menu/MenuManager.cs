using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class MenuManager : OptionManager
{
  
    public VideoClip m_Clip;
    public GameObject m_HelpPanel;

    /// <summary>
    /// Init MainMenu
    /// </summary>
    private void Start()
    {
        InitBackgroundPlayer();       //initBackgorundVideo
        LoadDefaultValues();
        m_HelpPanel.SetActive(false);
    }

    /// <summary>
    /// turn of HelpPanel
    /// </summary>
    private void Update()
    {
        if(m_HelpPanel.activeSelf && Input.anyKey)
        {
            m_HelpPanel.SetActive(false);
        }
    }

    /// <summary>
    /// InitBackgroundVideo
    /// </summary>
    private void InitBackgroundPlayer()
    {
        GameObject camera = GameObject.Find("Main Camera");

        VideoPlayer videoPlayer = camera.AddComponent<VideoPlayer>();

        videoPlayer.clip = m_Clip;
        videoPlayer.renderMode = VideoRenderMode.CameraFarPlane;
        videoPlayer.targetCameraAlpha = 0.75F;
        videoPlayer.frame = 120;
        videoPlayer.isLooping = true;
        videoPlayer.SetDirectAudioMute(0, true);
        videoPlayer.Play();
    }

   
    /// <summary>
    /// Start new Scene: -> Simulation
    /// </summary>
    public void StartSimulation()
    {
        SaveOptions();
        SceneManager.LoadScene("Simulation");
    }


    /// <summary>
    /// Byby
    /// </summary>
    public void QuitSimulation()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
    }
}
