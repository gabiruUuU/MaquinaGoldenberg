using UnityEngine;

/// <summary>
/// MARCADOR DE ETAPA (um objeto vazio por mecanismo, na pasta "Marcadores").
///
/// Serve só para o HUD e a câmera: quando o "alvo" passa da posição do marcador,
/// o HUD mostra o nome do mecanismo e a câmera passa a seguir o "focoCamera".
/// Ele apenas LÊ a posição do alvo. Não move nem empurra nada.
///
/// Como usar: mova o marcador na cena até o X (ou Y) onde o mecanismo começa.
/// Para marcadores no eixo X: vale a posição X do marcador.
/// Para o eixo Y (gangorra): vale a posição Y do marcador.
/// </summary>
public class MarcadorDeEtapa : MonoBehaviour
{
    public enum Eixo { X, Y }

    [Header("Quem precisa passar pelo marcador")]
    public Rigidbody alvo;
    public Eixo eixo = Eixo.X;

    [Header("O que mostrar no HUD")]
    public int numero;
    public string nome;

    [Header("Câmera")]
    public Transform focoCamera;       // quem a câmera segue enquanto este mecanismo está ativo

    [Header("Evento final")]
    public bool eFimDaCadeia;          // marque só no marcador "FIM"

    private bool jaDisparou;

    void FixedUpdate()
    {
        GerenciadorMaquina g = GerenciadorMaquina.Instancia;
        if (g == null || !g.Rodando || jaDisparou || alvo == null) return;

        float posicaoAlvo = (eixo == Eixo.X) ? alvo.position.x : alvo.position.y;
        float limite      = (eixo == Eixo.X) ? transform.position.x : transform.position.y;

        if (posicaoAlvo >= limite)
        {
            jaDisparou = true;
            if (eFimDaCadeia) g.FinalizarCadeia(nome);
            else              g.AtivarMecanismo(numero, nome, focoCamera);
        }
    }

    // Desenha uma linha no Scene para você enxergar onde o marcador está.
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.45f, 0.1f);
        Vector3 p = transform.position;
        if (eixo == Eixo.X) Gizmos.DrawLine(p + Vector3.down * 3f, p + Vector3.up * 3f);
        else                Gizmos.DrawLine(p + Vector3.left * 2f, p + Vector3.right * 2f);
    }
}
