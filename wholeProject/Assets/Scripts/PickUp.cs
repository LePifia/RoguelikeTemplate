using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickUpType
{
    coin,
    Health
}
public class PickUp : MonoBehaviour
{

    [SerializeField] PickUpType type;
    [SerializeField] int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (type == PickUpType.coin)
            {
                collision.GetComponent<Player>().AddCoins(value);
                Destroy(gameObject);
            }

            if (type == PickUpType.Health)
            {
                collision.GetComponent <Player>().RecoverLife(value);
                Destroy (gameObject);
            }
        }

        
    }
}
