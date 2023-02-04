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
}