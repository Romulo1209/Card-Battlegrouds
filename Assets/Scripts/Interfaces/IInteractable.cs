using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Declara��o da interface IInteractable.
public interface IInteractable
{
    // Este m�todo � chamado quando o objeto � "hovered" (quando o cursor ou outra intera��o est� sobre ele).
    void OnHover();

    // Este m�todo � chamado quando o objeto n�o est� mais sendo "hovered" (quando o cursor ou outra intera��o sai dele).
    void OnUnhover();

    // Este m�todo � chamado quando ocorre uma intera��o de clique no objeto.
    void OnClick();
}
