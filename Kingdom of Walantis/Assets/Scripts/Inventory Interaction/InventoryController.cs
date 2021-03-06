﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();
    public bool isInventoryOpen = false;
    [Header("UI Items")]
    public GameObject uiWindow;
    public Image[] itemsImages;
    [Header("UI Item Description")]
    public GameObject uiDescriptionWindow;
    public Image descriptionImage;
    public Text descriptionText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        uiWindow.SetActive(isInventoryOpen);
    }

    public void PickUp(GameObject item)
    {
        items.Add(item);
        item.transform.parent = gameObject.transform;
        UpdateUI();
    }

    private void UpdateUI()
    {
        HideItems();
        for (int i = 0; i < items.Count; i++)
        {
            //Debug.Log(items[i]);
            itemsImages[i].sprite = items[i].GetComponent<SpriteRenderer>().sprite;
            itemsImages[i].gameObject.SetActive(true);
        }
    }

    private void HideItems()
    {
        foreach (var item in itemsImages)
        {
            item.gameObject.SetActive(false);
        }
        HideDescription();
    }

    public void ShowDescription(int id)
    {
        descriptionImage.sprite = itemsImages[id].sprite;
        descriptionText.text = items[id].name + "\n\n" + items[id].GetComponent<Interactable>().descriptionText; ;
        descriptionImage.gameObject.SetActive(true);
        descriptionText.gameObject.SetActive(true);
    }

    public void HideDescription()
    {
        descriptionImage.gameObject.SetActive(false);
        descriptionText.gameObject.SetActive(false);
    }

    public void Consume(int id)
    {
        if (items[id].GetComponent<Interactable>().itemType == Interactable.ItemType.Consumable)
        {
            PowerUp(items[id].name);
            items[id].GetComponent<Interactable>().consumeEvent.Invoke();
            Destroy(items[id], 0.1f);
            items.RemoveAt(id);
            UpdateUI();
        }
    }

    public void PowerUp(string item)
    {
        switch (item)
        {
            case "Potion of Power":
                FindObjectOfType<PlayerController>().powerpoweredUp = true;
                FindObjectOfType<PlayerController>().powerLarge = false;
                break;
            case "Large Potion of Power":
                FindObjectOfType<PlayerController>().powerpoweredUp = true;
                FindObjectOfType<PlayerController>().powerLarge = true;
                break;
            case "Potion of Health":
                float playerHealth = FindObjectOfType<HealthController>().health;
                if (playerHealth >= 90) playerHealth = 100;
                else FindObjectOfType<HealthController>().health += 10;
                FindObjectOfType<HealthController>().UpdateBar();
                break;
            case "Large Potion of Health":
                float playerHealthLarge = FindObjectOfType<HealthController>().health;
                if (playerHealthLarge >= 80) playerHealth = 100;
                else FindObjectOfType<HealthController>().health += 20;
                FindObjectOfType<HealthController>().UpdateBar();
                break;
            case "Potion of Dash":
                FindObjectOfType<PlayerController>().dashpoweredUp = true;
                FindObjectOfType<PlayerController>().dashLarge = false;
                break;
            case "Large Potion of Dash":
                FindObjectOfType<PlayerController>().dashpoweredUp = true;
                FindObjectOfType<PlayerController>().dashLarge = true;
                break;
            case "Card of Armor":
                FindObjectOfType<HealthController>().armor += 5;
                break;
            case "Card of Sword":
                PlayerPrefs.SetInt("attackDamage",30);
                PlayerPrefs.SetInt("lightAttackDamage", 20);
                break;
            case "Card of Dash":
                PlayerPrefs.SetFloat("actionCooldown", 3.5f);
                break;
            default:
                break;
        }
    }

}
