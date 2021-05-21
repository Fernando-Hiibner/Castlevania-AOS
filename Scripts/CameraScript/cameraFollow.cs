using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target; //transform do que a camera vai seguir

    public float smoothing; //vai suavizar o movimento da camera

    Vector3 offset; //Distancia da camera e o alvo

    public float lowY; //Menor altura y que a camera vai, pra prevenir que se o personagem cair da camera ele não siga ele
    public float maxY; //Maior altura y que a camera vai, pra prevenir que se o personagem cair da camera ele não siga ele
    public float lowX; //Menor altura x que a camera vai, pra prevenir que se o personagem cair da camera ele não siga ele
    public float maxX; //Maior altura x que a camera vai, pra prevenir que se o personagem cair da camera ele não siga ele


    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position; //O offset vai ser a diferença inicial da camera e do alvo, ele sempre vai manter essa distancia de diferenca
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position+offset; //Posição do personagem + o offset que vai deixar ela na distancia que fica originalmente

        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing*Time.deltaTime); //Lerp pq vai dar uma suavizada (Lerp basicamente suaviza a transição entree dois valores)

        if(transform.position.y < lowY){
            transform.position = new Vector3(transform.position.x, lowY, transform.position.z);
        }else if(transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
        if(transform.position.x < lowX)
        {
            transform.position = new Vector3(lowX, transform.position.y, transform.position.z);
        }else if(transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
    }
}
