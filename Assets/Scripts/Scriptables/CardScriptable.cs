using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "YAW Teste/Create Card", order = 1)]
public class CardScriptable : ScriptableObject
{
    [Header("Card Basic Infos")]
    public string CardName;
    public Sprite CardIcon;
    public int CardHealth;
    public int CardCost;
    public int CardWalkDistance;

    [Header("Card Damage Infos")]
    public int CardDamage;
    public int CardAttackDistance;

    [Header("References")]
    public GameObject CharacterPrefab;
}
