using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    public void ChangeScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
