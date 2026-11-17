using UnityEngine;

/// <summary>
/// IMPULSO INICIAL (colocar no Bloco_A).
///
/// É o ÚNICO script da máquina que mexe na física, e só uma vez:
///   AddForce(..., ForceMode.Impulse)
/// Impulso J = m · Δv  ->  Δv = J / m = 0,5 / 0,5 = 1,0 m/s rampa abaixo.
///
/// Por que no FixedUpdate? Porque a física da Unity roda no FixedUpdate;
/// aplicar a força ali garante que o empurrão entra no passo de física certo.
///
/// Proibido (regra do projeto): mexer em transform.position ou linearVelocity.
/// Aqui só usamos AddForce.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class ImpulsoInicial : MonoBehaviour
{
    [Header("Valores da proposta")]
    public float impulsoJ = 0.5f;          // N·s
    public float anguloRampaGraus = 15f;   // a rampa 1 desce 15° (mesmo ângulo da rampa)

    private Rigidbody rb;
    private bool pedido;     // alguém apertou EMPURRAR
    private bool aplicado;   // o empurrão já foi dado (só pode acontecer uma vez)

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Ligado ao botão EMPURRAR do HUD (OnClick). Só "pede" o empurrão.
    public void Empurrar()
    {
        if (!aplicado) pedido = true;
    }

    void Update()
    {
        // Atalho opcional: tecla Espaço (só funciona se Active Input Handling = Both).
#if ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKeyDown(KeyCode.Space)) Empurrar();
#endif
    }

    void FixedUpdate()
    {
        if (!pedido || aplicado) return;
        aplicado = true;

        // Direção da rampa para baixo (eixo X para a direita, Y para cima):
        float a = anguloRampaGraus * Mathf.Deg2Rad;
        Vector3 direcao = new Vector3(Mathf.Cos(a), -Mathf.Sin(a), 0f);

        // O único empurrão da máquina inteira.
        rb.AddForce(direcao * impulsoJ, ForceMode.Impulse);

        // Liga o cronômetro do HUD.
        if (GerenciadorMaquina.Instancia != null)
            GerenciadorMaquina.Instancia.IniciarCadeia();
    }
}
