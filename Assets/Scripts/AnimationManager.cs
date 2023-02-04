using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetWalking(bool walking)
    {
        _animator.SetBool("IsWalking", walking);
    }

    public void Root()
    {
        _animator.Play("Root");
    }

    public void Uproot()
    {
        _animator.Play("Uproot");
    }

    public void SrartPossess()
    {
        _animator.Play("StartPossess");
    }

    public void EndPossess()
    {
        _animator.Play("EndPossess");
    }
}