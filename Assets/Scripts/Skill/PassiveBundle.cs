using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class PassiveBundle : ScriptableObject
{
	public List<PassiveForm> PassiveSkill; // Replace 'EntityType' to an actual type that is serializable.
}
