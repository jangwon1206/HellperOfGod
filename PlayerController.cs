using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private Vector3 forwardVec = Vector3.zero;  //forward가 될 Vector

    public Camera _Camera = null;

    public GameObject _Weapon = null;

    private string weapon_kind = "";

    public float weapon_Speed = 0f;

    private Animator _Ani;

    //버튼체크용
    private bool run = false;
    private bool attack = false;

    private bool wait = false;
    private bool battle = false;
    private bool waitCheck = false;

    private int attack_Combo = 0;

    //전투 판단용
    private bool in_Action = false; 

    //실제 달리기 체크용
    public bool runing = false;

    private UIMgr _UIMgr = null;

    private float moveSpeed = 0f; //Player moveSpeed

    //입력에 따른 Forward에 쓰일 인수
    private float upButtonVec_X = 0f;
    private float upButtonVec_Z = 0f;
    private float downButtonVec_X = 0f;
    private float downButtonVec_Z = 0f;
    private float rightButtonVec_X = 0f;
    private float rightButtonVec_Z = 0f;
    private float leftButtonVec_X = 0f;
    private float leftButtonVec_Z = 0f;

    //입력을 확인하는 값
    private int upButton = 0;
    private int downButton = 0;
    private int leftButton = 0;
    private int rightButton = 0;

    //버튼의 입력 총값
    private int pushCount = 0;

    //Forward 결과 인수
    private float forwardVec_X = 0;
    private float forwardVec_Z = 0;

    //상수들
    private const int buttonDown = 1;   //버튼 PreesDown
    private const int buttonUp = 0;     //버튼 PreesUp

    private const float weapon_TwoHandAxe = 1f;
    private const float weapon_TwoHandSword = 2f;
    private const float weapon_OneHandSword = 3f;

    private State state_Check;

    private enum State
    {
        NOMAL,
        BATTLE,
    }
   
    // Start is called before the first frame update
    void Start()
    {
        _Ani = this.GetComponent<Animator>();
        _UIMgr = FindObjectOfType<UIMgr>();
        
        moveSpeed = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        switch(state_Check)
        {
            case State.NOMAL:
                {
                    _Ani.SetBool("Battle", false);
                }
                break;

            case State.BATTLE:
                {
                    _Ani.SetBool("Battle", true);
                }
                break;
        }

        if (Input.GetKey(KeyCode.W))
        {
            upButton = buttonDown;
            upButtonVec_X = _Camera.transform.forward.x;
            upButtonVec_Z = _Camera.transform.forward.z;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            upButton = buttonUp;
            upButtonVec_X = 0f;
            upButtonVec_Z = 0f;
        }


        if (Input.GetKey(KeyCode.S))
        {
            downButton = buttonDown;
            downButtonVec_X = -_Camera.transform.forward.x;
            downButtonVec_Z = -_Camera.transform.forward.z;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            downButton = buttonUp;
            downButtonVec_X = 0f;
            downButtonVec_Z = 0f;
        }


        if (Input.GetKey(KeyCode.A))
        {
            leftButton = buttonDown;
            leftButtonVec_X = -_Camera.transform.right.x;
            leftButtonVec_Z = -_Camera.transform.right.z;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            leftButton = buttonUp;
            leftButtonVec_X = 0f;
            leftButtonVec_Z = 0f;
        }


        if (Input.GetKey(KeyCode.D))
        {
            rightButton = buttonDown;
            rightButtonVec_X = _Camera.transform.right.x;
            rightButtonVec_Z = _Camera.transform.right.z;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            rightButton = buttonUp;
            rightButtonVec_X = 0f;
            rightButtonVec_Z = 0f;
        }

        //총 Prees된 개수
        pushCount = upButton + downButton + leftButton + rightButton;

        //입력이 됐다면
        if (battle == false)
        {
            if (pushCount > 0)
            {
                if (run == true)
                {
                    moveSpeed = 8f;
                    runing = true;
                }
                else
                {
                    moveSpeed = 4f;
                    runing = false;
                }

                //버튼이 눌린 값을 모두 더해 Vector의 평균값을 구함
                forwardVec_X = (upButtonVec_X + downButtonVec_X + leftButtonVec_X + rightButtonVec_X) / pushCount;
                forwardVec_Z = (upButtonVec_Z + downButtonVec_Z + leftButtonVec_Z + rightButtonVec_Z) / pushCount;

                //forward가 될 Vector값
                forwardVec = new Vector3(forwardVec_X, 0f, forwardVec_Z);

                //천천히 Forward값을 변경
                this.transform.forward = Vector3.Lerp(this.transform.forward, forwardVec, 0.5f);

                //실제 움직임값

                this.transform.Translate(this.transform.forward * 0.005f * moveSpeed, Space.World);



                _Ani.SetBool("Run", run);
                _Ani.SetBool("Walk", true);
            }
            else
            {
                forwardVec_X = 0f;
                forwardVec_Z = 0f;

                _Ani.SetBool("Run", false);
                _Ani.SetBool("Walk", false);
                runing = false;

            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_UIMgr.Stamina_Gauge <= 0f)
            {
                run = false;
            }
            else
            {
                run = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            run = false;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            state_Check = State.BATTLE;
            attack = true;
            in_Action = true;
            //처음 기본공격 시작시
            if (battle == false)
            {
                _Ani.SetBool("Attack_Basic" + attack_Combo, attack);
                for (int i = 0; i < _Weapon.transform.childCount; i++)
                {
                    if (_Weapon.transform.GetChild(i).gameObject.activeSelf == true)
                    {
                        weapon_kind = _Weapon.transform.GetChild(i).gameObject.tag;

                        if (weapon_kind == "Axe")
                        {
                            weapon_Speed = 1f;
                        }
                        else if (weapon_kind == "TwoHand_Sword")
                        {
                            weapon_Speed = 2f;
                        }
                        else if (weapon_kind == "OneHand_Sword")
                        {
                            weapon_Speed = 3f;
                        }


                        _Ani.SetFloat("Weapon_Speed", weapon_Speed);
                        return;
                    }
                }
                battle = true;
            }
        }
       
        
    }
    //공격후 다음공격을 확인하기위한 준비 이벤트 (공격)**
    void Event_Attack_NextPreparations()
    {
        attack = false;
        _Ani.SetBool("Attack_Basic" + attack_Combo, attack);

        waitCheck = false;

        attack_Combo += 1;

        if (attack_Combo >= 2)
        {
            attack_Combo = 0;
        }
        wait = false;
        _Ani.SetBool("Wait", wait);
    }


    //기본공격 콤보를 이어가기위한 이벤트 (공격) **
    void Event_AttackCombo()
    {
        if (attack == true)
        {
            _Ani.SetBool("Attack_Basic" + attack_Combo, attack);
        }
        else
        {
            wait = true;
            _Ani.SetBool("Wait", wait);
        }
    }

    //Battle_Idle에서 콤보를 이어갈 수 있도록 해주는 Event(Battle_Idle)**
    void Event_NextComboTimer()
    {
        if (waitCheck == false)
        {
            StartCoroutine(Attack_NextComboTimer()); //풀리면 돌면안됌
        }
    }

    //Battle_Idle상태에서 Normal상태로 돌아가기위한 이벤트 (Battle_Idle)
    void Event_BattleOut()
    {
        if (in_Action == true)
        {
            StartCoroutine(Action_Off());
        }
    }

    //BattleIdle상태에서 공격 Event (Battle_Idle)**
    void Event_BattleIdle_Attack()
    {
        if (attack == true)
        {
            _Ani.SetBool("Attack_Basic" + attack_Combo, attack);
            if (waitCheck == true)
            {
                StopCoroutine(Attack_NextComboTimer());
            }
        }
    }

    
    //처음 기본공격시 공격속도 확인용 이벤트 (Attack_Basic0)**
    void Event_AttackStart()
    {
        //현재 장착한 무기 확인
        
    }

    //5초안으로 공격하면 다음 콤보로 이어갈수있는 타이머
    IEnumerator Attack_NextComboTimer()
    {
        waitCheck = true;
        wait = false;
        _Ani.SetBool("Wait", wait);
        yield return new WaitForSeconds(5f);

        attack_Combo = 0;
        wait = true;
        _Ani.SetBool("Wait", wait);
    }

    //Battle상태 해제를 위한 코루틴
    IEnumerator Action_Off()
    {
        in_Action = false;
        yield return new WaitForSeconds(10f);

        if (in_Action == true)
        {
            StopCoroutine(Action_Off());
            yield break;
        }
        else
        {
            //battle = false;
            //_Ani.SetBool("Battle", battle);
        }
    }
}
