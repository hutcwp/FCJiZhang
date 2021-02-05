using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject world;

    public Cell[] cells;

    public List<Robot> blueRobots;
    public List<Robot> redRobots;

    public Robot selectRobot;

    public int curTurnType = 0; //当前回合机器人类型


    private void Awake()
    {
        Debug.Log("GameManage Awake");
        if(instance == null)
        {
            instance = this;           
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }       
        }
        DontDestroyOnLoad(gameObject);
        Debug.Log("after GameManage Awake instance="+ instance);
    }

    private void Start()
    {
        foreach(Cell cell in cells)
        {
            cell.transform.SetParent(world.transform);
        }

      
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("鼠标右键按下");
            back();
        }

        CheckGameTurn();
        //CheckGameOver();
    }

    private void CheckGameTurn()
    {

        if (curTurnType == 0)
        {
            if (blueRobots.Find(x => x.hasMoved == false) == null)
            {
                Debug.Log("更换回合->1");
                curTurnType = 1;
                redRobots.ForEach(delegate (Robot robot)
                {
                    robot.hasMoved = false;
                });
            }
        }
        else
        {
            if (redRobots.Find(x => x.hasMoved == false) == null)
            {
                Debug.Log("更换回合->0");
                curTurnType = 0;
                blueRobots.ForEach(delegate (Robot robot)
                {
                    robot.hasMoved = false;
                });
            }
        }
    }

    /**
     * 点击了地图格子
     */
    public void MouseDownCell(Cell cell)
    {
        Debug.Log("点击了地图 selectRobot->" + selectRobot);
        if (selectRobot==null)
        {
            Debug.Log("MouseDownCell selectRobot==null");
            return;
        }
    
        if (selectRobot.status == Robot.STATE.MOVE)
        {
            selectRobot.Move(cell);
        } 
        else 
        {
           
        }
    }

    /**
     * 点击了机器人
     */
    public void RobotMouseClick(Robot robot)
    {
        Debug.Log("RobotMouseClick selectRobot->" + selectRobot);
        if (selectRobot == null)
        {
            if (robot.robotType != curTurnType)
            {
                Debug.Log("还没有到你的回合！！！");
                return;
            }
            if (!robot.hasMoved)
            {
                selectRobot = robot;
                robot.ShowMoveRange();
                Debug.Log("selectRobot->" + selectRobot);
            }
            else
            {
                robot.ShowInfo(); //显示机器信息
            }
        }
        else
        {
            if (selectRobot.status == Robot.STATE.ATACK)
            {
                if (selectRobot.robotType != robot.robotType)
                {
                    selectRobot.Attack(robot);
                }
                else
                {
                    if (selectRobot == robot)
                    {
                        Debug.Log("不攻击，待机！");
                        selectRobot.finishTurn();
                    }
                    else
                    {
                        Debug.Log("不能攻击己方机体！");
                    }
                }
            }
        }
    }

    /**
     * 取消上一步操作
     */
    private void back()
    {
        if (selectRobot.status != Robot.STATE.FINISH)
        {
            selectRobot.Revert();
            selectRobot = null;
        }
        
    }


    public void CheckGameOver()
    {
        Debug.Log("redRobots="+ redRobots.Count+ " blueRobots=" + blueRobots.Count);
        if (redRobots.Count==0)
        {
            Debug.Log("游戏胜利");
        }else if (blueRobots.Count == 0)
        {
            Debug.Log("游戏失败");
        }
    }

}
