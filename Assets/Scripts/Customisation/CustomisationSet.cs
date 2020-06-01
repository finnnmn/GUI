using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomisationSet : BaseStats
{
    #region variables
    [Header("Character Name")]
    public string characterName;
    [Header("Character Class")]
    public CharacterClass charClass = CharacterClass.Barbarian;
    public string[] selectedClass = new string[8];
    public int selectedIndex = 0;

    [Header("Stat points")]
    public int statPoints = 10;
    public Text pointsText;

    [System.Serializable]
    public struct StatBlock
    {
        public Text statText;
        public GameObject increase;
        public GameObject decrease;
    }
    public StatBlock[] statBlock = new StatBlock[6];
    
    [Header("Dropdown")]
    public Dropdown classDropdown;

    [Header("Texture List")]
    public List<Texture2D> skin = new List<Texture2D>();
    public List<Texture2D> hair = new List<Texture2D>();
    public List<Texture2D> eyes = new List<Texture2D>();
    public List<Texture2D> mouth = new List<Texture2D>();
    public List<Texture2D> clothes = new List<Texture2D>();
    public List<Texture2D> armour = new List<Texture2D>();

    [Header("Index")]
    //index numbers for current textures
    public int skinIndex;
    public int hairIndex, eyesIndex, mouthIndex, clothesIndex, armourIndex;

    [Header("Renderer")]
    //renderer for character mesh
    public Renderer characterRenderer;

    [Header("Max Index")]
    //max index of texture lists
    public int skinMax;
    public int hairMax, eyesMax, mouthMax, clothesMax, armourMax;


    #endregion

    #region start
    private void Start()
    {
        #region for loop pull textures from file
        #region skin
        for (int i = 0; i < skinMax; i++)
        {
            //temp texture2D that grabs textures using Resources.Load from the character file looking for Skin_i
            Texture2D temp = Resources.Load("Character/Skin_" + i) as Texture2D;
            //add out temp texture to the skin list
            skin.Add(temp);
        }
        #endregion
        #region hair
        for (int i = 0; i < hairMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Hair_" + i) as Texture2D;
            hair.Add(temp);
        }
        #endregion
        #region eyes
        for (int i = 0; i < eyesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Eyes_" + i) as Texture2D;
            eyes.Add(temp);
        }
        #endregion
        #region mouth
        for (int i = 0; i < mouthMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Mouth_" + i) as Texture2D;
            mouth.Add(temp);
        }
        #endregion
        #region clothes
        for (int i = 0; i < clothesMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Clothes_" + i) as Texture2D;
            clothes.Add(temp);
        }
        #endregion
        #region armour
        for (int i = 0; i < armourMax; i++)
        {
            Texture2D temp = Resources.Load("Character/Armour_" + i) as Texture2D;
            armour.Add(temp);
        }
        #endregion
        #endregion

        characterRenderer = GameObject.FindGameObjectWithTag("CharacterMesh").GetComponent<Renderer>();

        SetRandomTextures();

        selectedClass = new string[] { "Barbarian", "Bard", "Druid", "Monk", "Paladin", "Ranger", "Sorcerer", "Warlock" };

        List<string> tempName = new List<string> { "Strength", "Dexterity", "Constitution", "Wisdom", "Intelligence", "Charisma" };
        for (int i = 0; i < tempName.Count; i++)
        {
            characterStats[i].name = tempName[i];
            characterStats[i].tempValue = characterStats[i].value;
        }
        tempName = new List<string> { "Barbarian", "Bard", "Druid", "Monk", "Paladin", "Ranger", "Sorcerer", "Warlock" };
        classDropdown.ClearOptions();
        classDropdown.AddOptions(tempName);
        ChooseClass(0);

        characterName = "Adventurer";
    }
    #endregion

    #region set texture
    public void SetTexture(string type, int dir)
    {
        int index = 0, max = 0, matIndex = 0;
        Texture2D[] textures = new Texture2D[0];

        #region Switch material and values
        switch (type)
        {

            case "Skin":
                index = skinIndex;
                max = skinMax;
                textures = skin.ToArray();
                matIndex = 1;
                break;
            case "Eyes":
                index = eyesIndex;
                max = eyesMax;
                textures = eyes.ToArray();
                matIndex = 2;
                break;
            case "Mouth":
                index = mouthIndex;
                max = mouthMax;
                textures = mouth.ToArray();
                matIndex = 3;
                break;
            case "Hair":
                index = hairIndex;
                max = hairMax;
                textures = hair.ToArray();
                matIndex = 4;
                break;
            case "Clothes":
                index = clothesIndex;
                max = clothesMax;
                textures = clothes.ToArray();
                matIndex = 5;
                break;
            case "Armour":
                index = armourIndex;
                max = armourMax;
                textures = armour.ToArray();
                matIndex = 6;
                break;
            default:
                break;
        }
        #endregion

        #region assign direction
        //set index
        index += dir;

        if (index >= max)
        {
            index -= max;
        }
        else if (index < 0)
        {
            index = max - 1;
        }

        //get material array
        Material[] mat = characterRenderer.materials;

        //set texture
        mat[matIndex].mainTexture = textures[index];

        //set material array with updated texture
        characterRenderer.materials = mat;

        #endregion

        #region set material switch
        switch (type)
        {
            case "Skin":
                skinIndex = index;
                break;
            case "Eyes":
                eyesIndex = index;
                break;
            case "Mouth":
                mouthIndex = index;
                break;
            case "Hair":
                hairIndex = index;
                break;
            case "Clothes":
                clothesIndex = index;
                break;
            case "Armour":
                armourIndex = index;
                break;
        }
        #endregion
    }

    public void SetTexturePos(string type)
    {
        SetTexture(type, 1);
    }

    public void SetTextureNeg(string type)
    {
        SetTexture(type, -1);
    }

    public void SetRandomTextures()
    {
        SetTexture("Skin", Random.Range(0, skinMax));
        SetTexture("Hair", Random.Range(0, hairMax));
        SetTexture("Mouth", Random.Range(0, mouthMax));
        SetTexture("Eyes", Random.Range(0, eyesMax));
        SetTexture("Clothes", Random.Range(0, clothesMax));
        SetTexture("Armour", Random.Range(0, armourMax));
    }

    public void ResetTextures()
    {
        SetTexture("Skin", -skinIndex);
        SetTexture("Hair", -hairIndex);
        SetTexture("Mouth", -mouthIndex);
        SetTexture("Eyes", -eyesIndex);
        SetTexture("Clothes", -clothesIndex);
        SetTexture("Armour", -armourIndex);
    }
    #endregion

    #region chooseClass
    public void ChooseClass(int classIndex)
    {
        switch (classIndex)
        {
            case 0:
                characterStats[0].value = 15;
                characterStats[1].value = 13;
                characterStats[2].value = 14;
                characterStats[3].value = 10;
                characterStats[4].value = 12;
                characterStats[5].value = 8;
                charClass = CharacterClass.Barbarian;
                break;
            case 1:
                characterStats[0].value = 10;
                characterStats[1].value = 14;
                characterStats[2].value = 8;
                characterStats[3].value = 12;
                characterStats[4].value = 13;
                characterStats[5].value = 15;
                charClass = CharacterClass.Bard;
                break;
            case 2:
                characterStats[0].value = 12;
                characterStats[1].value = 14;
                characterStats[2].value = 10;
                characterStats[3].value = 15;
                characterStats[4].value = 13;
                characterStats[5].value = 8;
                charClass = CharacterClass.Druid;
                break;
            case 3:
                characterStats[0].value = 15;
                characterStats[1].value = 12;
                characterStats[2].value = 13;
                characterStats[3].value = 10;
                characterStats[4].value = 14;
                characterStats[5].value = 8;
                charClass = CharacterClass.Monk;
                break;
            case 4:
                characterStats[0].value = 14;
                characterStats[1].value = 10;
                characterStats[2].value = 15;
                characterStats[3].value = 12;
                characterStats[4].value = 8;
                characterStats[5].value = 13;
                charClass = CharacterClass.Paladin;
                break;
            case 5:
                characterStats[0].value = 14;
                characterStats[1].value = 15;
                characterStats[2].value = 12;
                characterStats[3].value = 8;
                characterStats[4].value = 10;
                characterStats[5].value = 13;
                charClass = CharacterClass.Ranger;
                break;
            case 6:
                characterStats[0].value = 8;
                characterStats[1].value = 12;
                characterStats[2].value = 10;
                characterStats[3].value = 14;
                characterStats[4].value = 15;
                characterStats[5].value = 13;
                charClass = CharacterClass.Sorcerer;
                break;
            case 7:
                characterStats[0].value = 8;
                characterStats[1].value = 10;
                characterStats[2].value = 13;
                characterStats[3].value = 15;
                characterStats[4].value = 14;
                characterStats[5].value = 12;
                charClass = CharacterClass.Warlock;
                break;

        }
        selectedIndex = classIndex;
        ResetStats();


    }
    #endregion

    #region statpoints

    public void UpdateStatBlock()
    {
        pointsText.text = ("Points: " + statPoints);
        for (int i = 0; i < characterStats.Length; i++)
        {
            statBlock[i].statText.text = (characterStats[i].name + ": " + characterStats[i].tempValue);
            if (characterStats[i].tempValue == characterStats[i].value)
            {
                statBlock[i].statText.color = new Color32(0, 0, 0, 255);
                statBlock[i].decrease.SetActive(false);
            }
            else
            {
                statBlock[i].statText.color = new Color32(255, 0, 0, 255);
                statBlock[i].decrease.SetActive(true);
            }

            if (statPoints == 0)
            {
                statBlock[i].increase.SetActive(false);
            }
            else
            {
                statBlock[i].increase.SetActive(true);
            }

        }


    }
    public void IncreaseStat(int index)
    {
        if (statPoints > 0)
        {
            statPoints -= 1;
            characterStats[index].tempValue += 1;
            UpdateStatBlock();
        }
    }
    public void DecreaseStat(int index)
    {
        if (characterStats[index].tempValue > characterStats[index].value)
        {
            statPoints += 1;
            characterStats[index].tempValue -= 1;
            UpdateStatBlock();
        }
    }

    public void ResetStats()
    {
        for (int i = 0; i < characterStats.Length; i++)
        {
            characterStats[i].tempValue = characterStats[i].value;
        }
        statPoints = 10;
        UpdateStatBlock();
    }


    #endregion

    #region characterName
    public void ChangeCharactername(string name)
    {
        characterName = name;
    }
    #endregion

    #region saving
    public void CreateCharacter()
    {
        //Create the character data
        PlayerData data = new PlayerData
        {
            playerName = characterName,
            playerClass = charClass.ToString(),
            level = 1,
            pX = 290,
            pY = 3,
            pZ = 70,

            customIndex = new int[6],
            characterStats = new int[6],
            lifeValue = new float[3]
        };
        data.customIndex[0] = skinIndex;
        data.customIndex[1] = hairIndex;
        data.customIndex[2] = eyesIndex;
        data.customIndex[3] = mouthIndex;
        data.customIndex[4] = clothesIndex;
        data.customIndex[5] = armourIndex;

        for (int i = 0; i < characterStats.Length; i++)
        {
            data.characterStats[i] = characterStats[i].tempValue;
        }
        for (int i = 0; i < characterStatus.Length; i++)
        {
            data.lifeValue[i] = 100;
        }

        //save data to file
        PlayerBinary.SaveNewData(data);

        //Load game
        SceneManager.LoadScene(2);
    }
    #endregion

}

public enum CharacterClass
{
    Barbarian,
    Bard,
    Druid,
    Monk,
    Paladin,
    Ranger,
    Sorcerer,
    Warlock
}
