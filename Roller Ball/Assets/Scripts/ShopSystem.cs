using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public Button randomButton;

    public item balls;
    public item cylinders;


    private void Start()
    {
        Init(balls);
        Init(cylinders);
    }

    void Init(item it)
    {
        foreach (SubItems t in it.subItems)
        {
            //Unlock first Item
            if (t.id == 1)
            {
                t.isUnlocked = true;
                t.isSelected = true;
                t.SaveData(it);
            }

            //Load the saved data and asign it to the UI
            t.LoadData(it);
            if (!t.isUnlocked)
            {
                t.itemParent.GetComponent<Image>().sprite = it.unUsedSprite;
                if (t.isOpenWithLevel)
                    t.itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Level " + t.unlockLevel);
                else
                    t.itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Use Random");
            }
            else if(t.isUnlocked && t.isSelected)
            {
                
                it.subItems[0].isSelected = false;
                it.subItems[0].itemParent.image.sprite = it.unUsedSprite;
                it.subItems[0].SaveData(it);

                t.itemParent.GetComponent<Image>().sprite = it.usedSprite;
                t.itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("");
                it.itemGameObject.material = t.mat;

            }
            else
            {
                t.itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("");
                t.itemParent.GetComponent<Image>().sprite = it.unUsedSprite;
            }



            //Active whene change to other Item
            t.itemParent.onClick.AddListener(() =>
            {
                if (t.isUnlocked)
                {
                    foreach (SubItems b in it.subItems)
                    {
                        b.isSelected = false;
                        b.SaveData(it);
                        b.itemParent.image.sprite = it.unUsedSprite;
                    }

                    t.isSelected = true;
                    t.SaveData(it);
                    it.itemGameObject.material = t.mat;
                    t.itemParent.image.sprite = it.usedSprite;
                }
                else if (!t.isUnlocked && t.isOpenWithLevel)
                {
                    t.itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Level " + t.unlockLevel);
                }
                else
                {
                    t.itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Use Random");
                }
            });

            //Check if the SubItem unlocke with random
            if (!t.isOpenWithLevel)
                it.numberOfItemsToUnlockWithRandom++;
            if (t.isOpenWithLevel == false && t.isUnlocked == true)
                it.numberOfUnlockedItemsWithRandom++;
        }
    }


    void Update()
    {
        UpdateFun(balls);
        UpdateFun(cylinders);
    }

    void UpdateFun(item it)
    {
        foreach (SubItems t in it.subItems)
        {
            //Unlock Item whene reach the leve;
            if (t.isOpenWithLevel && t.unlockLevel <= GetComponent<LSystem>().currentLevel)
            {
                t.isUnlocked = true;
                t.SaveData(it);
            }
        }
    }


    int rand = 0;
    public void UnlockRandom(item it)
    {
        rand = Random.Range(1, it.subItems.Length);
        try
        {
            if (it.numberOfUnlockedItemsWithRandom < it.numberOfItemsToUnlockWithRandom)
            {
                if (it.subItems[rand].isUnlocked == true || it.subItems[rand].isOpenWithLevel == true)
                {
                    UnlockRandom(it);
                }
                else
                {
                    it.numberOfItemsToUnlockWithRandom = 0;
                    it.numberOfUnlockedItemsWithRandom = 0;


                    it.subItems[rand].isUnlocked = true;
                    foreach (SubItems b in it.subItems)
                    {
                        b.isSelected = false;
                        b.itemParent.image.sprite = it.unUsedSprite;
                        b.SaveData(it);


                        //Check if the SubItem unlocke with random
                        if (!b.isOpenWithLevel)
                            it.numberOfItemsToUnlockWithRandom++;
                        if (b.isOpenWithLevel == false && b.isUnlocked == true)
                            it.numberOfUnlockedItemsWithRandom++;
                    }

                    it.subItems[rand].isSelected = true;
                    
                    it.subItems[rand].SaveData(it);
                    it.subItems[rand].itemParent.image.sprite = it.usedSprite;
                    it.subItems[rand].itemParent.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("");
                    it.itemGameObject.material = it.subItems[rand].mat;
                }
            }
            else
            {
                //meesageText.SetText("All Random " + it.itemName + " Are Unlocked ");
                randomButton.interactable = false;
            }
        }
        catch
        {
            //meesageText.SetText("All Random " + it.itemName + " Are Unlocked!");
        }


    }

}



[System.Serializable]
public class item
{
    public string itemName;
    public MeshRenderer itemGameObject;
    public Sprite usedSprite, unUsedSprite;
    public SubItems[] subItems;

    
    public int numberOfUnlockedItemsWithRandom, numberOfItemsToUnlockWithRandom;


}
[System.Serializable]
public class SubItems
{
    public int id;
    public Button itemParent;
    public Material mat;
    public bool isUnlocked;
    public bool isSelected;

    public bool isOpenWithLevel;
    public int unlockLevel;


    public void SaveData(item t)
    {
        PlayerPrefs.SetInt("isUnlocked" + t.itemName + id, isUnlocked == true ? 1 : 0);
        PlayerPrefs.SetInt("isSelected" + t.itemName + id, isSelected == true ? 1 : 0);
        //Debug.Log(t.itemName);
    }

    public void LoadData(item t)
    {
        isUnlocked = PlayerPrefs.GetInt("isUnlocked" + t.itemName + id) == 1 ? true : false;
        isSelected = PlayerPrefs.GetInt("isSelected" + t.itemName + id) == 1 ? true : false;
        //Debug.Log(t.itemName);
    }
}

