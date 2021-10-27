using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class DisableOutsideRange : MonoBehaviour
    {
        private GameObject ItemActivatorObject;
        private ItemActivator activationScript;
        // Start is called before the first frame update
        void Start()
        {
            ItemActivatorObject = GameObject.Find("ItemActivatorObj");
            activationScript = ItemActivatorObject.GetComponent<ItemActivator>();
            StartCoroutine("AddToList");
        }
        IEnumerator AddToList()
        {
            yield return new WaitForSeconds(0.1f);
            //activationScript.activatorItems.Add(new ActivatorItem { item = this.gameObject, itemPos = transform.position });
            activationScript.addList.Add(new ActivatorItem { item = this.gameObject, itemPos = transform.position });
        }
    }
}
