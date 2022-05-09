using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    // 마우스가 UI위에 있는지 체크
    // UI위에 있으면 마우스로 카메라 이동 불가
    private bool canCamMove = true;

    //이동 속도
    private float MoveSpeed = 5.0f;
  
    //메인 카메라를 담을 변수
    private Camera camera;

    //마우스로 이동할 때 해당 부분의 두께
    private float XThicness = Screen.height / 25;
    private float YThicness = Screen.height / 17;

    private void Start()
    {
        camera = Camera.main;
    }

    //카메라 이동

    //1. WASD키
    //2. 마우스가 화면에 끝에 닿으면 이동
    //3. 카메라가 바라보는 곳으로 축소 확대
    
    // 카메라 회전은 Y축으로만 회전

    private void Update()
    {
        if (canCamMove)
        {
            //키보드 WASD로 이동
            CamMoveByKeyBoard();
            ZoomCamera();

            //카메라가 회전중일 때는 마우스로 이동 불가
            if (Input.GetMouseButton(1))
            {
                RotateCamera();
            }
            else
            {
                CamMoveByMouse();
            }
        }
    }

    private void RotateCamera()
    {
        //마우스의 이동값 좌표 저장
        float mouseDelat = Input.GetAxisRaw("Mouse X");
        float thisAngle = this.transform.rotation.eulerAngles.y;

        this.transform.rotation = Quaternion.Euler(0, thisAngle + mouseDelat, 0).normalized;        
    }


    private float sumzoom = 0;

    private void ZoomCamera()
    {
        //카메라 확대 이동
        float zoominout = Input.GetAxisRaw("Mouse ScrollWheel") * 10f;

        //휠 값을 저장
        sumzoom += zoominout;

        //휠 값을 범위의 제한으로 두고 확대의 범위 제한
        sumzoom = Mathf.Clamp(sumzoom, -10, 20);

        if(zoominout >= 0 && sumzoom <20)
        {
            camera.transform.position += camera.transform.forward * zoominout * Time.deltaTime*20;
        }
        else if (zoominout <= 0 && sumzoom>-10)
        {
            camera.transform.position += camera.transform.forward * zoominout * Time.deltaTime*20;
        }
    }

    private void CamMoveByMouse()
    {
        if (Input.mousePosition.y >= Screen.height - YThicness)
        {
            this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= YThicness)
        {
            this.transform.position -= this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - XThicness)
        {
            this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= XThicness)
        {
            this.transform.position -= this.transform.right * MoveSpeed * Time.deltaTime;
        }

        //Mathf로 오브젝트의 위치를 제한
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, 30), this.transform.position.y, Mathf.Clamp(this.transform.position.z, 0, 30));
    }

    private void CamMoveByKeyBoard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += this.transform.forward*MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position -= this.transform.forward * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position -= this.transform.right * MoveSpeed * Time.deltaTime;
        }

        //Mathf로 오브젝트의 위치를 제한
        this.transform.position = new Vector3(Mathf.Clamp(this.transform.position.x, 0, 30), this.transform.position.y, Mathf.Clamp(this.transform.position.z, 0, 30));
    }

    //UI위에 마우스가 있는지 체크
    public void CamMoveOn()
    {
        canCamMove = true;
    }
    public void CamMoveOff()
    {
        canCamMove = false;
    }

}
