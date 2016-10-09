using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{     
    public void StartLevel(int level)
    {
        Application.LoadLevel(level);
    }
}