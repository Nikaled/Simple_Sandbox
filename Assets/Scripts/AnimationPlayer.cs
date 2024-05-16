using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Player player;
    private void OnEnable()
    {
        player.PistolFire += DoPistolAnimation;
        player.characterController.myJumpAction += JumpAnimation;

    }
    private void OnDisable()
    {
        player.PistolFire -= DoPistolAnimation;
        player.characterController.myJumpAction -= JumpAnimation;

    }
    void Update()
    {
        if (player.examplePlayer.MyLockOnShoot == false)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            anim.SetBool("isRun", h != 0 || v != 0);
        }
        else
        {
            anim.SetBool("isRun", false);

        }
    }
    public void JumpAnimation()
    {
        anim.SetTrigger("jump");

    }
    private void DoPistolAnimation()
    {
        anim.SetTrigger("PistolFire");
    }

}
