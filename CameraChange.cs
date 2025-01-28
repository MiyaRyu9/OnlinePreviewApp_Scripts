using UnityEngine;

public class CameraChange : MonoBehaviour
{

    public GameObject mainCamera; 
    public GameObject subCamera; 
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            if(mainCamera.activeSelf){
                mainCamera.gameObject.SetActive(false);
                subCamera.gameObject.SetActive(true);
            }
            else{
                mainCamera.gameObject.SetActive(true);
                subCamera.gameObject.SetActive(false);
            }
        }   
    }
}