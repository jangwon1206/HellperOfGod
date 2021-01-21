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
    private bool idleComebackCheck = false;
    private bool combo = false;

    public int attack_Combo = 0;
    private bool comboTime = false;


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

    private IEnumerator timer_AttackCombo;
    private IEnumerator timer_BattleState_Off;

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
        timer_AttackCombo = Timer_AttackCombo();
        timer_BattleState_Off = Timer_BattleState_Off();

    }

    // Update is called once per frame
    void Update()
    {
        

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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(battle == false)
            {
                battle = true;
                _Ani.SetBool("Battle", battle);
                WeaponSpeed();
            }
            
            attack = true;
            idleComebackCheck = true;
           
           
           
            if(comboTime == true || wait == true)
            {
                comboTime = false;
                _Ani.SetBool("Attack_Basic" + attack_Combo, attack);
                wait = false;
                _Ani.SetBool("Wait", wait);
            }
        }


    }

    //장착한 무기의 스피드를 확인 (공격 시작전 한번만 확인하면됌)
    void WeaponSpeed()
    {
        for (int i = 0; i < _Weapon.transform.childCount; i++)
        {
            if (_Weapon.transform.GetChild(i).gameObject.activeSelf == true)
            {
                weapon_kind = _Weapon.transform.GetChild(i).gameObject.tag;

                if (weapon_kind == "Axe")
                {
                    weapon_Speed = 0.8f;
                }
                else if (weapon_kind == "TwoHand_Sword")
                {
                    weapon_Speed = 1f;
                }
                else if (weapon_kind == "OneHand_Sword")
                {
                    weapon_Speed = 1.2f;
                }


                _Ani.SetFloat("Weapon_Speed", weapon_Speed);
                return;
            }
        }
    }

    //공격후 다음공격을 확인하기위한 준비 이벤트 (Attack)
    void Event_Attack_NextPreparations()
    {
        StopCoroutine(timer_AttackCombo);
        StopCoroutine(timer_BattleState_Off);
        comboTime = false;
        attack = false;
        _Ani.SetBool("Attack_Basic" + attack_Combo, attack);


        attack_Combo += 1;

        if (attack_Combo >= 3)
        {
            attack_Combo = 0;
        }
        wait = false;
        _Ani.SetBool("Wait", wait);
    }

    //attack도중 combo를 이어서 할수있는지 확인해주는 이벤트(attack)
    void Event_AttackCombo()
    {
        comboTime = true;

    }

    //아무런 공격입력이 없을시 wait상태로 (attack);
    void Event_AttackWait()
    {

        combo = true;
        wait = true;
        _Ani.SetBool("Wait", wait);

    }


    //Wait 애니메이션 진입시 다음 콤보를 이어가도록함 (Wait -> attack)
    void Event_BattleCombo()
    {
        if (combo == true)
        {
            StartCoroutine(timer_AttackCombo);
        }
    }

    IEnumerator Timer_AttackCombo()
    {
        
        combo = false;
        yield return new WaitForSecondsRealtime(4f);
        attack_Combo = 0;
    }

    //Wait 애니메이션 진입시 한번만 돌아가도록함 (WAIT -> IDLE)
    void Event_BattleState_Off()
    {
        if (idleComebackCheck == true)
        {
            StartCoroutine(timer_BattleState_Off);
        }
    }

    IEnumerator Timer_BattleState_Off()
    {
        idleComebackCheck = false;
        
        yield return new WaitForSecondsRealtime(8f);
        wait = false;
        _Ani.SetBool("Wait", wait);
        battle = false;
        _Ani.SetBool("Battle", battle);
    }




}
