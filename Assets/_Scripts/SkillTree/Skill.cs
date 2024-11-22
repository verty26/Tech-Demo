using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Skill : MonoBehaviour, IPointerDownHandler
{
    public bool isUnlockable = false;
    public bool isUnlocked = false;

    private Image skillImage;

    [SerializeField] private Image[] bounds;

    [Header("Bounded Skills")]
    [SerializeField] private Skill skillBefore;
    [SerializeField] private Skill[] skillsAfter;

    private void Awake()
    {
        skillImage = GetComponent<Image>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isUnlockable)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (SkillTreeManager.instance.skillPoints > 0 && !isUnlocked)
                {
                    isUnlocked = true;
                    skillImage.color = Color.white;

                    foreach (Image image in bounds)
                    {
                        image.color = Color.white;
                    }

                    foreach(Skill skill in skillsAfter)
                    {
                        skill.isUnlockable = true;
                        skill.skillImage.color = new Color(0.5f, 0.5f, 0.5f);
                    }

                    SkillTreeManager.instance.skillPoints--;

                    SkillTreeManager.instance.UpdateUI();
                }
                else
                {
                    Debug.Log("Not enough skill points or skill is already unlocked.");
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right && isUnlocked)
            {
                foreach(Skill skill in skillsAfter)
                {
                    if (skill.isUnlocked)
                    {
                        return;
                    }
                }

                isUnlocked = false;

                skillImage.color = new Color(0.29f, 0.29f, 0.29f);

                foreach (Image image in bounds)
                {
                    image.color = new Color(0.29f, 0.29f, 0.29f);
                }

                

                SkillTreeManager.instance.skillPoints++;

                SkillTreeManager.instance.UpdateUI();
            }
        }
    }

    public void RefundSkill()
    {
        if (!isUnlocked) return;
        isUnlocked = false;
        isUnlockable = false;

        skillImage.color = new Color(0.29f, 0.29f, 0.29f);

        foreach (Image image in bounds)
        {
            image.color = new Color(0.29f, 0.29f, 0.29f);
        }



        SkillTreeManager.instance.skillPoints++;

        SkillTreeManager.instance.UpdateUI();
    }
}
