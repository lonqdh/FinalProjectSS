using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill Database", menuName = "Skill System/Skill Database")]
public class SkillDatabase : ScriptableObject
{
    public List<SkillData> allSkillsList;
}
