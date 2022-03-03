using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private void Update()
    {
        CamMove();
    }
   

    private void CamMove()
    {
        Vector2 mouseDela2 = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        if (Input.GetMouseButton(1))
        {
            //마우스의 이동값 좌표 저장
            Vector2 mouseDelat = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

            Vector3 CamAngle = this.transform.rotation.eulerAngles; //카메라의 부모 오브젝트의 회전값을 변수로 저장
           

            //마우스의 Y축 회전은 3D오브젝트의 X축 회전
            float X = CamAngle.x -= mouseDelat.y;
            
            //카메라 상하의 제한
            X = Mathf.Clamp(X, 1f, 90f);
            Debug.Log(X);

            this.transform.rotation = Quaternion.Euler(X, CamAngle.y + mouseDelat.x, CamAngle.z).normalized;
        }

        //카메라 이동
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveZ = Input.GetAxisRaw("Vertical");

        this.transform.position += this.transform.forward*MoveZ* Time.deltaTime*2.0f;
        this.transform.position += this.transform.right*MoveX*Time.deltaTime*2.0f;

        float Y = Input.GetAxisRaw("Mouse ScrollWheel");
        Debug.Log(Y);
        this.transform.position -= new Vector3(0, Y, 0).normalized * Time.deltaTime*5.0f;

    }

}
