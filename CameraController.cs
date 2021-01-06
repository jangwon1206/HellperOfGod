using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float Camera_StartMouseControlPos_X = 0f;
    private float Camera_StartMouseControlPos_Y = 0f;

    private float Camera_ControlRotation_X = 0f;
    private float Camera_ControlRotation_Y = 0f;

    private float Camera_ResultRotation_X = 0f;
    private float Camera_ResultRotation_Y = 0f;

    public Camera _Camera = null;

    public GameObject _CameraPos = null;

    private RaycastHit CameraPosRay;

    private float CameraMaxDistance = 0f;

    private float CameraDistance = 0f;

    // Start is called before the first frame update
    void Start()
    {
        CameraMaxDistance = -18f;
        CameraDistance = CameraMaxDistance;
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = _CameraPos.transform.position;
        //작업용 카메라위치 RAY
        Debug.DrawRay(this.transform.position, this.transform.forward * CameraMaxDistance, Color.red, 0.3f);

        //캐릭터 비추는 카메라의 위치를 조정
        if (Physics.Raycast(this.transform.position, -this.transform.forward, out CameraPosRay, -CameraMaxDistance))
        {
            //Map오브젝트가 캐릭터를 가릴 경우
            if(CameraPosRay.collider.CompareTag("Map"))
            {
                CameraDistance = -Vector3.Distance(this.transform.position, CameraPosRay.point); //Ray에 닿은 거리 (카메라의 거리)
            }
            //가리지 않을 경우
            else
            {
                CameraDistance = CameraMaxDistance;  //카메라 최대거리
            }

        }
        //Ray가 닿지 않을 경우 ( 최대거리 )
        else
        {
            CameraDistance = CameraMaxDistance;
        }
        

        //카메라의 위치
        _Camera.transform.localPosition= new Vector3(0f, 0f, CameraDistance);

        //마우스 오른쪽 키다운시 눌렀던 화면 처음포지션확인
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Camera_StartMouseControlPos_X = Input.mousePosition.x;
            Camera_StartMouseControlPos_Y = Input.mousePosition.y;
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            //마우스 클릭하여 드래그시 카메라의 방향을 조정
            Camera_ControlRotation_X = (Camera_StartMouseControlPos_X - Input.mousePosition.x) * 0.01f;
            Camera_ControlRotation_Y = (Camera_StartMouseControlPos_Y - Input.mousePosition.y) * 0.01f;

            if (Camera_ControlRotation_X >= 2f)
            {
                Camera_ControlRotation_X = 2f;
            }
            if (Camera_ControlRotation_Y >= 2f)
            {
                Camera_ControlRotation_Y = 2f;
            }

            Camera_ResultRotation_X = this.transform.eulerAngles.x + Camera_ControlRotation_Y;
            Camera_ResultRotation_Y = this.transform.eulerAngles.y - Camera_ControlRotation_X;

           

            this.transform.rotation = Quaternion.Euler(Camera_ResultRotation_X, Camera_ResultRotation_Y, 0f);

        }

        //휠 밀면 카메라 멀어짐
        if(Input.GetAxis("Mouse ScrollWheel")> 0f)
        {
            if (CameraMaxDistance > -20f)
            {
                CameraMaxDistance -= 1f;
            }
        }

        //휠 당기면 카메라 가까워짐
        if(Input.GetAxis("Mouse ScrollWheel")< 0f)
        {
           
            if (CameraMaxDistance < -3f)
            {
                CameraMaxDistance += 1f;
            }
        }
        
    }

}
