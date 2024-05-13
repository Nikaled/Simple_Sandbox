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
    private void Start()
    {
        checker = Instantiate(CitizenNavMeshManager.instance.Checker, gameObject.transform.position, Quaternion.identity);
        checker.citizen = this;
        checker.GetComponent<SphereCollider>().enabled = true;
        FindNewDestination();
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
    public void FindNewDestination()
    {
        CitizenNavMeshManager.instance.MoveCheckerToNewPoint(checker.gameObject);
    }
    void Update()
    {
         MoveToPosition(checker.transform.position);
            if (Vector3.Distance(gameObject.transform.position, checker.transform.position) < 9)
            {
                StopMoving();
                FindNewDestination();
            }
    }
}
