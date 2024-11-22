using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResetButton : MonoBehaviour
{

     public void OnButtonClick()
    {
        Debug.Log("hi");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
