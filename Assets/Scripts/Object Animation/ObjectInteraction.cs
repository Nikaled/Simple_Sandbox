using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteraction : MonoBehaviour
{
    private bool _isPlayerNear;
    private bool _objectActivated;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.instance.ShowObjectInteructInstruction(true);
            _isPlayerNear = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CanvasManager.instance.ShowObjectInteructInstruction(false);
            _isPlayerNear = false;
        }
    }

    private void Update()
    {
        if(_isPlayerNear == false)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_objectActivated==false)
            {
            ActivateObject();
                _objectActivated = true;
                return;
            }
            else
            {
                DeactivateObject();
                _objectActivated = false;
                return;
            }
        }
    }
    protected virtual void ActivateObject()
    {

    }
    protected virtual void DeactivateObject()
    {

    }
}
