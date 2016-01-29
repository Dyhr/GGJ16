using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Clock>().Running = false;
            other.transform.parent.GetComponent<Player>().Control = false;
            Debug.Log("You Win");
        }
    }
}
