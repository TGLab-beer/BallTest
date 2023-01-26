using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public int tryes;
    
    public GameObject ballPref;
    private GameObject ball;
    
    private PoolManager pool;

    public string wallTag;
    public int spawnRadius_wall;
    private int wallScaleK;

    private Vector3Int ballLastPos;

    public Vector3 startVelocity = new Vector3(1, -1, 0);
    private Rigidbody rb_ball;
    public float speed;
    public float timer = 15;
    public float barrierTimer = 3, barrierTimerReload = 1;

    public Vector3 distanceToCamera;

    public GameObject barrier;
    public DifficultLevelEnum difficult;
    public enum DifficultLevelEnum
    {
        easy,
        medium,
        hard
    }
    public List<Difficults> difficultLevel;
    [System.Serializable]
    public struct Difficults
    {
        public float barrierTimer;
        public float speed;
    }

    public float gameTimer;
    public void ChangeDifficultLevel(int dif)
    {
        
        switch (dif)
        {
            case 0:
                difficult = DifficultLevelEnum.easy;
                break;
            case 1:
                difficult = DifficultLevelEnum.medium;
                break;
            case 2:
                difficult = DifficultLevelEnum.hard;
                break;
        }

        speed = difficultLevel[dif].speed;
        barrierTimer = difficultLevel[dif].barrierTimer;
        barrierTimerReload = barrierTimer;
    }
    public void StartGame()
    {
        tryes = PlayerPrefs.GetInt("Tryes");
        pool = PoolManager.instance;
        ball = Instantiate(ballPref, Vector3.zero, Quaternion.identity);
        ball.GetComponent<Ball>().manager = this;
        wallScaleK = Mathf.RoundToInt(pool.pools[0].prefab.transform.localScale.x);
        rb_ball = ball.GetComponent<Rigidbody>();
        for (int i = RoundToValueUP(ball.transform.position, wallScaleK).y - spawnRadius_wall * wallScaleK - wallScaleK; i < RoundToValueUP(ball.transform.position, wallScaleK).y + spawnRadius_wall * wallScaleK + wallScaleK; i+=wallScaleK)
        {
            for (int j = RoundToValueUP(ball.transform.position, wallScaleK).x - 5; j < RoundToValueUP(ball.transform.position, wallScaleK).x + spawnRadius_wall * wallScaleK + wallScaleK; j+=wallScaleK)
            {
                pool.Spawn(wallTag, new Vector3Int(j, i, Mathf.RoundToInt(ball.transform.position.z) + 2), Quaternion.identity);
                pool.Spawn(wallTag, new Vector3Int(j, i, Mathf.RoundToInt(ball.transform.position.z) - 2), Quaternion.identity);
            }
        }
        ballLastPos = Vector3Int.zero;
    }

    void OnPositionChanged()
    {
        List<GameObject> list = new List<GameObject>(pool._pools[wallTag]);
            for (int i = RoundToValueUP(ball.transform.position, wallScaleK).x - wallScaleK * 2;
                 i < RoundToValueUP(ball.transform.position, wallScaleK).x + spawnRadius_wall * wallScaleK;
                 i += wallScaleK)
            {
                if (list.Find(x => Vector2Int.RoundToInt(x.transform.position) == new Vector2Int(i,
                        RoundToValueUP(ball.transform.position, wallScaleK).y + spawnRadius_wall * wallScaleK)) == null)
                {
                    pool.Spawn(wallTag, new Vector3Int(i, RoundToValueUP(ball.transform.position, wallScaleK).y + spawnRadius_wall * wallScaleK, Mathf.RoundToInt(ball.transform.position.z) + 2), Quaternion.identity);
                    pool.Spawn(wallTag, new Vector3Int(i, RoundToValueUP(ball.transform.position, wallScaleK).y + spawnRadius_wall * wallScaleK, Mathf.RoundToInt(ball.transform.position.z) - 2), Quaternion.identity);
                }
            }
            for (int i = RoundToValueUP(ball.transform.position, wallScaleK).x - wallScaleK * 2;
                 i < RoundToValueUP(ball.transform.position, wallScaleK).x + spawnRadius_wall * wallScaleK;
                 i += wallScaleK)
            {
                if (list.Find(x => Vector2Int.RoundToInt(x.transform.position) == new Vector2Int(i,
                        RoundToValueUP(ball.transform.position, wallScaleK).y - spawnRadius_wall * wallScaleK)) == null)
                {
                    pool.Spawn(wallTag, new Vector3Int(i, RoundToValueUP(ball.transform.position, wallScaleK).y - spawnRadius_wall * wallScaleK, Mathf.RoundToInt(ball.transform.position.z) + 2), Quaternion.identity);
                    pool.Spawn(wallTag, new Vector3Int(i, RoundToValueUP(ball.transform.position, wallScaleK).y - spawnRadius_wall * wallScaleK, Mathf.RoundToInt(ball.transform.position.z) - 2), Quaternion.identity);
                }
            }
            for (int i = RoundToValueUP(ball.transform.position, wallScaleK).y - spawnRadius_wall * wallScaleK;
                 i < RoundToValueUP(ball.transform.position, wallScaleK).y + spawnRadius_wall * wallScaleK + wallScaleK;
                 i += wallScaleK)
            {
                if (list.Find(x => Vector2Int.RoundToInt(x.transform.position) ==
                                   new Vector2Int(RoundToValueUP(ball.transform.position, wallScaleK).x + spawnRadius_wall * wallScaleK, i)) == null)
                {
                    pool.Spawn(wallTag, new Vector3Int(RoundToValueUP(ball.transform.position, wallScaleK).x + spawnRadius_wall * wallScaleK, i, Mathf.RoundToInt(ball.transform.position.z) + 2), Quaternion.identity);
                    pool.Spawn(wallTag, new Vector3Int(RoundToValueUP(ball.transform.position, wallScaleK).x + spawnRadius_wall * wallScaleK, i, Mathf.RoundToInt(ball.transform.position.z) - 2), Quaternion.identity);
                }
            }

            GameObject[] objects = GameObject.FindGameObjectsWithTag("Wall");
            foreach (GameObject obj in objects)
            {
                if(obj.transform.position.x < ball.transform.position.x - wallScaleK * 2)
                    pool.DeSpawn(wallTag, obj);
            }
    }

    void SpawnBarrier()
    {
        int posY =
            Mathf.RoundToInt(UnityEngine.Random.Range(ball.transform.position.y - 2 * speed, ball.transform.position.y + 2 * speed));
        pool.Spawn("Barrier", new Vector3Int((int)(Mathf.RoundToInt(ball.transform.position.x) + 30), posY, 0),
            Quaternion.identity);
    }
    private void FixedUpdate()
    {
        if(pool == null) return;
        if (ball == null) return;
        if (StateMachine.instance.state != StateMachine.States.game) return;

        gameTimer += Time.deltaTime;
        
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 15;
            speed += 1;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb_ball.velocity = new Vector3(startVelocity.x, startVelocity.y * speed, 0);
        }
        else
        {
            rb_ball.velocity = new Vector3(startVelocity.x, -startVelocity.y * speed, 0);
        }

        barrierTimer -= Time.deltaTime;
        if (barrierTimer <= 0)
        {
            SpawnBarrier();
            barrierTimer = barrierTimerReload;
        }

        transform.position = ball.transform.position - distanceToCamera;
        transform.LookAt(ball.transform.position);
        
        if (Vector3Int.RoundToInt(ball.transform.position).x - ballLastPos.x >= wallScaleK || Vector3Int.RoundToInt(ball.transform.position).y - ballLastPos.y >= wallScaleK)
        {
            OnPositionChanged();
        }
    }

    public void Lose()
    {
        StateMachine.instance.ChangeState(StateMachine.States.lose);

        tryes += 1;
        PlayerPrefs.SetInt("Tryes", tryes);
        
       
        Camera.main.transform.position =
            new Vector3(0, Camera.main.transform.position.y, Camera.main.transform.position.z);
        
        Destroy(ball);
        PoolManager.instance.ClearAll();
    }

    Vector2Int RoundToValueUP(Vector2 vec, int value)
    {
        int x = Mathf.RoundToInt(vec.x);
        int y = Mathf.RoundToInt(vec.y);

        x = x + (value - x % value);
        y = y + (value - y % value);

        return new Vector2Int(x, y);
    }
    
}
