using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public Transform player;
    [SerializeField] GameObject GameTitle;
    [SerializeField] GameObject Play_Button;
    //[SerializeField] GameObject GameManager;
    public void OnButtonClick()
    {
        GameTitle.SetActive(false);
        Play_Button.SetActive(false);
        //GameManager.moveSpeed = 0.5f;
        GameManager.moveSpeed = 0.5f;
    }
}
