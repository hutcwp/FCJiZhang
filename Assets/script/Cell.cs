using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public bool canWalk=true;

    public bool canAttack = true;

    private SpriteRenderer spriteRenderer;

    public Color highLightColor  ;
    public Color attackLightColor = Color.red;

    public Robot robot; //位于该地形上面的robot

    public LayerMask robotLayerMask;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start spriteRenderer=" + spriteRenderer);
        spriteRenderer = GetComponent<SpriteRenderer>();
        //HighLightColor();

        getRobot();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getRobot()
    {
        Collider2D collider = Physics2D.OverlapCircle(transform.position, spriteRenderer.bounds.extents.x/3, robotLayerMask);
        if (collider != null)
        {
            robot = collider.GetComponent<Robot>();
        }
    }

    public void ShowMoveColor()
    {
        if (canWalk)
        {
            spriteRenderer.color = highLightColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void ShowAttackColor()
    {
        if (canAttack)
        {
            spriteRenderer.color = attackLightColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public void resetHighColor()
    {
        spriteRenderer.color = Color.white;
    }

    private void OnMouseDown()
    {
        GameManager.instance.MouseDownCell(this);

    }
}
