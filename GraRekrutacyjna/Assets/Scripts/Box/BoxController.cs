using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public float bounceForce = 8f;

    private void OnTriggerEnter(Collider collider)
    {
        
        if (collider.CompareTag("Player"))
        {
            PlayerBounce(collider.GetComponent<Rigidbody>());
            DestroyBox();
        }
    }

    private void PlayerBounce(Rigidbody playerRB)
    {
        playerRB.velocity = new Vector3(playerRB.velocity.x, bounceForce, playerRB.velocity.z);
    }

    private void DestroyBox()
    {

        Destroy(gameObject);
        FindObjectOfType<GameMenager>().ShowCongratulations();
    }
}
