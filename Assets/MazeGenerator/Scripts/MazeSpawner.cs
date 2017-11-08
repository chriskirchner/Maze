using UnityEngine;
using System.Collections;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	//public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
    public GameObject wayPoint;

	private BasicMazeGenerator mMazeGenerator = null;

	void Start () {
		if (!FullRandom) {
			Random.seed = RandomSeed;
		}
		switch (Algorithm) {
		case MazeGenerationAlgorithm.PureRecursive:
			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveTree:
			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RandomTree:
			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.OldestTree:
			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
			break;
		case MazeGenerationAlgorithm.RecursiveDivision:
			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
			break;
		}
        
        mMazeGenerator.GenerateMaze ();
        GameObject[] walls = new GameObject[Rows * Columns];
        int i = 0;

        for (int row = 0; row < Rows; row++) {
			for(int column = 0; column < Columns; column++){
				float x = column*(CellWidth+(AddGaps?.2f:0));
				float z = row*(CellHeight+(AddGaps?.2f:0));
				MazeCell cell = mMazeGenerator.GetMazeCell(row,column);
				GameObject tmp;
                tmp = Instantiate(wayPoint, new Vector3(x,3,z), Quaternion.identity) as GameObject;
                tmp.transform.parent = this.transform;
                //wayPoint.transform.position = new Vector3(x, 0, z);
				//tmp = Instantiate(Floor,new Vector3(x,0,z), Quaternion.Euler(0,0,0)) as GameObject;
    //            tmp.transform.parent = this.transform;
                if (cell.WallRight){
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z-CellHeight/2),Quaternion.Euler(270,90,0)) as GameObject;// right
                    tmp.transform.parent = this.transform;
                }
				if(cell.WallFront){
					tmp = Instantiate(Wall,new Vector3(x+CellWidth/2,0,z+CellHeight/2),Quaternion.Euler(270, 0,0)) as GameObject;// front
                    tmp.transform.parent = this.transform;
                }
				if(cell.WallLeft){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z+CellHeight/2),Quaternion.Euler(270, 270,0)) as GameObject;// left
                    tmp.transform.parent = this.transform;
                }
				if(cell.WallBack){
					tmp = Instantiate(Wall,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.Euler(270, 180,0)) as GameObject;// back
                    tmp.transform.parent = this.transform;
                }
				if(cell.IsGoal && GoalPrefab != null){
					tmp = Instantiate(GoalPrefab,new Vector3(x,1,z), Quaternion.Euler(0,0,0)) as GameObject;
					tmp.transform.parent = this.transform;
				}
                walls[i++] = tmp;
            }
        }


        GameObject[] pillars = new GameObject[(Rows + 1) * (Columns + 1)];
        i = 0;
        if (Pillar != null){
			for (int row = 0; row < Rows+1; row++) {
				for (int column = 0; column < Columns+1; column++) {
					float x = column*(CellWidth+(AddGaps?.2f:0));
					float z = row*(CellHeight+(AddGaps?.2f:0));
					GameObject tmp = Instantiate(Pillar,new Vector3(x-CellWidth/2,0,z-CellHeight/2),Quaternion.Euler(270,0,0)) as GameObject;
					tmp.transform.parent = this.transform;
                    pillars[i++] = tmp;
                }
            }
		}




        this.transform.Translate(new Vector3(this.transform.parent.position.x, 
            this.transform.parent.position.y, 
            this.transform.parent.position.z));

        StaticBatchingUtility.Combine(walls, this.transform.parent.gameObject);
        StaticBatchingUtility.Combine(pillars, this.transform.parent.gameObject);
    }
}
