using UnityEngine;

/// <summary>
/// PONTO DE MEDIÇÃO "PREVISTO x MEDIDO" (um objeto vazio por medição; são 3).
///
/// Quando o alvo passa do X do ponto, mede UMA vez:
///   - Tempo: lê o cronômetro (soma de Time.fixedDeltaTime desde o impulso)
///   - Velocidade: lê o módulo da velocidade do alvo (só LEITURA de linearVelocity)
/// e escreve no Console e no HUD:
///   erro (%) = |medido - previsto| / previsto * 100
///
/// Ler a velocidade é permitido; a regra proíbe ESCREVER em linearVelocity.
/// </summary>
public class PontoDeMedicao : MonoBehaviour
{
    public enum Grandeza { Tempo, Velocidade }

    [Header("O que medir")]
    public Rigidbody alvo;
    public Grandeza grandeza = Grandeza.Tempo;

    [Header("Valor calculado na proposta")]
    public float previsto = 1f;
    public string unidade = "s";
    public string rotulo = "Mecanismo ?";

    private bool medido;

    void FixedUpdate()
    {
        GerenciadorMaquina g = GerenciadorMaquina.Instancia;
        if (g == null || !g.Rodando || medido || alvo == null) return;

        // Só mede quando o alvo chega (ou passa) da posição X deste objeto.
        if (alvo.position.x < transform.position.x) return;
        medido = true;

        float valorMedido = (grandeza == Grandeza.Tempo) ? g.Tempo : alvo.linearVelocity.magnitude;
        float erro = Mathf.Abs(valorMedido - previsto) / previsto * 100f;

        string linha = "[PREVISTO x MEDIDO] " + rotulo
                     + " | previsto = " + previsto.ToString("F2") + " " + unidade
                     + " | medido = " + valorMedido.ToString("F2") + " " + unidade
                     + " | erro = " + erro.ToString("F1") + " %";

        Debug.Log(linha);
        g.RegistrarMedicao(linha);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.6f, 1f);
        Vector3 p = transform.position;
        Gizmos.DrawLine(p + Vector3.down * 2f, p + Vector3.up * 2f);
    }
}
