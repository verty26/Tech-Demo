using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillTreeManager : MonoBehaviour
{
    public static SkillTreeManager instance;

    [SerializeField] private List<Skill> skills = new List<Skill>();

    public int skillPoints;
    [SerializeField] private TextMeshProUGUI skillPointsText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        skillPointsText.text = "Skill Points: " + skillPoints.ToString(); 
    }

    public void ResetSkills()
    {
        foreach(Skill skill in skills)
        {
            skill.RefundSkill();
        }

        skills[0].isUnlockable = true;
    }
}
