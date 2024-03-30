using UnityEngine;

namespace Controllers
{
    public class BaseCollectableController : MonoBehaviour
    {
        public void OnTriggerStay2D(Collider2D col)
        {
            Debug.Log("InCollectable trigger");
            gameObject.SetActive(false);
        }
    }
}