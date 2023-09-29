using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Castle : MonoBehaviour
{
    [Header("Castle Parameters")]
    [SerializeField] Players castleOwner;
    [SerializeField] GameObject castleParent;

    [Header("Castle Rferences")]
    [SerializeField] Image castleLifeFill;
    [SerializeField] TMP_Text castleLifeText;

    public void SetupCastle(Players owner) {
        castleOwner = owner;
        if (owner == Players.Bot)
            castleParent.transform.Rotate(new Vector3(0, -180, 0), Space.Self);

        UpdateLife();
    }

    public void UpdateLife() {
        castleLifeFill.fillAmount = (float)GameController.instance.GetPlayerLife(castleOwner) / 15;
        castleLifeText.text = $"{GameController.instance.GetPlayerLife(castleOwner)}/15";
    }
}
