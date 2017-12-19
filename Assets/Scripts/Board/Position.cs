using System.Collections;

[System.Serializable]
public class Position {

	public int row;
	public int col;


	public Position(int r, int c) {
		row = r;
		col = c;
	}

	public Position Move(Actor.Action action) {
		int nr = row;
		int nc = col;
		switch (action) {
			case Actor.Action.MOVE_U:
				nr = row-1;
				break;
			case Actor.Action.MOVE_D:
				nr = row+1;
				break;
			case Actor.Action.MOVE_L:
				nc = col-1;
				break;
			case Actor.Action.MOVE_R:
				nc = col+1;
				break;
		}
		return new Position(nr, nc);
	}

}