using UnityEngine;

/// <summary>
/// CÂMERA QUE ACOMPANHA A AÇÃO (colocar na Main Camera ortográfica).
///
/// Segue o objeto "FocoAtual" do GerenciadorMaquina só no eixo X, com suavização,
/// e trava nos limites xMin/xMax para que a máquina inteira apareça de ponta a ponta
/// (rampa 1 à esquerda, bandeira à direita). Mexer na câmera não afeta a física.
///
/// Valores para Orthographic Size = 6 e janela Game em 16:9.
/// </summary>
public class CameraSegue : MonoBehaviour
{
    public float xMin = 9.7f;        // mostra desde x = -1 (começo da rampa 1)
    public float xMax = 30.5f;       // mostra até x = 41 (bandeira)
    public float alturaY = 3.5f;     // centro vertical da imagem
    public float distanciaZ = -10f;  // a câmera fica "na frente" do cenário
    public float suavizacao = 3f;    // quanto maior, mais rápido a câmera acompanha

    // LateUpdate: roda depois de tudo ter se movido no frame (evita tremida).
    void LateUpdate()
    {
        GerenciadorMaquina g = GerenciadorMaquina.Instancia;
        Transform foco = (g != null) ? g.FocoAtual : null;

        float xAlvo = (foco != null) ? foco.position.x : xMin;
        xAlvo = Mathf.Clamp(xAlvo, xMin, xMax);

        Vector3 p = transform.position;
        p.x = Mathf.Lerp(p.x, xAlvo, 1f - Mathf.Exp(-suavizacao * Time.deltaTime));
        p.y = alturaY;
        p.z = distanciaZ;
        transform.position = p;
    }
}
