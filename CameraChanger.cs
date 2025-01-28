using Cinemachine;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    /// <summary>
    /// 一人称視点の参照
    /// </summary>
    [SerializeField]
    private CinemachineVirtualCamera firstPersonCamera;


    /// <summary>
    /// 三人称視点の参照
    /// </summary>
    [SerializeField]
    private CinemachineVirtualCamera thirdPersonCamera;

    [SerializeField]
    private bool isFPS;

    private void Start()
    {
        SetFirstPersonCamera();
    }
    

    /// <summary>
    /// 視点の切り替えを実行する
    /// </summary>
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        if(isFPS)
        {
            SetThirdPersonCamera();
        }
        else{
            SetFirstPersonCamera();
        }
      }
    }   

    /// <summary>
    ///一人称視点に切り替える
    /// </summary>
    private void SetFirstPersonCamera()
    {
        firstPersonCamera.Priority=10;
        thirdPersonCamera.Priority=0;
        isFPS=true;
    }

    /// <summary>
    /// 三人称視点に切り替える
    /// </summary>
    private void SetThirdPersonCamera()
    {
        firstPersonCamera.Priority=0;
        thirdPersonCamera.Priority=10;
        isFPS=false;
    }
}
