using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Tilemap tilemap_otherGame;
    public Piece activePiece { get; private set; }

    public TetrominoData[] tetrominoes;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public Vector3Int spawnPosition = new Vector3Int(-1, 8, 0);

    public SpriteRenderer grid;
    public SpriteRenderer border;

    public TileBase grey_Tile;

    public TextMeshProUGUI text;
    [SerializeField]
    internal Piece pieceScript;

    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-boardSize.x / 2, -boardSize.y / 2);
            return new RectInt(position, boardSize);
        }
    }

    private void Awake()
    {

        //Camera.main.orthographicSize = 14;
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        grid.size = boardSize;
        //spawnPosition.y = (int)((boardSize.y - 1) / 2);
        border.transform.localScale = new Vector3((float)boardSize.x / 10, (float)boardSize.y / 20, 1);

        for (int i = 0; i < tetrominoes.Length; i++) {
            tetrominoes[i].Initialize();
        }

        text = FindObjectOfType<TextMeshProUGUI>();

    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        activePiece.Initialize(this, spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition)) {
            Set(activePiece);
        } else {
            GameOver();
        }
    }

    public void GameOver()
    {
        tilemap.ClearAllTiles();
        tilemap_otherGame.ClearAllTiles();
        // Do anything else you want on game over here..
        if(pieceScript.isGame2)
        {
            WhoWon.Instance.whoWon = 1;
        } else
        {
            WhoWon.Instance.whoWon = 0;
        }
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bounds;

        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (tilemap.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // Clear from bottom to top
        while (row < bounds.yMax)
        {
            // Only advance to the next row if the current is not cleared
            // because the tiles above will fall down when a row is cleared
            if (IsLineFull(row)) {
                LineClear(row);
                //rowsSetFromEnemy++;
            } else {
                row++;
            }
        }
    }

    public bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            // The line is not full if a tile is missing
            if (!tilemap.HasTile(position) || tilemap.GetTile(position).Equals(grey_Tile)) {
                return false;
            }
        }

        return true;
    }

    public void LineClear(int row)
    {
        RectInt bounds = Bounds;

        SetEnemyLine();


        // Clear all tiles in the row
        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            tilemap.SetTile(position, null);
        }

        // Shift every row above down one
        while (row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row + 1, 0);
                TileBase above = tilemap.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap.SetTile(position, above);
            }

            row++;
        }

    }

    public void SetEnemyLine()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;
        // Shift every row below up one
        while (HasLineTile(row, tilemap_otherGame))
        {
            row++;
        }

        while (row > bounds.yMin)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row - 1, 0);

                TileBase below = tilemap_otherGame.GetTile(position);

                position = new Vector3Int(col, row, 0);
                tilemap_otherGame.SetTile(position, below);

                if (row == bounds.yMin + 1)
                {
                    position = new Vector3Int(col, row - 1, 0);
                    tilemap_otherGame.SetTile(position, grey_Tile);
                }

            }

            row--;
        }



        //for (int col = bounds.xMin; col < bounds.xMax; col++)
        //{
        //    Vector3Int position = new Vector3Int(col, -boardSize.y/2, 0);
        //    tilemap_otherGame.SetTile(position, grey_Tile);
        //}


    }

    public bool HasLineTile(int row, Tilemap tilemap)
    {
        RectInt bounds = Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (tilemap.HasTile(position))
            {
                return true;
            }
        }

        return false;
    }

}
