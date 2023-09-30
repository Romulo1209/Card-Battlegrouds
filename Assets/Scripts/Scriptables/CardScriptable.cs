using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Atributos que determinam onde os objetos deste tipo podem ser criados no Editor Unity
[CreateAssetMenu(fileName = "Card", menuName = "YAW Teste/Create Card", order = 1)]
public class CardScriptable : ScriptableObject
{
    [Header("Card Basic Infos")]
    public string CardName; // Nome da carta
    public Sprite CardIcon; // Ícone da carta (geralmente uma imagem)
    public int CardHealth; // Pontos de vida da carta
    public int CardCost; // Custo em mana para jogar esta carta
    public int CardWalkDistance; // Distância que a carta pode percorrer

    [Header("Card Damage Infos")]
    public int CardDamage; // Dano que a carta pode causar
    public int CardAttackDistance; // Distância de ataque da carta

    [Header("References")]
    public GameObject CharacterPrefab; // Prefab do personagem associado a esta carta
}
