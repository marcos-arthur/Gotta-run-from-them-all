using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine.PlayerLoop;
using Vector3 = UnityEngine.Vector3;

public class Testing : MonoBehaviour {
    [SerializeField] private HeatMapVisual heatMapVisual;
    [SerializeField] private HeatMapBoolVisual heatMapBoolVisual;
    [SerializeField] private HeatMapGenericVisual heatMapGenericVisual;
    private Grid<HeatMapGridObject> grid;
    // private float mouseMoveTimer;
    // private float mouseMoveTimerMax;


    private void Start() {
        grid = new Grid<HeatMapGridObject>(20, 10, 8f, new Vector3(-96, -50, 0), (Grid<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));

        // heatMapVisual.SetGrid(grid);
        // heatMapBoolVisual.SetGrid(grid);
        heatMapGenericVisual.SetGrid(grid);
    }

    private void Update() {
        // HandleClickToModifyGrid();
        // HandleHeatMapMouseMove();

        if(Input.GetMouseButtonDown(0)) {
            Vector3 position = UtilsClass.GetMouseWorldPosition();
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position);
            if(heatMapGridObject != null){
                heatMapGridObject.AddValue(5);
            }
            // grid.SetValue(position, true);
            // grid.AddValue(position, 100, 5, 40);
        }

        // if(Input.GetMouseButtonDown(1)) {
        //     Debug.Log(grid.GetValue(UtilsClass.GetMouseWorldPosition()));
        // }
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