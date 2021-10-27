using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class ItemActivator : MonoBehaviour
    {
        [SerializeField]
        private int distanceFromPlayer;
        [SerializeField] private GameObject Player;
        private List<ActivatorItem> activatorItems;
        public List<ActivatorItem> addList;
        // Start is called before the first frame update
        void Start()
        {
            activatorItems = new List<ActivatorItem>();
            addList = new List<ActivatorItem>();
            //StartCoroutine("CheckActivation");
            AddToList();
        }
        void AddToList()
        {
            if (addList.Count > 0)
            {
                foreach (ActivatorItem item in addList)
                {
                    if (item.item != null)
                    {
                        activatorItems.Add(item);
                    }
                }

                addList.Clear();
            }
            StartCoroutine("CheckActivation");
        }
        IEnumerator CheckActivation()
        {
            List<ActivatorItem> removeList = new List<ActivatorItem>();
            if (activatorItems.Count > 0)
            {
                //Debug.Log("activatorItems Count: " + activatorItems.Count);
                foreach (ActivatorItem item in activatorItems)
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                    }
                    else
                    {
                        if (Vector3.Distance(Player.transform.position, item.itemPos) > distanceFromPlayer)
                        {
                            activatorItems.Remove(item);
                            if (item.item == null)
                            {
                                removeList.Add(item);
                            }
                            else
                            {
                                item.item.SetActive(false);
                            }
                        }
                        else
                        {
                            if (item.item == null)
                            {
                                removeList.Add(item);
                            }
                            else
                            {
                                item.item.SetActive(true);
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.01f);
                }
            }
            yield return new WaitForSeconds(0.01f);
            if (removeList.Count > 0)
            {
                foreach (ActivatorItem item in removeList)
                {
                    activatorItems.Remove(item);
                }
            }
            yield return new WaitForSeconds(0.05f);
            //StartCoroutine("CheckActivation");
            AddToList();
        }
    }

    public class ActivatorItem
    {
        public GameObject item;
        public Vector3 itemPos;
    }
}
