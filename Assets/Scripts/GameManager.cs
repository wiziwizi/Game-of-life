using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private float _delay;
    
    private BoardManager _boardScript;
    private float _time;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        _boardScript = GetComponent<BoardManager>();
    }

    private void Start()
    {
        _boardScript.SetupScene();
        StartCoroutine(NextGeneration());
    }

    private IEnumerator NextGeneration()
    {
        while (true)
        {
            for (var x = 0; x < _boardScript.GetRows; x++)
            {
                for (var y = 0; y < _boardScript.GetColums; y++)
                {
                    _boardScript.GetCellMatrix[x, y].GetComponent<CellScript>().ScanNeighbours();
                }
            }
            for (var x = 0; x < _boardScript.GetRows; x++)
            {
                for (var y = 0; y < _boardScript.GetColums; y++)
                {
                    _boardScript.GetCellMatrix[x, y].GetComponent<CellScript>().Determine();
                }
            }
            
            yield return new WaitForSeconds(_delay);
        }
    }
}
