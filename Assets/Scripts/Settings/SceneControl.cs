using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{

   
    //로비
    //로딩 씬
    //게임 스테이지 씬

    private string currentScene = null;

    public void ScneneChange(string _SceneNum)
    {
       SceneManager.LoadScene(_SceneNum);
    }



    public string GetSceneName
    {
        set => currentScene = value;
    }

}
