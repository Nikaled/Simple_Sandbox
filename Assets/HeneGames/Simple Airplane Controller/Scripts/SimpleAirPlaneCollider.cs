using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeneGames.Airplane
{
    public class SimpleAirPlaneCollider : MonoBehaviour
    {
        public bool collideSometing;

        [HideInInspector]
        public SimpleAirPlaneController controller;

        private void OnTriggerEnter(Collider other)
        {
            //Collide someting bad
            if (other.gameObject.GetComponent<SimpleAirPlaneCollider>() == null && other.gameObject.GetComponent<LandingArea>() == null)
            {
                collideSometing = true;
            }
            if (other.CompareTag("Ground") || other.CompareTag("Road"))
            {
            }

            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out RaycastHit raycastHit, 45))
            {
                Debug.Log(raycastHit.collider.gameObject.name);
                if(Vector3.Distance(raycastHit.collider.transform.position, gameObject.transform.position) < gameObject.GetComponent<Collider>().bounds.size.y)
                {
                    Debug.Log("������� �������");
                    controller.airplaneState = SimpleAirPlaneController.AirplaneState.Landing;
                }
            }
        }
    }
}