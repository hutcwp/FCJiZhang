using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Robot : MonoBehaviour
{


    public enum STATE { IDEL, MOVE, ATACK, FINISH};


    public float offset = 0.4f;
    public float moveDistance = 2.5f;
    public int moveSpeed = 2;

    public float attackDistance = 2f;

    public List<Cell> moveableList = new List<Cell>(); //可移动范围
    public List<Cell> attackableList = new List<Cell>(); //可攻击范围


    public STATE status = STATE.IDEL; //0:默认可以被选择状态,1:选中可移动状态，2:移动完可攻击状态 3：待机状态


    public int robotType = -1; //机器人类型：0 己方，1 敌方


    public int  health = 500;
    public int maxHealth = 500;
    public int attack = 70;

    public bool hasMoved = false; //是否已经移动过，本回合
    public Vector3 originPos;

    public SpriteRenderer spriteRenderer;
    public Color waitColor;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        //healthBar.SetUp(this);

     
        Debug.Log("Start spriteRenderer="+ spriteRenderer);
        ShowMoveAviable(false);
        originPos = transform.position;
    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        GameManager.instance.RobotMouseClick(this);
    }

    public float healthRate()
    {
        return health*1f / maxHealth;
    }

    public void ShowInfo()
    {
        Debug.Log("helath=" + health);
    }

    public void ChangeStatus(STATE status)
    {
        Debug.Log("status:"+this.status+"->"+status);
        this.status = status;
    }

    public void ShowMoveRange()
    {
        ChangeStatus(STATE.MOVE);
        moveableList.Clear();
 
        //Debug.Log("cur pos x=" + transform.position.x + " y=" + transform.position.y);
        foreach (Cell cell in GameManager.instance.cells)
        {
            float disX = Mathf.Abs(transform.position.x - cell.transform.position.x);
            float disY = Mathf.Abs(transform.position.y - cell.transform.position.y);

            if (disX + disY <= (moveDistance+ offset) && cell.canWalk)
            {
                cell.ShowMoveColor();
                moveableList.Add(cell);
                //Debug.Log("cell pos= x=" + cell.transform.position.x + " y=" + cell.transform.position.y);
            }

            if (disX == 1 || disY == 1)
            {
                //Debug.Log("disx = "+disX +",disY="+disY+",moveDistance="+ moveDistance);
            }
            if (transform.position.x == cell.transform.position.x && transform.position.y == cell.transform.position.y)
            {
                //Debug.Log("find out !!!!");
            }
        }
    }

    public void HideMoveRange()
    {
        foreach (Cell cell in moveableList)
        {
            cell.resetHighColor();
        }
    }

    public void ShowAttackRange()
    {
        ChangeStatus(STATE.ATACK);
        attackableList.Clear();
        foreach (Cell cell in GameManager.instance.cells)
        {
            float disX = Mathf.Abs(transform.position.x - cell.transform.position.x);
            float disY = Mathf.Abs(transform.position.y - cell.transform.position.y);

            if (disX + disY <= (attackDistance+offset))
            {
                cell.ShowAttackColor();
                attackableList.Add(cell);
            }
        }
    }

    public void HideAttackRange()
    {
        foreach (Cell cell in attackableList)
        {
            cell.resetHighColor();
        }
    }

    private bool CanMove(Cell cell)
    {
        if (moveableList == null)
        {
            return false;
        }

        return moveableList.Contains(cell);
    }

    public void Move(Cell cell)
    {
        if (!CanMove(cell))
        {
            Debug.Log("Can not Move ");
        }
        else
        {
            //StartCoroutine(MoveCo(_trans));
            MoveNormal(cell.transform.position);
        }

        HideMoveRange();
        ShowAttackRange();
    }

    public void Attack(Robot robot)
    {
        Debug.Log("attack");
        HideAttackRange();
        robot.Damege(attack);
        
        finishTurn();
    }

    public void Damege(int hart)
    {
        Debug.Log("Damege:hart=" + hart+ " health:"+ health);
        if (health - hart <= 0)
        {
            Destroy(gameObject);
            GameManager.instance.blueRobots.Remove(this);
            GameManager.instance.redRobots.Remove(this);
            GameManager.instance.CheckGameOver();

        }
        else
        {
            health -= hart;
        }
    }

    public void finishTurn()
    {
        ChangeStatus(STATE.FINISH);
        hasMoved = true;
        originPos = transform.position;
        ShowMoveAviable(false);
        HideMoveRange();
        HideAttackRange();
        GameManager.instance.selectRobot = null;
        ResetCellStatus();
    }

    private void ShowMoveAviable(bool moveable)
    {
        Debug.Log("ShowMoveAviable spriteRenderer=" + spriteRenderer);
        if (moveable)
        {
            spriteRenderer.color = waitColor;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    } 

    private void MoveNormal(Vector3 vector3)
    {
        // transform.position = _trans.position;
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(vector3.x, vector3.y), moveSpeed);       
    }

    IEnumerator MoveCo(Transform _trans)
    {
        while (transform.position.x != _trans.position.x)
        {
            Debug.Log("1=" + transform.position.x + " 2=" + _trans.position.x);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_trans.position.x, transform.position.y), moveSpeed);
            yield return new WaitForSeconds(0);
        }

        Debug.Log("22222" + transform.position.x + " 2=" + _trans.position.x);
        while (transform.position.y != _trans.position.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(_trans.position.x, transform.position.y), moveSpeed);
            yield return 0;
        }
    }

    public void ResetCellStatus()
    {
        moveableList.Clear();
        attackableList.Clear();
    }


    /**
     * 取消当前操作。移动了取消移动
     */
    public void Revert()
    {
        hasMoved = false;
        HideMoveRange();
        HideAttackRange();
        MoveNormal(originPos);
        ResetCellStatus();
    }
}
