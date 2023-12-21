using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCube : MonoBehaviour
{
    public List<GameObject> deleteQueue = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < deleteQueue.Count; i++) //Creates a queue to delete items
        {
            Debug.Log("Object: |" + deleteQueue[i].name + "| is being deleted!");
            GameObject @object = deleteQueue[i];
            if (@object.CompareTag("Map"))
            {
                PrefabManager.prefabManager.currentPrefabs.RemoveAt(i);
            }
            Destroy(@object);
            deleteQueue.RemoveAt(i);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("something Entered me!");
        deleteQueue.Add(other.gameObject);
    }
}
