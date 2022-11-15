using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BaShen", order = 1)]
public class BaShenSO : ScriptableObject
{
    public List<Sprite> idle;
    public List<Sprite> run;
    public List<Sprite> jump;
}
