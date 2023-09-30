using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    [Header("Grid Parameters")]
    [SerializeField] Vector2 gridSize = new Vector2(5, 5); // Tamanho da grade em c�lulas (linhas, colunas)
    [SerializeField] GridZones gridZones; // Zonas da grade (por exemplo, onde o jogador e o inimigo come�am)
    [SerializeField] CellColors cellColors; // Cores das c�lulas

    [Header("References")]
    [SerializeField] GameObject castlePrefab; // Prefab do castelo
    [SerializeField] GameObject gridCubePrefab; // Prefab das c�lulas da grade
    [SerializeField] Transform gridParent; // Pai das c�lulas da grade

    private List<Castle> castles = new List<Castle>(); // Lista de castelos no jogo
    private List<GameObject> gridCubes = new List<GameObject>(); // Lista de objetos de c�lulas da grade no jogo
    private List<CellGrid> playerGrid = new List<CellGrid>(); // Lista de c�lulas da grade do jogador
    private List<CellGrid> enemyGrid = new List<CellGrid>(); // Lista de c�lulas da grade do inimigo

    #region Getters && Setters

    public Vector2 GridSize { get => gridSize; }
    public List<Castle> Castles { get => castles; }
    public CellColors CellColors { get => cellColors; }
    public List<CellGrid> GetPlayerGrid { get => playerGrid; }
    public List<CellGrid> GetEnemyGrid { get => enemyGrid; }

    #endregion

    // Fun��o para configurar a grade
    public void SetupGrid() {
        RemoveActualGrid(); // Remove a grade existente
        // Loop para criar as c�lulas da grade com base no tamanho especificado
        for (int i = 0; i < gridSize.y; i++) {
            for (int j = 0; j < gridSize.x; j++) {
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

        SetupCastles(); // Configura os castelos
    }

    // Fun��o para configurar os castelos no in�cio do jogo
    public void SetupCastles() {
        // Encontra a c�lula de refer�ncia para o castelo do jogador
        CellGrid refenceCell = GetSpecificCellGrid(new Vector2(0, Mathf.Round(gridSize.y / 2) - 1));
        GameObject playerCastle = Instantiate(castlePrefab, new Vector3(refenceCell.transform.position.x - 1.75f, 0, refenceCell.transform.position.z), Quaternion.identity);
        playerCastle.transform.SetParent(gridParent);
        playerCastle.GetComponent<Castle>().SetupCastle(Players.Player);
        castles.Add(playerCastle.GetComponent<Castle>());

        // Encontra a c�lula de refer�ncia para o castelo do bot
        refenceCell = GetSpecificCellGrid(new Vector2(gridSize.x - 1, Mathf.Round(gridSize.y / 2) - 1));
        GameObject botCastle = Instantiate(castlePrefab, new Vector3(refenceCell.transform.position.x + 1.75f, 0, refenceCell.transform.position.z), Quaternion.identity);
        botCastle.transform.SetParent(gridParent);
        botCastle.GetComponent<Castle>().SetupCastle(Players.Bot);
        castles.Add(botCastle.GetComponent<Castle>());
    }

    // Fun��o para remover a grade existente
    public void RemoveActualGrid() {
        if (gridCubes.Count == 0)
            return;

        foreach (GameObject grid in gridCubes)
            Destroy(grid);
        foreach (Castle castle in castles)
            Destroy(castle.gameObject);

        gridCubes.Clear();
        playerGrid.Clear();
        enemyGrid.Clear();
        castles.Clear();
    }

    // Fun��o para determinar o tipo de c�lula da grade com base na posi��o
    CellGridType GetCellGridType(int pos, GridZones gridZone) {
        if (pos >= gridZone.PlayerGrid.x && pos <= gridZone.PlayerGrid.y)
            return CellGridType.Player;
        if (pos >= gridZone.EnemyGrid.x && pos <= gridZone.EnemyGrid.y)
            return CellGridType.Enemy;
        return CellGridType.Neutral;
    }

    // Fun��o para obter uma c�lula da grade adjacente a uma c�lula dada
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

    // Fun��o para obter uma c�lula da grade com base em sua posi��o
    public CellGrid GetSpecificCellGrid(Vector2 cellPosition) {
        foreach (GameObject cell in gridCubes) {
            if (cell.GetComponent<CellGrid>().CellPosition == cellPosition)
                return cell.GetComponent<CellGrid>();
        }
        return null;
    }
}

// Enumera��o para dire��o de obten��o de c�lula da grade
public enum GetDirection { Up, Down, Left, Right }

// Estrutura para definir zonas na grade
[System.Serializable]
public struct GridZones
{
    public Vector2 PlayerGrid; // Zona do jogador
    public Vector2 EnemyGrid; // Zona do inimigo
}

// Estrutura para definir cores das c�lulas da grade
[System.Serializable]
public struct CellColors
{
    [Header("Team Colors")]
    public Color Neutral; // Cor para c�lulas neutras
    public Color Player; // Cor para c�lulas do jogador
    public Color Enemy; // Cor para c�lulas do inimigo

    [Header("Hover Colors")]
    public Color NeutralHoverColor; // Cor de destaque para c�lulas neutras
    public Color PlayerHoverTiles; // Cor de destaque para c�lulas do jogador
    public Color EnemyHoverTiles; // Cor de destaque para c�lulas do inimigo
}

