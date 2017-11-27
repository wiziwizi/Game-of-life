using UnityEngine;

public class CellScript : MonoBehaviour
{

    public bool IsAlive;
    
    [SerializeField] private Sprite _aliveSprite;
    [SerializeField] private Sprite _deadSprite;

    private BoardManager _boardScript;
    private GameObject[,] _cellMatrix;
    private SpriteRenderer _spriteRenderer;

    private int _aliveNeighbours;
    private int _x;
    private int _y;
    private bool _isSpriteSet;

    private void Awake()
    {
        _boardScript = GameManager.instance.GetComponent<BoardManager>();
        _cellMatrix = _boardScript.GetCellMatrix;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _isSpriteSet = false;
    }

    private void Update()
    {
        if (_isSpriteSet) return;
        _spriteRenderer.sprite = IsAlive ? _aliveSprite : _deadSprite;
        _isSpriteSet = true;
    }

    public void ScanNeighbours()
    {
        _aliveNeighbours = 0;

        for (var x = _x - 1; x <= _x + 1; x++)
        {
            for (var y = _y - 1; y <= _y + 1; y++)
            {
                if (x == _x && y == _y) continue;
                if (x < 0 || x >= _boardScript.GetColums || y < 0 || y >= _boardScript.GetRows) continue;
                if (_cellMatrix[x, y].GetComponent<CellScript>().IsAlive)
                    _aliveNeighbours++;
            }
        }
    }

    public void Determine()
    {
        if (IsAlive && (_aliveNeighbours < 2 || _aliveNeighbours > 3))
        {
            IsAlive = false;
        }
        else if (IsAlive && (_aliveNeighbours == 2 || _aliveNeighbours == 3))
        {
            IsAlive = true;
        }
        else if (!IsAlive && _aliveNeighbours == 3)
        {
            IsAlive = true;
        }
        _isSpriteSet = false;
    }

    public void SetPosition(int x, int y)
    {
        _x = x;
        _y = y;
    }
}
