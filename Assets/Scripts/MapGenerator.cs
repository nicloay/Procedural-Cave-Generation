using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapGenerator : MonoBehaviour {

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;

    public Action OnMapGenerationDone;

	[Range(0,100)]
	public int randomFillPercent;

    public MeshGenerator meshGen;

    public int WallsNumber{
        get{
            return meshGen.OutlinesCount;
        }
    }


    public Vector3 GetNodeGlobalPosition(int x, int y){
        return meshGen.squareGrid.squares[x,y].CentralControlNode.position;
    }

    public bool IsNodeFree(int x, int y){
        return GameData.Map[x,y] == 0;
    }

    public int GetMapNodeValue(int x, int y){
        return GameData.Map[x,y];
    }
    /// <summary>
    /// Gets the random map node.*/
    /// </summary>
    /// <returns><c>true</c>, if random map node was gotten, <c>false</c> otherwise.</returns>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="value">return true if value has 1 or false if it's 0</param>
    public bool GetRandomMapNode(out int x, out int y, out int value){
        x = UnityEngine.Random.Range(0, GameData.Map.GetLength(0));
        y = UnityEngine.Random.Range(0, GameData.Map.GetLength(1));
        value = GetMapNodeValue(x,y);
        return value == 1;
    }


    static float epsilon = 0.1f;
    static Vector3[] CollisionOffsets = new Vector3[]{
        new Vector3(0,0,0),new Vector3(0,0,epsilon),new Vector3(0,0,-epsilon)
        ,new Vector3(epsilon,0,0),new Vector3(-epsilon,0,0) 
    };
    /// <summary>
    /// Removes the node at global position if its not A wall.
    /// ATTENTION: it doesn't actually change mesh
    /// We have a small issue here. Physical contact with wall doesn't mean that 
    ///    it will directly map to map indexes because of float issue calculation and because we floor value
    ///    so what we will do here is check neigbour indexes with small offsets
    /// </summary>
    /// <returns><c>true</c>, if node was changed, <c>false</c> otherwise.</returns>
    /// <param name="globalPosition">Global position.</param>
    public bool RemoveNodeAtGlobalPositionIfItsNotAWall(Vector3 globalPosition){
        int x,y;
        foreach(Vector3 offset in CollisionOffsets){            
            if (meshGen.squareGrid.GlobalPositionToNodeIndexes(globalPosition + offset, out x, out y)){
                if (!IsNodeAtIndexesAwall(x,y)){
                    if (GameData.Map[x,y] == 1){                        
                        GameData.Map[x,y] = 0;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    bool IsNodeAtIndexesAwall(int x, int y){
        return (x == 0 || y == 0 || x == (width - 1) || y == (height - 1));
    }


	void Start() {
        meshGen = GetComponent<MeshGenerator>();
        if (GameData.Map == null){
            GenerateMap();           
        } else {
            width = GameData.Map.GetLength(0);
            height = GameData.Map.GetLength(1);
        }
        RegenrateMesh();
        DynamicGI.UpdateEnvironment();
        if (OnMapGenerationDone != null){
            OnMapGenerationDone();
        }
	}       	

	public void GenerateMap() {
		
        GameData.Map = new byte[width,height];
		RandomFillMap();


        for (int i = 0; i < 5; i ++) {
            SmoothMap();
        }

        ProcessMap ();
        int borderSize = 1;
        byte[,] borderedMap = new byte[width + borderSize * 2,height + borderSize * 2];

        for (int x = 0; x < borderedMap.GetLength(0); x ++) {
            for (int y = 0; y < borderedMap.GetLength(1); y ++) {
                if (x >= borderSize && x < width + borderSize && y >= borderSize && y < height + borderSize) {
                    borderedMap[x,y] = GameData.Map[x-borderSize,y-borderSize];
                }
                else {
                    borderedMap[x,y] =1;
                }
            }
        }  
        GameData.Map = borderedMap;
	}


    public void RegenrateMesh(){                 
        meshGen.GenerateMesh(GameData.Map, 1);
    }

	void ProcessMap() {
		List<List<Coord>> wallRegions = GetRegions (1);
		int wallThresholdSize = 50;

		foreach (List<Coord> wallRegion in wallRegions) {
			if (wallRegion.Count < wallThresholdSize) {
				foreach (Coord tile in wallRegion) {
					GameData.Map[tile.tileX,tile.tileY] = 0;
				}
			}
		}

		List<List<Coord>> roomRegions = GetRegions (0);
		int roomThresholdSize = 50;
		List<Room> survivingRooms = new List<Room> ();
		
		foreach (List<Coord> roomRegion in roomRegions) {
			if (roomRegion.Count < roomThresholdSize) {
				foreach (Coord tile in roomRegion) {
					GameData.Map[tile.tileX,tile.tileY] = 1;
				}
			}
			else {
				survivingRooms.Add(new Room(roomRegion, GameData.Map));
			}
		}
		survivingRooms.Sort ();
		survivingRooms [0].isMainRoom = true;
		survivingRooms [0].isAccessibleFromMainRoom = true;

		ConnectClosestRooms (survivingRooms);
	}

	void ConnectClosestRooms(List<Room> allRooms, bool forceAccessibilityFromMainRoom = false) {

		List<Room> roomListA = new List<Room> ();
		List<Room> roomListB = new List<Room> ();

		if (forceAccessibilityFromMainRoom) {
			foreach (Room room in allRooms) {
				if (room.isAccessibleFromMainRoom) {
					roomListB.Add (room);
				} else {
					roomListA.Add (room);
				}
			}
		} else {
			roomListA = allRooms;
			roomListB = allRooms;
		}

		int bestDistance = 0;
		Coord bestTileA = new Coord ();
		Coord bestTileB = new Coord ();
		Room bestRoomA = new Room ();
		Room bestRoomB = new Room ();
		bool possibleConnectionFound = false;

		foreach (Room roomA in roomListA) {
			if (!forceAccessibilityFromMainRoom) {
				possibleConnectionFound = false;
				if (roomA.connectedRooms.Count > 0) {
					continue;
				}
			}

			foreach (Room roomB in roomListB) {
				if (roomA == roomB || roomA.IsConnected(roomB)) {
					continue;
				}
			
				for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA ++) {
					for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB ++) {
						Coord tileA = roomA.edgeTiles[tileIndexA];
						Coord tileB = roomB.edgeTiles[tileIndexB];
						int distanceBetweenRooms = (int)(Mathf.Pow (tileA.tileX-tileB.tileX,2) + Mathf.Pow (tileA.tileY-tileB.tileY,2));

						if (distanceBetweenRooms < bestDistance || !possibleConnectionFound) {
							bestDistance = distanceBetweenRooms;
							possibleConnectionFound = true;
							bestTileA = tileA;
							bestTileB = tileB;
							bestRoomA = roomA;
							bestRoomB = roomB;
						}
					}
				}
			}
			if (possibleConnectionFound && !forceAccessibilityFromMainRoom) {
				CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			}
		}

		if (possibleConnectionFound && forceAccessibilityFromMainRoom) {
			CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
			ConnectClosestRooms(allRooms, true);
		}

		if (!forceAccessibilityFromMainRoom) {
			ConnectClosestRooms(allRooms, true);
		}
	}

	void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB) {
		Room.ConnectRooms (roomA, roomB);
		//Debug.DrawLine (CoordToWorldPoint (tileA), CoordToWorldPoint (tileB), Color.green, 100);

		List<Coord> line = GetLine (tileA, tileB);
		foreach (Coord c in line) {
			DrawCircle(c,5);
		}
	}

	void DrawCircle(Coord c, int r) {
		for (int x = -r; x <= r; x++) {
			for (int y = -r; y <= r; y++) {
				if (x*x + y*y <= r*r) {
					int drawX = c.tileX + x;
					int drawY = c.tileY + y;
					if (IsInMapRange(drawX, drawY)) {
						GameData.Map[drawX,drawY] = 0;
					}
				}
			}
		}
	}

	List<Coord> GetLine(Coord from, Coord to) {
		List<Coord> line = new List<Coord> ();

		int x = from.tileX;
		int y = from.tileY;

		int dx = to.tileX - from.tileX;
		int dy = to.tileY - from.tileY;

		bool inverted = false;
		int step = Math.Sign (dx);
		int gradientStep = Math.Sign (dy);

		int longest = Mathf.Abs (dx);
		int shortest = Mathf.Abs (dy);

		if (longest < shortest) {
			inverted = true;
			longest = Mathf.Abs(dy);
			shortest = Mathf.Abs(dx);

			step = Math.Sign (dy);
			gradientStep = Math.Sign (dx);
		}

		int gradientAccumulation = longest / 2;
		for (int i =0; i < longest; i ++) {
			line.Add(new Coord(x,y));

			if (inverted) {
				y += step;
			}
			else {
				x += step;
			}

			gradientAccumulation += shortest;
			if (gradientAccumulation >= longest) {
				if (inverted) {
					x += gradientStep;
				}
				else {
					y += gradientStep;
				}
				gradientAccumulation -= longest;
			}
		}

		return line;
	}

	Vector3 CoordToWorldPoint(Coord tile) {
		return new Vector3 (-width / 2 + .5f + tile.tileX, 2, -height / 2 + .5f + tile.tileY);
	}

	List<List<Coord>> GetRegions(int tileType) {
		List<List<Coord>> regions = new List<List<Coord>> ();
		int[,] mapFlags = new int[width,height];

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (mapFlags[x,y] == 0 && GameData.Map[x,y] == tileType) {
					List<Coord> newRegion = GetRegionTiles(x,y);
					regions.Add(newRegion);

					foreach (Coord tile in newRegion) {
						mapFlags[tile.tileX, tile.tileY] = 1;
					}
				}
			}
		}

		return regions;
	}

	List<Coord> GetRegionTiles(int startX, int startY) {
		List<Coord> tiles = new List<Coord> ();
		int[,] mapFlags = new int[width,height];
		int tileType = GameData.Map [startX, startY];

		Queue<Coord> queue = new Queue<Coord> ();
		queue.Enqueue (new Coord (startX, startY));
		mapFlags [startX, startY] = 1;

		while (queue.Count > 0) {
			Coord tile = queue.Dequeue();
			tiles.Add(tile);

			for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++) {
				for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++) {
					if (IsInMapRange(x,y) && (y == tile.tileY || x == tile.tileX)) {
						if (mapFlags[x,y] == 0 && GameData.Map[x,y] == tileType) {
							mapFlags[x,y] = 1;
							queue.Enqueue(new Coord(x,y));
						}
					}
				}
			}
		}
		return tiles;
	}

	bool IsInMapRange(int x, int y) {
		return x >= 0 && x < width && y >= 0 && y < height;
	}


	void RandomFillMap() {
		if (useRandomSeed) {
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				if (x == 0 || x == width-1 || y == 0 || y == height -1) {
					GameData.Map[x,y] = 1;
				}
				else {
                    GameData.Map[x,y] = (byte)( (pseudoRandom.Next(0,100) < randomFillPercent)? 1: 0);
				}
			}
		}
	}

	void SmoothMap() {
		for (int x = 0; x < width; x ++) {
			for (int y = 0; y < height; y ++) {
				int neighbourWallTiles = GetSurroundingWallCount(x,y);

				if (neighbourWallTiles > 4)
					GameData.Map[x,y] = 1;
				else if (neighbourWallTiles < 4)
					GameData.Map[x,y] = 0;

			}
		}
	}

	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (IsInMapRange(neighbourX,neighbourY)) {
					if (neighbourX != gridX || neighbourY != gridY) {
						wallCount += GameData.Map[neighbourX,neighbourY];
					}
				}
				else {
					wallCount ++;
				}
			}
		}

		return wallCount;
	}

	struct Coord {
		public int tileX;
		public int tileY;

		public Coord(int x, int y) {
			tileX = x;
			tileY = y;
		}
	}


	class Room : IComparable<Room> {
		public List<Coord> tiles;
		public List<Coord> edgeTiles;
		public List<Room> connectedRooms;
		public int roomSize;
		public bool isAccessibleFromMainRoom;
		public bool isMainRoom;

		public Room() {
		}

		public Room(List<Coord> roomTiles, byte[,] map) {
			tiles = roomTiles;
			roomSize = tiles.Count;
			connectedRooms = new List<Room>();

			edgeTiles = new List<Coord>();
			foreach (Coord tile in tiles) {
				for (int x = tile.tileX-1; x <= tile.tileX+1; x++) {
					for (int y = tile.tileY-1; y <= tile.tileY+1; y++) {
						if (x == tile.tileX || y == tile.tileY) {
							if (map[x,y] == 1) {
								edgeTiles.Add(tile);
							}
						}
					}
				}
			}
		}

		public void SetAccessibleFromMainRoom() {
			if (!isAccessibleFromMainRoom) {
				isAccessibleFromMainRoom = true;
				foreach (Room connectedRoom in connectedRooms) {
					connectedRoom.SetAccessibleFromMainRoom();
				}
			}
		}

		public static void ConnectRooms(Room roomA, Room roomB) {
			if (roomA.isAccessibleFromMainRoom) {
				roomB.SetAccessibleFromMainRoom ();
			} else if (roomB.isAccessibleFromMainRoom) {
				roomA.SetAccessibleFromMainRoom();
			}
			roomA.connectedRooms.Add (roomB);
			roomB.connectedRooms.Add (roomA);
		}

		public bool IsConnected(Room otherRoom) {
			return connectedRooms.Contains(otherRoom);
		}

		public int CompareTo(Room otherRoom) {
			return otherRoom.roomSize.CompareTo (roomSize);
		}
	}

}
