using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declaração da interface IInteractable.
public interface IInteractable
{
    // Este método é chamado quando o objeto é "hovered" (quando o cursor ou outra interação está sobre ele).
    void OnHover();

    // Este método é chamado quando o objeto não está mais sendo "hovered" (quando o cursor ou outra interação sai dele).
    void OnUnhover();

    // Este método é chamado quando ocorre uma interação de clique no objeto.
    void OnClick();
}
