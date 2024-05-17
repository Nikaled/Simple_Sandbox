using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CitizenMovement : MonoBehaviour
{
    private bool isMoving;
    public NavMeshAgent agent;
    public Animator animator;
    public Transform CurrentDestination;
    private Vector3? CurrentDest;
    private NavMeshChecker checker;
    private bool IsDying;
    [SerializeField] HpSystem hpSystem;
    private void Start()
    {
        checker = Instantiate(CitizenNavMeshManager.instance.Checker, gameObject.transform.position, Quaternion.identity);
        checker.citizen = this;
        checker.GetComponent<SphereCollider>().enabled = true;
        FindNewDestination();
        hpSystem.OnDied += CitizenDie;
    }
    public void MoveToPosition(Vector3 DestinationPosition)
    {

        agent.SetDestination(DestinationPosition);
        animator.SetBool("IsWalk", true);
    }
    private void StopMoving()
    {
        animator.SetBool("IsWalk", false);
        Debug.Log("Stop moving Citizen");

    }
    private void CitizenDie()
    {
        if(gameObject.GetComponent<CapsuleCollider>() != null)
        {
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        animator.SetBool("IsWalk", false);
        animator.SetTrigger("Die");
        IsDying = true;
        agent.SetDestination(transform.position);
        StartCoroutine(WaitForDyingAnimation());
    }
    private IEnumerator WaitForDyingAnimation()
    {
        hpSystem.enabled = false;
        yield return new WaitForSeconds(2f);
        Destroy(hpSystem.RootObject);
    }
    public void FindNewDestination()
    {
        CitizenNavMeshManager.instance.MoveCheckerToNewPoint(checker.gameObject);
    }
    void Update()
    {
        if(IsDying == false)
        {
            MoveToPosition(checker.transform.position);
            if (Vector3.Distance(gameObject.transform.position, checker.transform.position) < 9)
            {
                StopMoving();
                FindNewDestination();
            }
        }

    }
}
