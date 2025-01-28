using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class titleButtons : MonoBehaviour
{
    public void StartBtn()
    {
        
        SceneManager.LoadScene("ChoiceHome");
    }
    public void ChatBtn()
    {

        SceneManager.LoadScene("chat");
    }

    public void ConfigBtn()
    {
        SceneManager.LoadScene("Setting");
    }

    public void BackTitleBtn()
    {
        SceneManager.LoadScene("title");
    }

    public void ChoiceAvatar()
    {
        SceneManager.LoadScene("ChoiceAvatar");
    }

    

}
