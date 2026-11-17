using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// GERENCIADOR DA MÁQUINA (um único objeto vazio na cena: "Gerenciador").
///
/// O que ele faz (e o que NÃO faz):
///  - FAZ: contar o tempo (cronômetro do HUD), mostrar o nome do mecanismo ativo,
///         guardar quem a câmera deve seguir e escrever as linhas "previsto x medido".
///  - NÃO FAZ: mexer em posição, velocidade ou força de nenhum objeto.
///    Quem move a máquina é a física da Unity (regra do projeto).
///
/// Cronômetro: soma Time.fixedDeltaTime dentro do FixedUpdate a partir do impulso,
/// exatamente como o documento do projeto pede (assim o tempo fica "preso" à física).
/// </summary>
public class GerenciadorMaquina : MonoBehaviour
{
    // Atalho para os outros scripts acharem este gerenciador: GerenciadorMaquina.Instancia
    public static GerenciadorMaquina Instancia { get; private set; }

    [Header("HUD (uGUI) - arraste os textos do Canvas")]
    public TMP_Text textoCronometro;   // "00,00 s"
    public TMP_Text textoMecanismo;    // nome do mecanismo ativo
    public TMP_Text textoMedicoes;     // linhas previsto x medido (opcional)

    [Header("Câmera")]
    public Transform focoInicial;      // objeto que a câmera segue antes do empurrão (Bloco_A)

    // Estado da cadeia (só leitura para os outros scripts)
    public bool Rodando { get; private set; }
    public bool Finalizada { get; private set; }
    public float Tempo { get; private set; }            // segundos desde o impulso
    public Transform FocoAtual { get; private set; }    // quem a câmera segue agora

    private readonly System.Text.StringBuilder linhasMedicao = new System.Text.StringBuilder();

    void Awake()
    {
        Instancia = this;
        FocoAtual = focoInicial;
    }

    void Start()
    {
        if (textoMecanismo != null) textoMecanismo.text = "Aguardando o empurrão...";
        if (textoMedicoes != null) textoMedicoes.text = "";
    }

    // Soma o tempo de cada passo de física. Só conta enquanto a cadeia está rodando.
    void FixedUpdate()
    {
        if (Rodando) Tempo += Time.fixedDeltaTime;
    }

    // Atualiza o texto do cronômetro a cada frame (só desenha, não mexe na física).
    void Update()
    {
        if (textoCronometro != null)
            textoCronometro.text = Tempo.ToString("00.00") + " s";
    }

    /// Chamado pelo ImpulsoInicial no instante do empurrão: zera e liga o cronômetro.
    public void IniciarCadeia()
    {
        Tempo = 0f;
        Rodando = true;
        Finalizada = false;
        AtivarMecanismo(1, "Impulso inicial", focoInicial);
        Debug.Log("[MÁQUINA] Impulso aplicado. Cronômetro iniciado.");
    }

    /// Chamado pelos MarcadorDeEtapa quando um mecanismo "entra em cena".
    public void AtivarMecanismo(int numero, string nome, Transform foco)
    {
        if (textoMecanismo != null) textoMecanismo.text = "Mecanismo " + numero + " — " + nome;
        if (foco != null) FocoAtual = foco;
        Debug.Log("[MÁQUINA] t = " + Tempo.ToString("F2") + " s | Mecanismo " + numero + " — " + nome);
    }

    /// Chamado pelo marcador final (bandeira no alto): para o cronômetro.
    public void FinalizarCadeia(string nome)
    {
        Rodando = false;
        Finalizada = true;
        if (textoMecanismo != null) textoMecanismo.text = nome;
        Debug.Log("[MÁQUINA] FIM da cadeia. Tempo total = " + Tempo.ToString("F2") + " s");
    }

    /// Chamado pelos PontoDeMedicao: mostra a linha no HUD (o Debug.Log já foi feito por eles).
    public void RegistrarMedicao(string linha)
    {
        linhasMedicao.AppendLine(linha);
        if (textoMedicoes != null) textoMedicoes.text = linhasMedicao.ToString();
    }

    /// Ligado ao botão REINICIAR: recarrega a cena (usado nas 3 execuções seguidas).
    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
