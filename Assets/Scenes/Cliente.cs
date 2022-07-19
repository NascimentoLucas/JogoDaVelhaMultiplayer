using UnityEngine;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine.UI;

public class Cliente : MonoBehaviour
{
    private const string SERVIDOR = "127.0.0.1";
    private const int PORTA = 5000;


    private TcpClient cliente;
    private StreamReader reader = null;
    private StreamWriter writer = null;

    private Thread threadCliente;

    private bool rodando = false;
    [Header("Setup")]
    [SerializeField]
    private Peca[] pecas;
    [SerializeField]
    private Text text;
    private string dados;

    public int id = -1;

    private void OnApplicationQuit()
    {
        //Na unity quando se cria uma thread é importante que ao sair
        //do modo play, a gente termine as threads, isso pq a unity não faz sozinha
        Sair();
    }

    private void Update()
    {
        text.text = dados;
    }

    private void MetodoDaThread()
    {
        cliente = new TcpClient(SERVIDOR, PORTA);
        NetworkStream stream = cliente.GetStream();

        //Objeto responsável em ler as mensagens
        reader = new StreamReader(stream);
        //Objeto responsável em enviar mensagens
        writer = new StreamWriter(stream);


        dados = reader.ReadLine();
        //jogador:0
        try
        {
            string[] idDados = dados.Split(":");
            if (idDados.Length == 2)
            {
                //idDados[0] = jogador
                //idDados[1] = 0
                id = int.Parse(idDados[1]);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            id = -1;
        }


        while (rodando)
        {
            try
            {
                ReceberMensagem();
            }
            catch
            {
                rodando = false;
            }
        }
    }

    private void ReceberMensagem()
    {
        try
        {
            //Essa linha para a execução do código enquanto não chega nenhuma mensagem 
            dados = reader.ReadLine();
            while (dados != null)
            {
                for (int i = 0; i < pecas.Length; i++)
                {
                    if (pecas[i].UseDados(dados))
                    {
                        break;
                    }
                }

                dados = reader.ReadLine();
            }
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Sair();
        }
    }

    public void Sair()
    {
        rodando = false;
        if (threadCliente != null)
        {
            threadCliente.Abort();
        }
        cliente = null;
    }


    public void Conectar()
    {
        try
        {
            //Começa uma thread separada da unity
            rodando = true;
            threadCliente = new Thread(MetodoDaThread);
            threadCliente.Start();

        }
        catch (Exception e)
        {
            Debug.Log($"Erro no clinte {e}");
        }
    }

    public void EnviarMensagem(string msg)
    {
        if (writer == null) return;
        try
        {
            writer.WriteLine(msg);
            writer.Flush();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
