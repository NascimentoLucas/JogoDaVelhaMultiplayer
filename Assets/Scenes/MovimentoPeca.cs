using System.Collections;
using UnityEngine;

namespace Assets.Scenes
{
    public class MovimentoPeca : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField]
        private Peca peca;
        public bool selecionada = false;

        void Update()
        {
            if (selecionada)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    var v3 = Input.mousePosition;
                    v3 = Camera.main.ScreenToWorldPoint(v3);
                    selecionada = false;
                    v3.z = transform.position.z;
                    transform.position = v3;
                }
            }
        }


        private void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
                selecionada = true;
        }
    }
}