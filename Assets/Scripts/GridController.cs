using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Parameters")]
    [SerializeField] Vector2 gridSize = new Vector2(5, 5);
    [SerializeField] GridZones gridZones;
    [SerializeField] CellColors cellColors;

    [Header("References")]
    [SerializeField] GameObject castlePrefab;
    [SerializeField] GameObject gridCubePrefab;
    [SerializeField] Transform gridParent;

    private List<Castle> castles = new List<Castle>();
    private List<GameObject> gridCubes = new List<GameObject>();
    private List<CellGrid> playerGrid = new List<CellGrid>();
    private List<CellGrid> enemyGrid = new List<CellGrid>();

    #region Getters && Setters

    public Vector2 GridSize { get => gridSize; }
    public List<Castle> Castles { get => castles; }
    public CellColors CellColors { get => cellColors; }
    public List<CellGrid> GetPlayerGrid { get => playerGrid; }
    public List<CellGrid> GetEnemyGrid { get => enemyGrid; }

    #endregion

    public void SetupGrid() {
        RemoveActualGrid();
        for(int i = 0; i < gridSize.y; i++) {
            for(int j = 0; j < gridSize.x; j++) {
                Vector3 position = new Vector3(j, 0, i);
                CellGridType cellType = GetCellGridType(j, gridZones);
                GameObject cellGrid = Instantiate(gridCubePrefab, position, Quaternion.identity);
                cellGrid.name = $"X {j} Y {i}";
                cellGrid.transform.parent = gridParent;
                cellGrid.GetComponent<CellGrid>().InitCell(cellType, new Vector2(j, i));

                if (cellType == CellGridType.Player) playerGrid.Add(cellGrid.GetComponent<CellGrid>());
                else if (cellType == CellGridType.Enemy) enemyGrid.Add(cellGrid.GetComponent<CellGrid>());

                gridCubes.Add(cellGrid);
            }
        }
        SetupCastles();
    }

    public void SetupCastles() {
        CellGrid refenceCell = GetSpecificCellGrid(new Vector2(0, Mathf.Round(gridSize.y / 2) - 1));
        GameObject playerCastle = Instantiate(castlePrefab, new Vector3(refenceCell.transform.position.x - 1.75f, 0, refenceCell.transform.position.z), Quaternion.identity);
        playerCastle.transform.SetParent(gridParent);
        playerCastle.GetComponent<Castle>().SetupCastle(Players.Player);
        castles.Add(playerCastle.GetComponent<Castle>());

        refenceCell = GetSpecificCellGrid(new Vector2(gridSize.x - 1, Mathf.Round(gridSize.y / 2) - 1));
        GameObject botCastle = Instantiate(castlePrefab, new Vector3(refenceCell.transform.position.x + 1.75f, 0, refenceCell.transform.position.z), Quaternion.identity);
        botCastle.transform.SetParent(gridParent);
        botCastle.GetComponent<Castle>().SetupCastle(Players.Bot);
        castles.Add(botCastle.GetComponent<Castle>());
    }

    public void RemoveActualGrid() {
        if (gridCubes.Count == 0)
            return;

        foreach(GameObject grid in gridCubes)
            Destroy(grid);
        foreach (Castle castle in castles)
            Destroy(castle.gameObject);

        gridCubes.Clear();
        playerGrid.Clear();
        enemyGrid.Clear();
        castles.Clear();
    }

    CellGridType GetCellGridType(int pos, GridZones gridZone) {
        if (pos >= gridZone.PlayerGrid.x && pos <= gridZone.PlayerGrid.y)
            return CellGridType.Player;
        if (pos >= gridZone.EnemyGrid.x && pos <= gridZone.EnemyGrid.y)
            return CellGridType.Enemy;
        return CellGridType.Neutral;
    }

    public CellGrid GetAroundCellGrid(CellGrid actualCell, int distance, GetDirection direction) {
        foreach (GameObject cell in gridCubes) {
            switch (direction) {
                case GetDirection.Up:
                    if (cell.GetComponent<CellGrid>().CellPosition.x == actualCell.CellPosition.x && cell.GetComponent<CellGrid>().CellPosition.y == actualCell.CellPosition.y + 1 * distance) return cell.GetComponent<CellGrid>();
                    break;
                case GetDirection.Down:
                    if (cell.GetComponent<CellGrid>().CellPosition.x == actualCell.CellPosition.x && cell.GetComponent<CellGrid>().CellPosition.y == actualCell.CellPosition.y - 1 * distance) return cell.GetComponent<CellGrid>();
                    break;
                case GetDirection.Left:
                    if (cell.GetComponent<CellGrid>().CellPosition.y == actualCell.CellPosition.y && cell.GetComponent<CellGrid>().CellPosition.x == actualCell.CellPosition.x - 1 * distance) return cell.GetComponent<CellGrid>();
                    break;
                case GetDirection.Right:
                    if (cell.GetComponent<CellGrid>().CellPosition.y == actualCell.CellPosition.y && cell.GetComponent<CellGrid>().CellPosition.x == actualCell.CellPosition.x + 1 * distance) return cell.GetComponent<CellGrid>();
                    break;
            }
        } 
        return null;
    }

    public CellGrid GetSpecificCellGrid(Vector2 cellPosition) {
        foreach(GameObject cell in gridCubes) {
            if(cell.GetComponent<CellGrid>().CellPosition == cellPosition)
                return cell.GetComponent<CellGrid>();
        }
        return null;
    }
}

public enum GetDirection { Up, Down, Left, Right }

[System.Serializable]
public struct GridZones
{
    public Vector2 PlayerGrid;
    public Vector2 EnemyGrid;
}

[System.Serializable]
public struct CellColors
{
    [Header("Team Colors")]
    public Color Neutral;
    public Color Player;
    public Color Enemy;

    [Header("Hover Colors")]
    public Color NeutralHoverColor;
    public Color PlayerHoverTiles;
    public Color EnemyHoverTiles;
}
