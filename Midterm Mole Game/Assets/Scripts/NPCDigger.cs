using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDigger : MonoBehaviour
{
    //Determines whether or not the NPC in question can dig
    public bool CanDig = true;

    void Update()
    {
        //Keeps in the same location as parent object, since it wasn't doing that already
        transform.position = transform.parent.position;
    }

    //Destroys dirt
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CanDig && collision.gameObject.CompareTag("Dirt"))
        {
            collision.gameObject.SendMessage("DestroyDirt");
        }
    }
}
