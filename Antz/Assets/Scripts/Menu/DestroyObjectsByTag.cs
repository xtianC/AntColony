using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DestroyObjectsByTag : MonoBehaviour
{

    public List<string> m_DestroyTags;


    /// <summary>
    /// Destroy all Objects in Current Scene with given Tags
    /// </summary>
    public void DestroyByTag()
    {
        List<GameObject> allObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(allObjects);

        for(int i=0; i < m_DestroyTags.Count; i++)
        {
            // iterate root objects and destroy them if right tagged
            foreach (GameObject go in allObjects)
            {
                if (go.tag.Equals(m_DestroyTags[i]))
                {
                    Destroy(go);
                }
            }
        }
       
    }
    
}
