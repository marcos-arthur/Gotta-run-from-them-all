using UnityEngine;

public class PathfindingVisual: MonoBehaviour
{
    private Grid<PathNode> grid;
    private Mesh mesh;
    private bool updateMesh;
    private void Awake(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(Grid<PathNode> _grid){
        grid = _grid;
        UpdateVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<PathNode>.OnGridValueChangedEventArgs e){
        // UpdateHeatMapVisual();
        updateMesh = true;
    }

    private void LateUpdate(){
        if(updateMesh){
            updateMesh = false;
            UpdateVisual();
        }
    }

    public void UpdateVisual(){
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth()* grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
        
        for(int x = 0; x < grid.GetWidth(); x++){
            for(int y = 0; y < grid.GetHeight(); y++){
                int index = x* grid.GetHeight() + y;
                Vector3 quadSize = new Vector3(1,1) * grid.GetCellSize();

                PathNode pathNode = grid.GetGridObject(x,y);

                if(pathNode.isWalkable){
                    quadSize = Vector3.zero;
                }

                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x,y) + quadSize * .5f, 0f, quadSize, Vector2.zero, Vector2.zero);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
