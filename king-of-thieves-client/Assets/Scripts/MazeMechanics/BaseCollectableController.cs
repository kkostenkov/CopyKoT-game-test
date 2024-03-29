using UnityEngine;

namespace MazeMechanics
{
    public class BaseCollectableController : MonoBehaviour
    {
        public void OnTriggerStay2D(Collider2D col)
        {
            Debug.Log("InCollectable trigger");
            this.gameObject.SetActive(false);
        }
    }
}