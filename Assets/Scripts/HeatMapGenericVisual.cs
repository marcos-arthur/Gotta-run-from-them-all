using UnityEngine;

public class HeatMapGenericVisual: MonoBehaviour
{
    private Grid<HeatMapGridObject> grid;
    private Mesh mesh;
    private bool updateMesh;
    private void Awake(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetGrid(Grid<HeatMapGridObject> _grid){
        grid = _grid;
        UpdateHeatMapVisual();

        grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, Grid<HeatMapGridObject>.OnGridValueChangedEventArgs e){
        // UpdateHeatMapVisual();
        updateMesh = true;
    }

    private void LateUpdate(){
        if(updateMesh){
            updateMesh = false;
            UpdateHeatMapVisual();
        }
    }

    public void UpdateHeatMapVisual(){
        MeshUtils.CreateEmptyMeshArrays(grid.GetWidth()* grid.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);
        
        for(int x = 0; x < grid.GetWidth(); x++){
            for(int y = 0; y < grid.GetHeight(); y++){
                int index = x* grid.GetHeight() + y;

                HeatMapGridObject gridValue = grid.GetGridObject(x,y);
                // float gridValueNormalized = Mathf.Clamp01((float) gridValue/maxGridValue);
                float gridValueNormalized = gridValue.GetValueNormalized();
                Vector2 gridCellUV = new Vector2(gridValueNormalized, 0f);

                Vector3 quadSize = new Vector3(1,1) * grid.GetCellSize();
                MeshUtils.AddToMeshArrays(vertices, uv, triangles, index, grid.GetWorldPosition(x,y) + quadSize * .5f, 0f, quadSize, gridCellUV, gridCellUV);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}
