using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Castle : MonoBehaviour
{
    [Header("Castle Parameters")]
    [SerializeField] Players castleOwner; // O dono do castelo (jogador ou bot)
    [SerializeField] GameObject castleParent; // O objeto pai do castelo

    [Header("Castle References")]
    [SerializeField] Image castleLifeFill; // Referência à barra de preenchimento da vida do castelo
    [SerializeField] TMP_Text castleLifeText; // Referência ao texto que exibe a vida do castelo

    public void SetupCastle(Players owner) {
        castleOwner = owner; // Define o dono do castelo com base no parâmetro

        // Inverte a rotação do castelo se pertence ao bot para que ele fique virado para o jogador
        if (owner == Players.Bot)
            castleParent.transform.Rotate(new Vector3(0, -180, 0), Space.Self);

        UpdateLife(); // Atualiza a exibição da vida do castelo
    }

    public void UpdateLife() {
        // Atualiza a barra de preenchimento da vida do castelo com base na vida atual do jogador ou bot
        castleLifeFill.fillAmount = (float)GameController.instance.GetPlayerLife(castleOwner) / 15;

        // Atualiza o texto que exibe a vida do castelo
        castleLifeText.text = $"{GameController.instance.GetPlayerLife(castleOwner)}/15";
    }
}
