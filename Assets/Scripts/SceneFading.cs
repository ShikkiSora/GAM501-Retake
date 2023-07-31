using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFading : MonoBehaviour
{
    public Animator animator;
    private int levelToLoad;

    void Update()
    {
        
    }
    public void FadeToLevel (int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
