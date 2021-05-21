using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class NormalizeSlope : MonoBehaviour
{
    playerController myPC;
    public LayerMask GroundLayer;
    public LayerMask WallLayer;
    public float maximumClimbAngle = 80;
    public bool grounded;
    public float hitSize = 0.35f;
    public float angleSize = 0.1f;
    Rigidbody2D myRB;

    //Deve ser chamado do fixed update
    public void MoveAndNormalizeSlope(float directionX, float speed)
    {
        //Pega o Rigidbody 2D
        myRB = GetComponent<Rigidbody2D>();
        myPC = GetComponent<playerController>();
        Vector2 forceDirection = Vector2.zero;
        //Verifica se há chão abaixo
        //Sai do transform do playerm reto pra baixo, com um tamanho procurando pelo chão
        RaycastHit2D hitDirection = Physics2D.Raycast(transform.position, Vector2.down, hitSize, GroundLayer);
        //Desenha isso no PlayMode com GizmosAtivados
        Debug.DrawRay(transform.position, Vector2.down * hitSize, Color.yellow);

        //Se achou chão
        if(hitDirection.collider != null)
        {
            grounded = true;
            //Desenha um raio na direção do chão detectado
            Debug.DrawRay(transform.position, hitDirection.normal, Color.white);

            //Se não apontar pra cima estamos inclinados
            //e devemos ajustar a direção em que nos movemos

            //Obtem o valor perpendicular a essa linha pra cima
            //Ou seja, um valor com 90º de inclinação em relação a linha pra cima
            //apontando pra cima em relação a rampa
            //forceDirection = new Vector2(hitDirection.normal.y, -hitDirection.normal.x);

            if(directionX > 0) //direita
            {
                forceDirection = new Vector2(hitDirection.normal.y, -hitDirection.normal.x);
            }
            else //Esquerda
            {
                forceDirection = new Vector2(-hitDirection.normal.y, hitDirection.normal.x);
            }
        }
        else
        {
            grounded = false;
            if (myPC.isGrounded)
            {
                myRB.velocity = new Vector2(directionX * speed, myRB.velocity.y);
            }   
        }

        //Codigo pra impedir que o player suba certos angulos
        float angle = 0; //Vai guardar o valor do angulo

        //Cria um raio que detecta o angulo do obstaculo a frente
        RaycastHit2D hitAngle = Physics2D.Raycast(transform.position, forceDirection, angleSize, WallLayer);
        //Debuga
        Debug.DrawRay(transform.position, hitAngle.normal, Color.blue);
        //Cria um angulo entre um veto pra cima e a posição do obstaculo
        angle = Vector2.Angle(hitAngle.normal, Vector2.up);

        //Verifica o angulo
        if(angle < maximumClimbAngle && grounded)
        {
            //Cria o raio na direção a seguir
            Debug.DrawRay(transform.position, forceDirection * Mathf.Abs(directionX), Color.green);

            //Move o player
            myRB.velocity = forceDirection * Mathf.Abs(directionX) * speed;
        }
        else
        {
            //Cria um raio indicando que não da pra subir
            Debug.DrawRay(transform.position, forceDirection * Mathf.Abs(directionX), Color.red);
        }
    }
}
