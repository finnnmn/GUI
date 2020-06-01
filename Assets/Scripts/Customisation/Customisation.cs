using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Customisation : MonoBehaviour
{

    public SkinnedMeshRenderer mesh;

    [Header("Texture List")]
    public List<Texture2D> skin = new List<Texture2D>();
    public List<Texture2D> hair = new List<Texture2D>();
    public List<Texture2D> eyes = new List<Texture2D>();
    public List<Texture2D> mouth = new List<Texture2D>();
    public List<Texture2D> clothes = new List<Texture2D>();
    public List<Texture2D> armour = new List<Texture2D>();

    [Header("Max Index")]
    //max index of texture lists
    public int skinMax;
    public int hairMax, eyesMax, mouthMax, clothesMax, armourMax;

    private void Awake()
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
    }
    #region set texture
    public void SetTexture(string type, int index)
    {
        //Debug.Log(type + "/" + index);

        int matIndex = 0;
        Texture2D[] textures = new Texture2D[0];

        #region Switch material and values
        switch (type)
        {

            case "Skin":
                textures = skin.ToArray();
                matIndex = 1;
                break;
            case "Eyes":
                textures = eyes.ToArray();
                matIndex = 2;
                break;
            case "Mouth":
                textures = mouth.ToArray();
                matIndex = 3;
                break;
            case "Hair":
                textures = hair.ToArray();
                matIndex = 4;
                break;
            case "Clothes":
                textures = clothes.ToArray();
                matIndex = 5;
                break;
            case "Armour":
                textures = armour.ToArray();
                matIndex = 6;
                break;
            default:
                break;
        }
        #endregion

        

        //get material array
        Material[] mat = mesh.materials;

        //set texture
        mat[matIndex].mainTexture = textures[index];

        //set material array with updated texture
        mesh.materials = mat;

        #endregion
    }

}
