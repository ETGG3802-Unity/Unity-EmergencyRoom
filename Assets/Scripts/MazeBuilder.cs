using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MazeBuilder : MonoBehaviour 
{

	public enum Tile : int
	{
		NONE = -1,
		WALL = 0,
		LOBBY = 1,
		CNE = 2,
		CNW = 3,
		CSE = 4,
		CSW = 5,
		HNS = 6, //hall running from north to south
		HEW = 7
	}

	public enum Dir
	{
		N,
		S,
		E,
		W,
		NONE
	}

	public int width;
	public int height;
	public float tWidth;
	public float tHeight;
	public float minDensity;
	public float maxDensity;
	List<List<Tile>> maze;
	List<Transform> tileList;
	int numTiles;

	public List<Transform> otherObjects;

	int minTiles;
	int maxTiles;

	Random rand;

	public Transform NONE; //for debug purposes only
	public Transform WALL;
	public Transform LOBBY;
	public Transform CNE;
	public Transform CNW;
	public Transform CSE;
	public Transform CSW;
	public Transform HNS; //hall running from north to south
	public Transform HEW;
	
	public bool isLegal(int x, int y)
	{
		if (x >= width || y >= height)
			return false;

		if (x < 0 || y < 0)
			return false;

		if (maze[x][y] != Tile.WALL)
			return false;

		return true;

	}

	public List<int> getNext(int x, int y, Dir d)
	{
		int cn = -1;
		int rn = -1;

		if (d == Dir.N)
		{
			cn = x;
			rn = y - 1;
		}
		else if (d == Dir.S)
		{
			cn = x;
			rn = y + 1;
		}
		else if (d == Dir.E)
		{
			cn = x + 1;
			rn = y;
		}
		else if (d == Dir.W)
		{
			cn = x - 1;
			rn = y;
		}

		return new List<int> {cn, rn}; //No tuples in this framework :(
	}

	public List<Tile> getTiles(Dir ndir){
		List<Tile> tiles = new List<Tile> {Tile.LOBBY, Tile.CNE, Tile.CNW, Tile.CSE, Tile.CSW, Tile.HNS, Tile.HEW};

		if (ndir == Dir.N)
		{
			tiles.Remove(Tile.CNE);
			tiles.Remove(Tile.CNW);
			tiles.Remove(Tile.HEW);
		}
		else if (ndir == Dir.S)
		{
			tiles.Remove(Tile.CSE);
			tiles.Remove(Tile.CSW);
			tiles.Remove(Tile.HEW);
		}
		else if (ndir == Dir.E)
		{
			tiles.Remove(Tile.CNE);
			tiles.Remove(Tile.CSE);
			tiles.Remove(Tile.HNS);
		}
		else if (ndir == Dir.W)
		{
			tiles.Remove(Tile.CNW);
			tiles.Remove(Tile.CSW);
			tiles.Remove(Tile.HNS);
		}

		return tiles;
	}

	public void buildMaze(int x, int y, Tile tile, Dir ldir)
	{

		int c = x;
		int r = y;
		List<int> next = new List<int> {-1,-1};
		List<Tile> tiles;
		Tile tnext = Tile.LOBBY;

		maze[c][r] = tile;
		numTiles += 1;

		if (tile == Tile.LOBBY)
		{		
			List<Dir> dirs = new List<Dir> {Dir.N, Dir.S, Dir.E, Dir.W};
			//random.shuffle(dirs)
			foreach (Dir d in dirs)
			{
				next = getNext(c, r, d);
				if (isLegal(next[0], next[1]))
				{
					tiles = getTiles(d);
					tiles.Remove(Tile.LOBBY);
					tnext = tiles[Random.Range(0,tiles.Count)];
					//print(tnext + ", " + d + ", (" + next[0] + "," + next[1] + ")\n"); 
					buildMaze(next[0], next[1], tnext , d);
				}
			}

			return;
		}

		Dir ndir = Dir.NONE;

		if (tile == Tile.CNE)
		{
			if (ldir == Dir.S)
				ndir = Dir.E;
			else if (ldir == Dir.W)
				ndir = Dir.N;
		}
		else if (tile == Tile.CNW)
		{
			if (ldir == Dir.S)
				ndir = Dir.W;
			else if (ldir == Dir.E)
				ndir = Dir.N;
		}
		else if (tile == Tile.CSE)
		{
			if (ldir == Dir.N)
				ndir = Dir.E;
			else if (ldir == Dir.W)
				ndir = Dir.S;
		}
		else if (tile == Tile.CSW)
		{
			if (ldir == Dir.N)
				ndir = Dir.W;
			else if (ldir == Dir.E)
				ndir = Dir.S;
		}
		else if (tile == Tile.HNS || tile == Tile.HEW)
			ndir = ldir;

		tiles = getTiles(ndir);
			
		next = getNext(c, r, ndir);
		if (isLegal(next[0], next[1]))
		{
			tnext = tiles[Random.Range(0, tiles.Count)];
			//print(tnext + ", " + ndir + ", (" + next[0] + "," + next[1] + ")\n");
			buildMaze(next[0], next[1], tnext, ndir);
		}

		return;
	}

	public void createMaze()
	{
		float tileWidth = tWidth;
		float tileHeight = tHeight;
       	float x = 0;
        float z = 0;
		float divWidth = (width / 2) * tileWidth;
		float divHeight = (height / 2) * -tileHeight;
        Tile tile;

		for (int i = 0; i < width; i++) {
			maze.Add(new List<Tile>());
			for (int j = 0; j < height; j++) {
				tile = maze[i][j];
				Transform pref = tileList[(int)tile];
				x = ((i * tileWidth) - divWidth);
				print (x);
				z = ((j * -tileHeight) - divHeight); //Required for the way Unity reads coordinates
				Instantiate(pref, new Vector3(x, 0, z), Quaternion.identity);
			}
		}
	}
	// Use this for initialization
	void Start () {
		/*
		width = 20;
		height = 20;
		minDensity = .25F; //Defines the minimum number of tiles based on the size of the maze
		maxDensity = .60F;
		*/
		
		maze = new List<List<Tile>>();
		
		for (int i = 0; i < width; i++) {
			maze.Add(new List<Tile>());
			for (int j = 0; j < height; j++) {
				maze[i].Add(Tile.WALL);
			}
		}
		
		int area = width * height;
		int minTiles = (int)(minDensity * area);
		int maxTiles = (int)(maxDensity * area);
		
		tileList = new List<Transform>();
		tileList.Add (WALL);
		tileList.Add (LOBBY);
		tileList.Add (CNE);
		tileList.Add (CNW);
		tileList.Add (CSE);
		tileList.Add (CSW);
		tileList.Add (HNS); //hall running from north to south
		tileList.Add (HEW);

		buildMaze (10, 10, Tile.LOBBY, Dir.N);
		createMaze();

	}

}
