using System.Collections.Generic;

public class Neighbours
{
	public List<Cell> list;

	public Neighbours(List<Cell> _cells)
	{
		list = _cells;
	}

	public Neighbours add(int x, int y, int state)
	{
		list.Add(new Cell(x, y, state));
		return this;
	}

	public Cell getCell(int index)
	{
		return list[index];
	}
}
