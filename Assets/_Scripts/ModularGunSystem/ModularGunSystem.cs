using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModularGunSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] sights;
    [SerializeField] private GameObject[] muzzles;
    [SerializeField] private GameObject[] grips;

    private Dictionary<string, GameObject[]> attachmentGroups = new Dictionary<string, GameObject[]>();

    private void Start()
    {
        attachmentGroups.Add("Sight", sights);
        attachmentGroups.Add("Muzzle", muzzles);
        attachmentGroups.Add("Grip", grips);
    }

    public void ActivateAttachment(string type, int id)
    {
        if (attachmentGroups.ContainsKey(type))
        {
            GameObject[] attachments = attachmentGroups[type];

            DeactivateAllAttachments(attachments);

            if (id >= 0 && id < attachments.Length && attachments[id] != null)
            {
                attachments[id].SetActive(true);
            }
            else if (id == 10)
            {
                DeactivateAllAttachments(attachments);
            }
        }
    }

    private void DeactivateAllAttachments(GameObject[] attachments)
    {
        foreach (GameObject attachment in attachments)
        {
            if (attachment != null)
            {
                attachment.SetActive(false);
            }
        }
    }

    public void RandomizeAttachments()
    {
        RandomAttachmentForType("Sight");
        RandomAttachmentForType("Muzzle");
        RandomAttachmentForType("Grip");
    }

    private void RandomAttachmentForType(string type)
    {
        if (attachmentGroups.ContainsKey(type))
        {
            GameObject[] attachments = attachmentGroups[type];

            int randomId = Random.Range(0, attachments.Length + 1); // +1 to include ID 4: No attachment id

            if (randomId == attachments.Length)
            {
                ActivateAttachment(type, 10); 
            }
            else
            {
                ActivateAttachment(type, randomId);
            }
        }
    }

    public void ActivateSight(int id)
    {
        ActivateAttachment("Sight", id);
    }

    public void ActivateMuzzle(int id)
    {
        ActivateAttachment("Muzzle", id);
    }

    public void ActivateGrip(int id)
    {
        ActivateAttachment("Grip", id);
    }
}
