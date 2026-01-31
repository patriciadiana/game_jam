using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class MaskAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        animator.SetInteger("Level", sceneIndex);
    }
}
