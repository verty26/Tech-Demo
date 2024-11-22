using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularClothingSystem : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] allRenderers;

    [SerializeField] private SkinnedMeshRenderer beardMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer hairMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer shirtMeshRenderer;
    [SerializeField] private SkinnedMeshRenderer glassesMeshRenderer;
    [SerializeField] private Mesh[] beards;
    [SerializeField] private Mesh[] hairs;
    [SerializeField] private Mesh[] tshirts;
    [SerializeField] private Mesh[] glasses;

    [SerializeField] private Material[] materials;

    public void Randomize()
    {
        int randomBeard = Random.Range(0, beards.Length);

        beardMeshRenderer.sharedMesh = beards[randomBeard];

        int randomHair = Random.Range(0, beards.Length);

        hairMeshRenderer.sharedMesh = hairs[randomHair];

        int randomShirt = Random.Range(0, beards.Length);

        shirtMeshRenderer.sharedMesh = tshirts[randomShirt];

        int randomGlasses = Random.Range(0, glasses.Length);

        glassesMeshRenderer.sharedMesh = glasses[randomGlasses];

        int randomMaterial = Random.Range(0, materials.Length);

        foreach(SkinnedMeshRenderer renderer in allRenderers)
        {
            renderer.material = materials[randomMaterial];
        }
    }
}
