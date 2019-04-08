using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButton : MonoBehaviour {


    //Object to build
	public GameObject m_BuildingPrefab;
    //Instance of building
	public GameObject m_preBuild;
    //Mesh of building
    public Mesh m_Mesh;


    //materialsList for several mats to differ preBuild and realBuild 
	public Material[] m_Materials;

    //do we want to build
	protected bool m_AttemptToBuild = false;
    //do we want to rotate
    protected bool m_IsRotating = false;
    //helpvar for define rotation
    protected Vector3 m_RotationPoint;
    



    /// <summary>
    /// Constructor for BuildButton
    /// deactivates cameraRotation
    /// Instatiates a prebuild 
    /// if materialsList size not 0 creating a preBuild with preBuildMaterial (maybe for a transparent appereance)
    /// </summary>
	public void AttemptBuilding(){

        m_AttemptToBuild = true;
        ColonyOptions.IsInBuildMode = true;
        FindObjectOfType<CameraControll>().SetRotationAllowed(false);

        m_preBuild = Instantiate(m_BuildingPrefab, GetMapHitPoint(), Quaternion.Euler(-90f, 0f, 0f));
       
        if (m_Materials.Length > 0)
        {
            m_preBuild.GetComponent<Renderer>().material = m_Materials[0];
        }
        Cursor.visible = false;      
	}



    /// <summary>
    /// StandardBuildingProcedure
    /// moving mouse to change buildingPosition 
    /// wheel for rotating
    /// left to build
    /// right to cancel
    /// </summary>
    protected virtual void Update()
    {
        if (!m_AttemptToBuild)
        {
            return;
        }
        if (Input.GetKeyDown("mouse 2") && !m_IsRotating) //attempt rotation
        {          
            m_RotationPoint = Input.mousePosition;
            m_IsRotating = true;
        }
        if (Input.GetKey("mouse 2") && m_IsRotating) // doing rotation
        {
            m_preBuild.transform.rotation = Quaternion.Euler(-90f, (m_RotationPoint.x - Input.mousePosition.x), 0f);
        }
        if (Input.GetKeyUp("mouse 2") && m_IsRotating) // cancel/finish rotation
        {
            m_IsRotating = false;           
        }
        if (!m_IsRotating) // moving
        {
            m_preBuild.transform.position = GetMapHitPoint();
        }
        if (Input.GetKeyDown("mouse 0")) // build
        {
            InitBuilding();
            Debug.Log("Init at Build");
        }
        if (Input.GetKeyDown("mouse 1")) //cancel buildingAttempt
        {
            CancelAttemptBuilding();
        }
    }


    /// <summary>
    /// Cancel buildingattemt
    /// activates cameraRotation
    /// destroys current preBuild
    /// </summary>
    protected void CancelAttemptBuilding()
    {
        m_AttemptToBuild = false;
        ColonyOptions.IsInBuildMode = false;
        Destroy(m_preBuild);
        Cursor.visible = true;
        FindObjectOfType<CameraControll>().SetRotationAllowed(true);
    }



    /// <summary>
    /// Creating  a real instance of a pre-created building
    /// setting and activating of public given triggerMesh
    /// Instance of a new preBuild -> same Buildingtype and rotation
    /// if materialsList size not 0 changing from preBuildMaterial to Buildingmaterial
    /// </summary>
    protected void InitBuilding()
    {
        m_preBuild.AddComponent<MeshCollider>();
        m_preBuild.gameObject.tag = "Building";
        MeshCollider mc = m_preBuild.GetComponent<MeshCollider>();
        mc.sharedMesh = m_Mesh;
        mc.convex = true;
        mc.isTrigger = true;

        if (m_Materials.Length > 0)
        {
            m_preBuild.GetComponent<Renderer>().material = m_Materials[1];
        }
        m_preBuild = Instantiate(m_BuildingPrefab, GetMapHitPoint(), m_preBuild.transform.rotation);
    }


    /// <summary>
    ///  Get the mousepostion on MapGround as Vector3
    /// </summary>
    /// <returns></returns>
	protected Vector3 GetMapHitPoint(){
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit)) {
			return hit.point;
		}
		return Vector3.one;
	}

}
