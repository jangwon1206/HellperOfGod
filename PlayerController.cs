using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 MoveVec = Vector3.zero;
    public Camera _Camera = null;
    private Vector3 ForwardVec = Vector3.zero;

    private bool UpButton = false;
    private bool DownButton = false;
    private bool LeftButton = false;
    private bool RightButton = false;
   
    public enum State
    {
        IDLE,
        MOVE
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        if(Input.GetKey(KeyCode.W))
        {
            ForwardVec = new Vector3(_Camera.transform.forward.x, 0f, _Camera.transform.forward.z).normalized;
            this.transform.forward = Vector3.Lerp(this.transform.forward, ForwardVec, 0.06f);

            MoveVec = this.transform.forward;

            this.transform.Translate(MoveVec* 0.01f, Space.World);
        } 
        if(Input.GetKey(KeyCode.S))
        {
            ForwardVec = new Vector3(_Camera.transform.forward.x, 0f, _Camera.transform.forward.z).normalized;
            this.transform.forward = Vector3.Lerp(this.transform.forward, -ForwardVec, 0.06f);

            MoveVec = this.transform.forward;
            
            this.transform.Translate(MoveVec*0.01f, Space.World);    
        }
        if (Input.GetKey(KeyCode.A))
        {
            ForwardVec = new Vector3(_Camera.transform.right.x, 0f, _Camera.transform.right.z).normalized;
            this.transform.forward = Vector3.Lerp(this.transform.forward, -ForwardVec, 0.06f);

            MoveVec = this.transform.forward;

            this.transform.Translate(MoveVec * 0.01f, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            ForwardVec = new Vector3(_Camera.transform.right.x, 0f, _Camera.transform.right.z).normalized;
            this.transform.forward = Vector3.Lerp(this.transform.forward, ForwardVec, 0.06f);

            MoveVec = this.transform.forward;

            this.transform.Translate(MoveVec * 0.01f, Space.World);
        }
       
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            UpButton = true;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            UpButton = false;
        }


        if (Input.GetKeyDown(KeyCode.S))
        {
            DownButton = true;
        } 
        if (Input.GetKeyUp(KeyCode.S))
        {
            DownButton = false;
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            RightButton = true;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            RightButton = false;
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            LeftButton = true;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            LeftButton = false;
        }
    }
}
