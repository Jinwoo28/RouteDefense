[System.Serializable]
public class ActiveForm
{
    public string SkillName;
    public float Value;
    public int Price;
    public int CurrentLevel;
    public int MaxLevel;
    public string PreSkill;
    public float UpValueRate;
    public int UnLock;
    public string BundleName;
    public string SkillInformation;
    public int Slot;
    public float CoolTime;
    public float CoolTimeDown;


    private int OriginPrice;
    public void UnLockSkill()
    {
        OriginPrice = Price;
        GetCheckLock = 1;
        UnLock = 1;
    }

    public int GetCheckLock { get; set; }

    public void LevelUp()
    {
        CurrentLevel++;
        Value += UpValueRate;
        Price += OriginPrice;
        CoolTime -= CoolTimeDown;
    }
}
