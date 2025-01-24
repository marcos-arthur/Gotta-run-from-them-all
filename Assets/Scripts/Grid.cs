using System;
using CodeMonkey.Utils;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Grid<TGridObject>{


    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs: EventArgs{
        public int x;
        public int y;
    }
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;
    
    public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
        width = _width;
        height = _height;
        cellSize = _cellSize;
        originPosition = _originPosition;

        gridArray = new TGridObject[width,height];

        for(int x = 0; x < gridArray.GetLength(0); x++){
            for(int y = 0; y < gridArray.GetLength(1); y++){
                gridArray[x,y] = createGridObject(this, x, y);
            }
        }
        
        bool showDebug = false;
        if(showDebug){
            TextMesh[,] debugTextArray = new TextMesh[width,height];
            for(int x = 0; x < gridArray.GetLength(0); x++){
                for(int y = 0; y < gridArray.GetLength(1); y++){
                    debugTextArray[x,y] = UtilsClass.CreateWorldText(gridArray[x,y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x,y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x,y), GetWorldPosition(x+1,y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0,height), GetWorldPosition(width,height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width,0), GetWorldPosition(width,height), Color.white, 100f);

            OnGridValueChanged += (object sender, OnGridValueChangedEventArgs eventArgs) =>{
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public int GetWidth(){
        return width;
    }

    public int GetHeight(){
        return height;
    }

    public Vector3 GetWorldPosition(int x, int y){
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public float GetCellSize(){
        return cellSize;
    }

    private void GetXY(Vector3 worldPosition, out int x, out int y){
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }
    
    public void SetGridObject(int x, int y, TGridObject value){
        if(x >= 0 && y >= 0 && x < width && y < height){
            gridArray[x, y] = value;
            if(OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
            // debugTextArray[x,y].text = gridArray[x, y].ToString();
        }
    }

    public void TriggerGridObjectChanged(int x, int y){
        if(OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value){
        int x,y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    // public void AddValue(int x, int y, TGridObject value){
    //     SetValue(x, y, GetValue(x, y) + value);
    // }

    public TGridObject GetGridObject(int x, int y){
        if(x >= 0 && y >= 0 && x < width && y < height){
            return gridArray[x, y];
        }else{
            return default;
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition){
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }

    // public void AddValue(Vector3 worldPosition, int value, int fullValueRange, int totalRange){
    //     int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullValueRange));

    //     GetXY(worldPosition, out int originX, out int originY);
    //     for(int x = 0; x < totalRange; x++){
    //         for(int y = 0; y < totalRange - x; y++){
    //             int radius = x + y;
    //             int addValueAmount = value;
    //             if(radius > fullValueRange){
    //                 addValueAmount -= lowerValueAmount * (radius - fullValueRange);
    //             }

    //             AddValue(originX + x, originY + y, addValueAmount);

    //             if(x != 0){
    //                 AddValue(originX - x, originY + y, addValueAmount);
    //             }
    //             if(y != 0){
    //                 AddValue(originX + x, originY - y, addValueAmount);
    //                 if(x != 0){
    //                     AddValue(originX - x, originY - y, addValueAmount);
    //                 }
    //             }
    //         }
    //     }
    // }
}
