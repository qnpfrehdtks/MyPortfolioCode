using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileGenerator : Singleton<TileGenerator>
{
    class Region
    {
        public int Area
        {
            get
            {
                return listCoord.Count;
            }
        }

        public List<Coord> listCoord = new List<Coord>();

        public void InsertCoord(int X, int Y)
        {
            listCoord.Add(new Coord(X, Y));
        }
        public void InsertCoord(Coord coord)
        {
            if (coord == null) return;

            listCoord.Add(coord);
        }
    }
    class Coord
    {
        public int X;
        public int Y;

        public Coord(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }

    E_TILE_TYPE[,] map;

    [SerializeField]
    int m_DeathLimit = 4;

    [SerializeField]
    int m_BirthLimit = 4;

    [SerializeField]
    int m_SimulationCnt = 10;

    [SerializeField]
    int m_ChanceWall = 50;


    private void Start()
    {
        InitializeManager();
    }

    DesturctionTile m_DesturctionTile;
    DesturctionTile m_DesturctionTile2;
  //  DesturctionTile m_waterTile;
    public override void InitializeManager()
    {
        m_DesturctionTile = ResourceManager.Instance.Load<DesturctionTile>("Prefabs/Tile/groundCubeGrass");
        m_DesturctionTile2 = ResourceManager.Instance.Load<DesturctionTile>("Prefabs/Tile/groundCube");
    }

    public void CreateMap()
    {
        GeneratePerlinMap(25, 25);

        for (int i = 0; i < m_SimulationCnt; i++)
        {
            doSimulationStep();
        }

        SetupTiles();

        NavMeshSurface nav = Common.GetOrAddComponent<NavMeshSurface>(Tiles[0]);
        nav.BuildNavMesh();
    }


    List<Region> GetRegions(E_TILE_TYPE TileType)
    {
        List<Region> list = new List<Region>();
        int[,] mapFlag = new int[map.GetUpperBound(0), map.GetUpperBound(1)];

        for (int x = 1; x < map.GetUpperBound(0) - 1; x++)
        {
            for (int y = 1; y < map.GetUpperBound(1) - 1; y++)
            {
                if (mapFlag[x, y] == 1) continue;

                if (map[x, y] == TileType)
                {
                    Region region = CreateRegionArea(x, y, TileType);
                    list.Add(region);
                    mapFlag[x, y] = 1;

                    foreach (var coord in region.listCoord)
                    {
                        mapFlag[coord.X, coord.Y] = 1;
                    }
                }
            }
        }

        return list;
    }


    Queue<Coord> m_BFSQ = new Queue<Coord>();

    Region CreateRegionArea(int StartX, int StartY, E_TILE_TYPE TileType)
    {
        int[,] mapFlag = new int[map.GetUpperBound(0), map.GetUpperBound(1)];
        m_BFSQ.Enqueue(new Coord(StartX, StartY));
        mapFlag[StartX, StartY] = 1;

        Region result = new Region();

        while (m_BFSQ.Count > 0)
        {
            Coord currentCoord = m_BFSQ.Dequeue();
            result.InsertCoord(currentCoord);

            for (int x = -1; x <= 1; x++)
            {
                for(int y = -1; y <= 1; y++)
                {
                    int nx = currentCoord.X + x;
                    int ny = currentCoord.Y + y;

                    if (!IsInMapRange(nx,ny)) continue;
                    if (nx == StartX && ny == StartY) continue;
                    if (map[nx, ny] != TileType) continue;
                    if (mapFlag[nx, ny] == 1) continue;
                    
                    mapFlag[nx, ny] = 1;
                    m_BFSQ.Enqueue(new Coord(nx, ny));
                }
            }
        }

        return result;
    }

    E_TILE_TYPE GetRandomTileType()
    {
       int randomType =  Random.Range((int)E_TILE_TYPE.groundCube, (int)E_TILE_TYPE.waterCube);
        return (E_TILE_TYPE)randomType;
    }


    void doSimulationStep()
    {
        E_TILE_TYPE[,] newMap = new E_TILE_TYPE[map.GetUpperBound(0), map.GetUpperBound(1)];

        for(int x = 1; x < map.GetUpperBound(0) - 1; x++)
        {
            for (int y = 1; y < map.GetUpperBound(1) - 1; y++)
            {

               int wallCnt = 8 - GetSurroundingWall(x, y);

                if (map[x, y] == E_TILE_TYPE.waterCube)
                {
                    if (wallCnt < m_DeathLimit)
                    {
                        newMap[x, y] = GetRandomTileType();

                    }
                    else
                    {
                        newMap[x, y] = E_TILE_TYPE.waterCube;// GetRandomTileType();
                    }
                }
                else
                {
                    if (wallCnt > m_BirthLimit)
                    {
                        newMap[x, y] = E_TILE_TYPE.waterCube;
                    }
                    else
                    {
                        newMap[x, y] = GetRandomTileType();
                    }
                }


            }
        }

        map = newMap;

    }

    bool IsInMapRange(int StartX, int StartY)
    {
        return (StartX >= 0 && StartY >= 0 && StartX < map.GetUpperBound(0) && StartY < map.GetUpperBound(1));
    }

    public void GeneratePerlinMap(int width, int height)
    {
        map = new E_TILE_TYPE[width, height];

        string seed = Time.time.ToString();
        System.Random pRand = new System.Random(seed.GetHashCode());

        for(int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if(x == 0 || y ==0 || x >= map.GetUpperBound(0) - 1 || y >= map.GetUpperBound(1) - 1)
                {
                    map[x, y] = E_TILE_TYPE.waterCube;
                    continue;
                }
                map[x, y] = pRand.Next(0,100) < m_ChanceWall ? E_TILE_TYPE.waterCube : GetRandomTileType();
            }
        }
    }

    int GetSurroundingWall(int X, int Y)
    {
        int wallCount = 0;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {

                int nx = X + x;
                int ny = Y + y;

                if (nx == X && ny == Y)
                {
                    continue;
                }

                if (!IsInMapRange(nx, ny))
                     continue;

                if( map[nx, ny] != E_TILE_TYPE.waterCube)
                {
                    wallCount++;
                }
            }
        }

        return wallCount;
    }

    E_TILEDIR SelectTileDir(bool[,] squre3X3)
    {
        // 왼쪽, 오른쪽, 아래, 위
        if(squre3X3[0,1] == true && squre3X3[2, 1] == false && squre3X3[1, 0] == true && squre3X3[1, 2] == true)
        {
            return E_TILEDIR.TO_RIGHT;
        }
        else if (squre3X3[0, 1] == false && squre3X3[2, 1] == true && squre3X3[1, 0] == true && squre3X3[1, 2] == true)
        {
            return E_TILEDIR.TO_LEFT;
        }
        else if (squre3X3[0, 1] == false && squre3X3[2, 1] == true && squre3X3[1, 0] == true && squre3X3[1, 2] == false)
        {
            return E_TILEDIR.TOP_LEFT;
        }
        else if (squre3X3[0, 1] == true && squre3X3[2, 1] == false && squre3X3[1, 0] == true && squre3X3[1, 2] == false)
        {
            return E_TILEDIR.TOP_RIGHT;
        }
        else if (squre3X3[0, 1] == true && squre3X3[2, 1] == false && squre3X3[1, 0] == false && squre3X3[1, 2] == true)
        {
            return E_TILEDIR.BOTTOM_RIGHT;
        }
        else if (squre3X3[0, 1] == false && squre3X3[2, 1] == true && squre3X3[1, 0] == false && squre3X3[1, 2] == true)
        {
            return E_TILEDIR.BOTTOM_LEFT;
        }
        else if (squre3X3[0, 1] == true && squre3X3[2, 1] == true && squre3X3[1, 0] == false && squre3X3[1, 2] == true)
        {
            return E_TILEDIR.TO_BOTTOM;
        }
        else if (squre3X3[0, 1] == true && squre3X3[2, 1] == true && squre3X3[1, 0] == true && squre3X3[1, 2] == false)
        {
            return E_TILEDIR.TO_TOP;
        }
        else if (squre3X3[0, 1] == true && squre3X3[2, 1] == true && squre3X3[1, 0] == true && squre3X3[1, 2] == true)
        {
            return E_TILEDIR.NONE;
        }

        return E_TILEDIR.END;
    }


    void CreateTile(int X, int Y)
    {
        bool[,] squre3X3 = new bool[3, 3];

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                int nx = X + x;
                int ny = Y + y;

                if (nx == X && ny == Y)
                {
                    continue;
                }

                if (!IsInMapRange(nx, ny))
                    continue;

                if(map[nx,ny] == E_TILE_TYPE.waterCube)
                {
                    squre3X3[x + 1, y + 1] = true;
                }

            }
        }

        E_TILEDIR tileDir = SelectTileDir(squre3X3);

        if (tileDir == E_TILEDIR.END)
        {
            return;
        }

        Vector3Int position = new Vector3Int(X, 0, Y);
        PoolingManager.Instance.PopFromPool(m_DesturctionTile.gameObject, position, Quaternion.identity);
       // PoolingManager.Instance.PopFromPool("ground_cube_grass", position, Quaternion.identity);
    }

    List<GameObject> Tiles = new List<GameObject>();

    private void SetupTiles()
    {
        Tiles.Clear();
        for (int x = 0; x < map.GetUpperBound(0) ; x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {

                Vector3Int position = new Vector3Int(x, 0, y);

                int ranQuat = Random.Range(0, 4);

                switch(map[x,y])
                {
                    case E_TILE_TYPE.groundCube:
                        Tiles.Add( PoolingManager.Instance.PopFromPool( m_DesturctionTile2.gameObject, position, Quaternion.Euler(0, ranQuat * 90, 0)));
                        break;
                    case E_TILE_TYPE.groundCubeGrass:
                        Tiles.Add(PoolingManager.Instance.PopFromPool(m_DesturctionTile.gameObject, position, Quaternion.Euler(0, ranQuat * 90, 0)));
                        break;
                    case E_TILE_TYPE.waterCube:
                       // PoolingManager.Instance.PopFromPool(m_waterTile.gameObject, position, Quaternion.Euler(0, ranQuat * 90, 0));
                        break;
                }

            }
        }
    }
}
