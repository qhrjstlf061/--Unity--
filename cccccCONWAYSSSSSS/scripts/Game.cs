using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class Game : MonoBehaviour
{
    private static int SCREEN_WIDTH = 64;
    private static int SCREEN_HEIGHT = 48;

    public float speed = 0.1f;
    private float timer = 0;

    public bool simulationEnabled = false;

    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

    // Start is called before the first frame update
    void Start()
    {
        PlaceCells();
    }

    // Update is called once per frame
    void Update()
    {
        if(simulationEnabled)
        {
        
            if (timer >= speed)
            {
                timer = 0f;
                CountNeighbors();
                populationControl();
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        UserInput();

    }

    private void savePattern()
    {
        string path = "patterns";

        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        Pattern pattern = new Pattern();

        string patternString = null;

        for (int y = 0; y< SCREEN_HEIGHT; y++)
        {
            for(int x = 0; x < SCREEN_WIDTH; x++)
            {
                if(grid[x,y].isAlive == false)
                {
                    patternString += "0";
                }
                else
                {
                    patternString += "1";
                }
            }
        }
        pattern.patternString = patternString;
        XmlSerializer serializer = new XmlSerializer(typeof(Pattern));
        StreamWriter writer = new StreamWriter(path + "/test.xml");
        writer.Close();
    }
    

    void UserInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePoint.x);
            int y = Mathf.RoundToInt(mousePoint.y);

            if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT)
            {
                grid[x, y].SetAlive(!grid[x,y].isAlive);
            }
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            simulationEnabled = false;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            simulationEnabled = true;
        }


    }
    void PlaceCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y ++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x ++)
            {
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x,y),Quaternion.identity) as Cell;
                grid[x,y] = cell;
                grid[x,y].SetAlive(RandomAliveCell());
            }
        }
    }

    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y ++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x ++)
            {
                int numNeighbors = 0;
                // - North
                if (y+1 < SCREEN_HEIGHT)
                {
                    if (grid[x,y+1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - East
                if (x+1 < SCREEN_WIDTH)
                {
                    if (grid[x+1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - South
                if (y-1 >= 0)
                {
                    if (grid[x, y-1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - West
                if (x-1 >= 0)
                {
                    if (grid[x-1, y].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - Northeast
                if(y+1 < SCREEN_HEIGHT && x+1 < SCREEN_WIDTH)
                {
                    if (grid[x+1, y+1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - Southeast
                if(y-1 >= 0 && x+1 < SCREEN_WIDTH)
                {
                    if (grid[x+1, y-1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - Southwest
                if(y-1 >= 0 && x-1 >= 0)
                {
                    if (grid[x-1, y-1].isAlive)
                    {
                        numNeighbors++;
                    }
                }
                // - Northwest
                if(y+1 < SCREEN_HEIGHT && x-1 >= 0)
                {
                    if (grid[x-1, y+1].isAlive)
                    {
                        numNeighbors++;
                    }
                }

                grid[x,y].numNeighbors = numNeighbors;
            }
        }
    }

    void populationControl()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y ++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x ++)
            {
                // - Any live cell with 2 or 3 neightbors stay alive
                // - Any dead cells with exactly 3 neighbors comes back to life
                // - Else die

                if (grid[x,y].isAlive)
                {
                    if(grid[x,y].numNeighbors != 2 && grid[x,y].numNeighbors != 3)
                    {
                        grid[x,y].SetAlive(false);
                    }

                }

                else
                {
                    if (grid[x,y].numNeighbors ==3)
                    {
                        grid[x,y].SetAlive(true);
                    }
                }
            }
        }
    }

    bool RandomAliveCell ()
    {
        int rand = UnityEngine.Random.Range(0,100);
        if (rand > 75)
        {
            return true;
        }
        return false;
    }
}