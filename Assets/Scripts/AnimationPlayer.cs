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
    }
    private void OnDisable()
    {
        player.PistolFire -= DoPistolAnimation;
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        anim.SetBool("isRun", h != 0 || v != 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("jump");
        }
    }
    private void DoPistolAnimation()
    {
        anim.SetTrigger("PistolFire");
    }

}
