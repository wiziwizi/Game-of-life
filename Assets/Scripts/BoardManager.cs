using UnityEngine;

public class BoardManager : MonoBehaviour
{

    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private GameObject _cellPrefab;
    [SerializeField] private int _rows;
    [SerializeField] private int _colums;
    [SerializeField] private float _spawnChance;
    [SerializeField] private GameObject[,] _cellMatrix;

    private Transform _gridHolder;

    private void Awake()
    {
        _cellMatrix = new GameObject[_colums, _rows];
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    public void SetupScene()
    {
        _gridHolder = new GameObject("Grid").transform;

        _mainCamera.transform.position = new Vector3(_colums / 2, _rows / 2, -10f);
        _mainCamera.GetComponent<Camera>().orthographicSize = _colums > _rows ? _colums / 2 + 1 : _rows / 2 + 1;

        var cellPosition = new Vector3(0f, 0f, 0f);

        for (var x = 0; x < _colums; x++)
        {
            for (var y = 0; y < _rows; y++)
            {
                cellPosition.x = x;
                cellPosition.y = y;
                var cellInstance = Instantiate(_cellPrefab, cellPosition, Quaternion.identity);
                cellInstance.transform.SetParent(_gridHolder);

                //Set the cell alive according with the chance of "spawnChance"
                var cellScript = cellInstance.GetComponent<CellScript>();
                cellScript.IsAlive = Random.value <= _spawnChance;

                cellScript.SetPosition(x, y);
                _cellMatrix[x, y] = cellInstance;
            }
        }
    }
    
    public int GetRows{get {return _rows;} }
    public int GetColums{get {return _colums;} }
    public GameObject[,] GetCellMatrix{get {return _cellMatrix;} }
}
