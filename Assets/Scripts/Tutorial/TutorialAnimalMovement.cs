using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAnimalMovement : CitizenMovement
{
    protected override void Start()
    {
        checker = Instantiate(TutorialAnimalMovementManager.instance.Checker, gameObject.transform.position, Quaternion.identity);
        checker.citizen = this;
        checker.GetComponent<SphereCollider>().enabled = true;
        FindNewDestination();
        hpSystem.OnDied += CitizenDie;
    }
    public override void FindNewDestination()
    {
        TutorialAnimalMovementManager.instance.MoveCheckerToNewPoint(checker.gameObject);
    }
}
