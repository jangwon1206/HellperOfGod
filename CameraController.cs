using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float camera_StartMouseControlPos_X = 0f;
    private float camera_StartMouseControlPos_Y = 0f;

    private float camera_ControlRotation_X = 0f;
    private float camera_ControlRotation_Y = 0f;

    private float camera_ResultRotation_X = 0f;
    private float camera_ResultRotation_Y = 0f;

    public Camera _Camera = null;

    public GameObject _CameraPos = null;

    private RaycastHit cameraPosRay;

    private float cameraMaxDistance = 0f;

    private float cameraDistance = 0f;

    public int layerMask_Map = 0;

    // Start is called before the first frame update
    void Start()
    {
        cameraMaxDistance = -18f;
        cameraDistance = cameraMaxDistance;
        layerMask_Map = 1 << LayerMask.NameToLayer("Map");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _CameraPos.transform.position;
        //작업용 카메라위치 RAY
        Debug.DrawRay(this.transform.position, this.transform.forward * cameraDistance, Color.red, 0.3f);

        //캐릭터 비추는 카메라의 위치를 조정
        if (Physics.Raycast(this.transform.position, -this.transform.forward, out cameraPosRay, -cameraMaxDistance, layerMask_Map))
        {
            //Map오브젝트가 캐릭터를 가릴 경우
            if(cameraPosRay.collider.CompareTag("Map"))
            {
                cameraDistance = -Vector3.Distance(this.transform.position, cameraPosRay.point); //Ray에 닿은 거리 (카메라의 거리)
            }
            //가리지 않을 경우
            else
            {
                cameraDistance = cameraMaxDistance;  //카메라 최대거리
            }

        }
        //Ray가 닿지 않을 경우 ( 최대거리 )
        else
        {
            cameraDistance = cameraMaxDistance;
        }
        

        //카메라의 위치
        _Camera.transform.localPosition= new Vector3(0f, 0f, cameraDistance);

        //마우스 오른쪽 키다운시 눌렀던 화면 처음포지션확인
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            camera_StartMouseControlPos_X = Input.mousePosition.x;
            camera_StartMouseControlPos_Y = Input.mousePosition.y;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            //마우스 클릭하여 드래그시 카메라의 방향을 조정
            camera_ControlRotation_X = (camera_StartMouseControlPos_X - Input.mousePosition.x) * 0.01f;
            camera_ControlRotation_Y = (camera_StartMouseControlPos_Y - Input.mousePosition.y) * 0.01f;

            if (camera_ControlRotation_X >= 2f)
            {
                camera_ControlRotation_X = 2f;
            }
            if (camera_ControlRotation_Y >= 2f)
            {
                camera_ControlRotation_Y = 2f;
            }

            camera_ResultRotation_X = this.transform.eulerAngles.x + camera_ControlRotation_Y;
            camera_ResultRotation_Y = this.transform.eulerAngles.y - camera_ControlRotation_X;

           

            this.transform.rotation = Quaternion.Euler(camera_ResultRotation_X, camera_ResultRotation_Y, 0f);

        }

        //휠 밀면 카메라 가까워짐
        if (Input.GetAxis("Mouse ScrollWheel")> 0f)
        {
            if (cameraMaxDistance < -3f)
            {
                cameraMaxDistance += 1f;
            }
        }

        //휠 당기면 카메라 멀어짐
        if (Input.GetAxis("Mouse ScrollWheel")< 0f)
        {
            if (cameraMaxDistance > -20f)
            {
                cameraMaxDistance -= 1f;
            }
            
        }
        
    }

}
