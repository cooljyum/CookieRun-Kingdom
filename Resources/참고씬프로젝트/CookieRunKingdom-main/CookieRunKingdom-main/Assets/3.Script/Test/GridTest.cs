using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Diagnostics;

public class GridTest : MonoBehaviour
{
    [SerializeField] private GameObject go;
    [SerializeField] private Grid grid;
    private Vector3 pos;
    private Camera cam;

    [SerializeField] private int maxI = 100000000;

    private void Awake()
    {
        cam = Camera.main;

        test1();
        test2();
    }

    public void OnDraw(InputAction.CallbackContext value)
    {
        // 그리기
        if (value.performed)
        {
            Vector2 val = value.ReadValue<Vector2>();
            Vector3 mousePos = new Vector3(val.x, val.y, cam.nearClipPlane);
            Vector3 targetpos = cam.ScreenToWorldPoint(mousePos);
            targetpos.z = 0;

            Vector3Int gridPosition = grid.WorldToCell(targetpos);
            go.transform.position = grid.CellToWorld(gridPosition);

            pos = go.transform.position;
        }
    }

    public void OnClick(InputAction.CallbackContext value)
    {
        // 놓기
        if(value.performed)
        {
            // UnityEngine.Debug.Log("OnClick 중입니다.");
            GameObject g = Instantiate(go, pos, Quaternion.identity);
            UnityEngine.Debug.Log(grid.WorldToCell(g.transform.position).x + "  " + grid.WorldToCell(g.transform.position).y);
        }
    }

    public class Pos
    {
        public float x, y, z;

        public Pos(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class PosInt
    {
        public int x, y, z;

        public PosInt(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }


    private void test1()
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        bool s;
        Pos pos = new Pos(0, 0, 0);
        Pos pos1 = new Pos(0, 0, 0);
        for(int i = 0; i < maxI; i++)
        {
            s = pos.Equals(pos1);
        }
        stopwatch.Stop();
        UnityEngine.Debug.Log("Vector3  " + stopwatch.ElapsedMilliseconds);
    }

    private void test2()
    {
        Stopwatch stopwatch = new Stopwatch();

        stopwatch.Start();

        bool s;
        PosInt pos = new PosInt(0, 0, 0);
        PosInt pos1 = new PosInt(0, 0, 0);
        for (int i = 0; i < maxI; i++)
        {
            s = pos.Equals(pos1);
        }

        stopwatch.Stop();
        UnityEngine.Debug.Log("Vector3Int  " + stopwatch.ElapsedMilliseconds);
    }
}
