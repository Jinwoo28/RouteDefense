
[System.Serializable]
public class PassiveForm
{
    public string SkillName;
    public int Value;
    public int Price;
    public int CurrentLevel;
    public int MaxLevel;
    public string PreSkill;
    public int UpValueRate;
    public int UnLock;
    public string BundleName;

    public void UnLockSkill()
    {
        UnLock = 1;
    }

    public void LevelUp()
    {
        
    }
}
