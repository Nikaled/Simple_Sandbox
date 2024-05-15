using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectVariants : MonoBehaviour
{
    [SerializeField] protected Texture[] Variants;
    [SerializeField] protected MeshRenderer ObjectToChange;
    [HideInInspector] public int currentVariantIndex;

    public virtual void ChangeTextures(Texture textures)
    {

            for (int i = 0; i < ObjectToChange.materials.Length; i++)
            {
                ObjectToChange.materials[i].mainTexture = textures;
            }
    }
    
    public virtual void FindTextureIndex()
    {
        for (int i = 0; i < Variants.Length; i++)
        {
            if (ObjectToChange.materials[0].mainTexture == Variants[i])
            {
                Debug.Log("currentVariantIndex:"+i);
                currentVariantIndex = i;
            }
        }
        ChangeTextureManager.instance.ChangeVariantText(currentVariantIndex);
    }
    public virtual void ChangeTextures(int PlusOrMinusOne)
    {
        currentVariantIndex += PlusOrMinusOne;
        if(currentVariantIndex < 0)
        {
            currentVariantIndex = Variants.Length-1;
        }
        if(currentVariantIndex >= Variants.Length)
        {
            currentVariantIndex = 0;
        }
        ChangeTextures(Variants[currentVariantIndex]);
        ChangeTextureManager.instance.ChangeVariantText(currentVariantIndex);
    }
}
