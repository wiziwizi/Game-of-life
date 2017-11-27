using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ListLife : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    List<List<int>> _actualState;
    List<Cell> _redrawList;
    int _topPointer, _middlePointer, _bottomPointer;

    void Initialize()
    {
        _actualState = new List<List<int>>();
        _redrawList = new List<Cell>();
        _topPointer = _middlePointer = _bottomPointer = 1;
    }

    int NextGeneration()
    {
        int x, y, n, t1, t2, alive = 0;
        int neighbours;

        Cell key;
        Neighbours deadNeighbours = new Neighbours(new List<Cell>());
        Dictionary<Cell, int> allDeadNeighbours = new Dictionary<Cell, int>();
        List<List<int>> newState = new List<List<int>>();

        for (int i = 0; i < _actualState.Count; i++)
        {
            _topPointer = 1;
            _bottomPointer = 1;
            for (int j = 1; j < _actualState[i].Count; j++)
            {
                x = _actualState[i][j];
                y = _actualState[i][0];

                deadNeighbours.add(x - 1, y - 1, 1)
                    .add(x, y - 1, 1)
                    .add(x + 1, y - 1, 1)
                    .add(x - 1, y, 1)
                    .add(x + 1, y, 1)
                    .add(x - 1, y + 1, 1)
                    .add(x, y + 1, 1)
                    .add(x + 1, y + 1, 1);

                neighbours = GetNeighboursFromAlive(x, y, i, deadNeighbours);

                for (int m = 0; m < 8; m++)
                {
                    if (true) //TODO: if (deadNeighbours[m] !== undefined)
                    {
                        key = new Cell(deadNeighbours.getCell(m).x, deadNeighbours.getCell(m).y, 1);

                        if (!allDeadNeighbours.ContainsKey(key))
                        {
                            allDeadNeighbours.Add(key, 1);
                        }
                        else
                        {
                            allDeadNeighbours[key]++;
                        }
                    }
                }

                if (!(neighbours == 0 || neighbours == 1 || neighbours > 3))
                {
                    AddCell(x, y, newState);
                    alive++;
                    _redrawList.Add(new Cell(x, y, 2));
                }
                else
                {
                    _redrawList.Add(new Cell(x, y, 0));
                }
            }
        }

        //Process dead neighbours
        foreach (Cell keyCell in allDeadNeighbours.Keys)
        {
            if (allDeadNeighbours[keyCell] == 3)
            {
                AddCell(keyCell.x, keyCell.y, newState);
                alive++;
                _redrawList.Add(new Cell(keyCell.x, keyCell.y, 1));
            }
        }

        _actualState = newState;

        return alive;
    }

    int GetNeighboursFromAlive(int x, int y, int i, Neighbours possibleNeighbours)
    {
        int neighbourCount = 0;
        int k;

        //TOP
        if (_actualState[i - 1] != null)
        {
            if (_actualState[i - 1][0] == (y - 1))
            {
                for (k = _topPointer; k < _actualState[i - 1].Count; k++)
                {
                    if (_actualState[i - 1][k] >= (x - 1))
                    {
                        if (_actualState[i - 1][k] == (x - 1))
                        {
                            possibleNeighbours.list[0] = null;
                            _topPointer = k + 1;
                            neighbourCount++;
                        }
                        if (_actualState[i - 1][k] == x)
                        {
                            possibleNeighbours.list[1] = null;
                            _topPointer = k;
                            neighbourCount++;
                        }
                        if (_actualState[i - 1][k] == (x + 1))
                        {
                            possibleNeighbours.list[2] = null;

                            if (k == 1)
                            {
                                _topPointer = 1;
                            }
                            else
                            {
                                _topPointer = k - 1;
                            }

                            neighbourCount++;
                        }
                        if (_actualState[i - 1][k] > (x + 1))
                        {
                            break;
                        }
                    }
                }
            }
        }

        //MIDDLE
        for (k = 1; k < _actualState[i].Count; k++)
        {
            if (_actualState[i][k] >= (x - 1))
            {
                if (_actualState[i][k] == (x - 1))
                {
                    possibleNeighbours.list[3] = null;
                    neighbourCount++;
                }
                if (_actualState[i][k] == (x + 1))
                {
                    possibleNeighbours.list[4] = null;
                    neighbourCount++;
                }
                if (_actualState[i][k] > (x + 1))
                {
                    break;
                }
            }
        }

        //BOTTOM
        if (_actualState[i + 1] != null)
        {
            if (_actualState[i + 1][0] == (y + 1))
            {
                for (k = _bottomPointer; k < _actualState[i + 1].Count; k++)
                {
                    if (_actualState[i + 1][k] >= (x - 1))
                    {
                        if (_actualState[i + 1][k] == (x - 1))
                        {
                            possibleNeighbours.list[5] = null;
                            _bottomPointer = k + 1;
                            neighbourCount++;
                        }
                        if (_actualState[i + 1][k] == x)
                        {
                            possibleNeighbours.list[6] = null;
                            _bottomPointer = k;
                            neighbourCount++;
                        }
                        if (_actualState[i + 1][k] == (x + 1))
                        {
                            possibleNeighbours.list[7] = null;

                            if (k == 1)
                            {
                                _bottomPointer = 1;
                            }
                            else
                            {
                                _bottomPointer = k - 1;
                            }

                            neighbourCount++;
                        }
                        if (_actualState[i + 1][k] > (x + 1))
                        {
                            break;
                        }
                    }
                }
            }
        }

        return neighbourCount;
    }

    bool IsAlive(int x, int y)
    {
        for (int i = 0; i < _actualState.Count; i++)
        {
            if (_actualState[i][0] == y)
            {
                for (int j = 0; j < _actualState[i].Count; j++)
                {
                    if (_actualState[i][j] == x)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    void RemoveCell(int x, int y, List<List<int>> state)
    {
        for (int i = 0; i < state.Count; i++)
        {
            if (state[i][0] == y)
            {
                if (state[i].Count == 2)
                {
                    state.RemoveAt(i);
                }
                else
                {
                    for (int j = 0; j < state[i].Count; j++)
                    {
                        if (state[i][j] == x)
                        {
                            state[i].RemoveAt(j);
                        }
                    }
                }
            }
        }
    }


    void AddCell(int x, int y, List<List<int>> state)
    {
        if (state.Count == 0)
        {
            List<int> newRow = new List<int> {y, x};
            state.Add(newRow);
            return;
        }

        int k, n, m, added;
        List<int> tempRow = new List<int>();
        List<List<int>> newState = new List<List<int>>();

        if (y < state[0][0])
        {
            //tempRow.Add
            //newState
        }
    }

}
