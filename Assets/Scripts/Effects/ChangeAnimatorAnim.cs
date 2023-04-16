using UnityEngine;

public class ChangeAnimatorAnim : MonoBehaviour
{
    public Animator anim;
    public string animationPlay = "";

    private void Start()
    {
        anim.Play(animationPlay);
    }
}
