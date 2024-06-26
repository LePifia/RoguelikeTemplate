using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bool keyActive = collision.GetComponent<Player>().HasTheKey();

            if (keyActive == true)
            {
                GameManager.instance.GoToNextLevel();
            }
        }
    }
}
