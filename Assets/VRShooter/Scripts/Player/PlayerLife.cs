using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{

    public float MaxHealth = 100;
    public float CurrentHealth;
    public Slider HealthSlider;
    public Image DamageImage;
    public float FlashSpeed = 5f;
    public Color FlashColor = new Color(1f, 0f, 0f, 0.1f);
    private GameObject LaserGun;
    private GameObject TeleportationBall;


    // Use this for initialization
    void Start ()
    {
        CurrentHealth = MaxHealth;
        HealthSlider.maxValue = MaxHealth;
        HealthSlider.value = CurrentHealth;
        LaserGun = GameObject.FindGameObjectWithTag("Weapon");
        TeleportationBall = GameObject.FindGameObjectWithTag("UtilityBall");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Hit(float damage)
    {
        DamageImage.color = FlashColor;
        CurrentHealth -= damage;
        HealthSlider.value = CurrentHealth;
        DamageImage.color = Color.Lerp(DamageImage.color, Color.clear, FlashSpeed * Time.deltaTime);
        if (CurrentHealth <= 0)
        {
            GameOver();    
        }
    }

    void GameOver()
    {
        //TODO What should be done when the Player dies
        foreach (var o in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            o.SendMessage("GameOver");
        }
        GameObject.FindGameObjectWithTag("HUD").GetComponent<Animator>().SetTrigger("GameOver");
        LaserGun.SetActive(false);
        TeleportationBall.SetActive(false);
    }
}
