using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    public enum Scene
    {
        Menu,
        ShootingRange
    }

    public void LoadShootingPage()
    {
        SceneManager.LoadScene(Scene.ShootingRange.ToString());
    }

}
