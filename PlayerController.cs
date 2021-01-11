using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Vector3 ForwardVec = Vector3.zero;  //forward가 될 Vector

    public Camera _Camera = null;

    //움직임 제한을 위한 Ray를 쏠 위치
    public GameObject _TileCheckPos = null;

    public GameObject _CharacterCenterPos = null;

    private Animator Ani;

    private bool Run = false;

    private float MoveSpeed = 0f; //Player MoveSpeed

    //입력에 따른 Forward에 쓰일 인수
    private float UpButtonVec_X = 0f;
    private float UpButtonVec_Z = 0f;
    private float DownButtonVec_X = 0f;
    private float DownButtonVec_Z = 0f;
    private float RightButtonVec_X = 0f;
    private float RightButtonVec_Z = 0f;
    private float LeftButtonVec_X = 0f;
    private float LeftButtonVec_Z = 0f;

    //입력을 확인하는 값
    private int UpButton = 0;
    private int DownButton = 0;
    private int LeftButton = 0;
    private int RightButton = 0;

    //버튼의 입력 총값
    private int PushCount = 0;

    //Forward 결과 인수
    private float ForwardVec_X = 0;
    private float ForwardVec_Z = 0;

    //상수들
    private const int ButtonDown = 1;   //버튼 PreesDown
    private const int ButtonUp = 0;     //버튼 PreesUp


   
    // Start is called before the first frame update
    void Start()
    {
        Ani = this.GetComponent<Animator>();
        MoveSpeed = 1f;

    }

    // Update is called once per frame
    void Update()
    {
      
        if (Input.GetKey(KeyCode.W))
        {
            UpButton = ButtonDown;
            UpButtonVec_X = _Camera.transform.forward.x;
            UpButtonVec_Z = _Camera.transform.forward.z;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            UpButton = ButtonUp;
            UpButtonVec_X = 0f;
            UpButtonVec_Z = 0f;
        }


        if (Input.GetKey(KeyCode.S))
        {
            DownButton = ButtonDown;
            DownButtonVec_X = -_Camera.transform.forward.x;
            DownButtonVec_Z = -_Camera.transform.forward.z;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            DownButton = ButtonUp;
            DownButtonVec_X = 0f;
            DownButtonVec_Z = 0f;
        }


        if (Input.GetKey(KeyCode.A))
        {
            LeftButton = ButtonDown;
            LeftButtonVec_X = -_Camera.transform.right.x;
            LeftButtonVec_Z = -_Camera.transform.right.z;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            LeftButton = ButtonUp;
            LeftButtonVec_X = 0f;
            LeftButtonVec_Z = 0f;
        }


        if (Input.GetKey(KeyCode.D))
        {
            RightButton = ButtonDown;
            RightButtonVec_X = _Camera.transform.right.x;
            RightButtonVec_Z = _Camera.transform.right.z;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            RightButton = ButtonUp;
            RightButtonVec_X = 0f;
            RightButtonVec_Z = 0f;
        }

        //총 Prees된 개수
        PushCount = UpButton + DownButton + LeftButton + RightButton;

        //입력이 됐다면
        if (PushCount > 0)
        {
            if (Run == true)
            {
                MoveSpeed = 6f;
            }
            else
            {
                MoveSpeed = 3f;
            }

            //버튼이 눌린 값을 모두 더해 Vector의 평균값을 구함
            ForwardVec_X = (UpButtonVec_X + DownButtonVec_X + LeftButtonVec_X + RightButtonVec_X) / PushCount;
            ForwardVec_Z = (UpButtonVec_Z + DownButtonVec_Z + LeftButtonVec_Z + RightButtonVec_Z) / PushCount;

            //forward가 될 Vector값
            ForwardVec = new Vector3(ForwardVec_X, 0f, ForwardVec_Z);

            //천천히 Forward값을 변경
            this.transform.forward = Vector3.Lerp(this.transform.forward, ForwardVec, 0.5f);

            //실제 움직임값

            this.transform.Translate(this.transform.forward * 0.005f * MoveSpeed, Space.World);



            Ani.SetBool("Run", Run);
            Ani.SetBool("Walk", true);
        }
        else
        {
            ForwardVec_X = 0f;
            ForwardVec_Z = 0f;

            Ani.SetBool("Run", false);
            Ani.SetBool("Walk", false);
           
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Run = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Run = false;
        }
    }

    
}
