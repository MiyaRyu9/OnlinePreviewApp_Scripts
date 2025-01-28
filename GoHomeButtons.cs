using UnityEngine.SceneManagement;
using UnityEngine;

public class GoHomeButtons : MonoBehaviour
{
    public void sample1()
    {
        SceneManager.LoadScene("samplehome");
    }
    public void avatarToHome()
    {
        SceneManager.LoadScene("title");
    }
}
