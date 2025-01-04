using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerHP = 100;


    public void PlayerTakeDamage(int damagereceived)
    {
        playerHP -= damagereceived;

        if (playerHP <= 0)
        {
            Debug.Log("Player Dead");
        }
        else
        {
            Debug.Log("Player is being attacked: " + playerHP);
        }
    }
}
