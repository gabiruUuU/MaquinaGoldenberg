using UnityEngine;

/// <summary>
/// MONITOR DE VELOCIDADE (OPCIONAL, só para você calibrar a cena).
///
/// Coloque em Bloco_A, Esfera_B, Bloco_C e Esfera_D. Durante o Play, o Inspector
/// mostra a velocidade e a energia cinética de cada um, para você comparar com
/// os valores previstos da tabela. Só LÊ; pode apagar antes de entregar.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class MonitorDeVelocidade : MonoBehaviour
{
    public float velocidade_m_s;      // |v|
    public float energiaCinetica_J;   // K = 1/2 · m · v²

    private Rigidbody rb;

    void Awake() { rb = GetComponent<Rigidbody>(); }

    void FixedUpdate()
    {
        velocidade_m_s = rb.linearVelocity.magnitude;
        energiaCinetica_J = 0.5f * rb.mass * velocidade_m_s * velocidade_m_s;
    }
}
