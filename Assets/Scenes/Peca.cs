using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peca : MonoBehaviour
{
    [Header("Setup")]
    public Cliente cliente;
    [SerializeField]
    SpriteRenderer local;
    [SerializeField]
    SpriteRenderer oponente;

    [Header("Rede")]
    public int pecaId = -1;

    private Vector3 oponentePos;


    private void Update()
    {

        string msg = "p;" + pecaId;
        msg += ";" + local.transform.position.x;
        msg += ";" + local.transform.position.y;
        msg += ";" + local.transform.position.z;
        cliente.EnviarMensagem(msg);
        oponente.transform.position = oponentePos;
    }

    public bool UseDados(string dados)
    {
        if (!dados[0].Equals('p')) return false;

        string[] pos = dados.Split(';');
        //p;-1;0;0;0
        //pos[0] = p
        //pos[1] = -1
        //pos[2] = 0
        //pos[3] = 0
        //pos[4] = 0

        try
        {
            if (!pos[1].Equals(pecaId.ToString()))
                return false;

            oponentePos.x = float.Parse(pos[2]);
            oponentePos.y = float.Parse(pos[3]);
            oponentePos.z = float.Parse(pos[4]);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
            return false;
        }

        return true;
    }
}
