using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using Vector3 = UnityEngine.Vector3;
using System.Threading;

public class Testing : MonoBehaviour {
    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
    [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    private Pathfinding pathfinding;
    private Grid<HeatMapGridObject> grid;
    // private float mouseMoveTimer;
    // private float mouseMoveTimerMax;


    private void Start() {
        pathfinding = new Pathfinding(10, 10);
        pathfindingVisual.SetGrid(pathfinding.GetGrid());
        // grid = new Grid<HeatMapGridObject>(20, 10, 8f, new Vector3(-96, -50, 0), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        // heatMapVisual.SetGrid(grid);
        // heatMapBoolVisual.SetGrid(grid);
        // heatMapGenericVisual.SetGrid(grid);
    }

    private void Update() {
        // HandleClickToModifyGrid();
        // HandleHeatMapMouseMove();

        if(Input.GetMouseButtonDown(0)) {
            // Vector3 position = UtilsClass.GetMouseWorldPosition();
            // HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            // if(heatMapGridObject != null){
            //     heatMapGridObject.AddValue(5);
            // }


            // grid.SetValue(position, true);
            // grid.AddValue(position, 100, 5, 40);

            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = pathfinding.FindPath(0,0,x,y);
            if(path!= null){
                for(int i=0; i<path.Count - 1; i++){
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i+1].x, path[i+1].y)*10f + Vector3.one * 5f, Color.green, 100f);
                }
            }
        }
        

        if(Input.GetMouseButtonDown(1)) {
            // Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x,y).SetIsWalkable(!pathfinding.GetNode(x,y).isWalkable);
        }
    }

     

    // private void HandleClickToModifyGrid(){
    //     if(Input.GetMouseButtonDown(0)) {
    //         grid.SetValue(UtilsClass.GetMouseWorldPosition(), 56);
    //     }
    // }

}

public class HeatMapGridObject{
        private const int MIN = 0;
        private const int MAX = 100;

        private Grid<HeatMapGridObject> grid;
        private int x;
        private int y;
        private int value;

        public HeatMapGridObject(Grid<HeatMapGridObject> _grid, int x, int y){
            grid = _grid;
            this.x = x;
            this.y = y;
        }
        public void AddValue(int addValue){
            value += Mathf.Clamp(addValue, MIN, MAX);
            grid.TriggerGridObjectChanged(x, y);
        }

        public float GetValueNormalized(){
            return (float)value/MAX;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }