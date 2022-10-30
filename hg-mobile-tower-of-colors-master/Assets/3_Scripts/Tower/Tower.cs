using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tower : MonoBehaviour
{
    [Header("Tower Settings")]
    public float TileHeight = 1.2f;
    public float TileRadius = 0.5f;
    public int TileCountPerFloor = 15;
    public int FloorCount = 15;
    public int PlayableFloors = 8;
    public float SpecialTileChance = 0.1f;
    public TowerTile TilePrefab;
    public TowerTile[] SpecialTilePrefabs;
    public bool BuildOnStart = true;

    [Header("Scene")]
    public Transform CameraTarget;

    private List<List<TowerTile>> tilesByFloor;
    private int currentFloor = -1;
    private int maxFloor = 0;

    public System.Action<TowerTile> OnTileDestroyedCallback;

    private void Start()
    {
        if (BuildOnStart) {
            BuildTower();
        }
    }

    public float CaculateTowerRadius(float sideLength, float sideCount)
    {
        return sideLength / (2 * Mathf.Sin(Mathf.Deg2Rad * (180.0f / sideCount)));
    }

    public void BuildTower()
    {
        ResetTower();
        tilesByFloor = new List<List<TowerTile>>();
        float towerRadius = CaculateTowerRadius(TileRadius * 2, TileCountPerFloor);
        float angleStep = 360.0f / TileCountPerFloor;
        Quaternion floorRotation = transform.rotation;
        for (int y = 0; y < FloorCount; y++) {
            tilesByFloor.Add(new List<TowerTile>());
            for (int i = 0; i < TileCountPerFloor; i++) {
                Quaternion direction = Quaternion.AngleAxis(angleStep * i, Vector3.up) * floorRotation;
                Vector3 position = transform.position + Vector3.up * y * TileHeight + direction * Vector3.forward * towerRadius;
                TowerTile tileInstance = CreateTowerTile(direction, position,y);
                tilesByFloor[y].Add(tileInstance);
            }
            floorRotation *= Quaternion.AngleAxis(angleStep / 2.0f, Vector3.up);
        }
        maxFloor = FloorCount - 1;

        SetCurrentFloor(tilesByFloor.Count - PlayableFloors);
        for (int i = 1; i < PlayableFloors; i++) {
            SetFloorActive(currentFloor + i, true);
        }
    }

    public float CaculateTowerRadius()
    {
        return CaculateTowerRadius(TileRadius * 2, TileCountPerFloor);
    }
    private TowerTile CreateTowerTile(Quaternion direction, Vector3 position, int floor)
    {
        // if there is only explosive barrels in the array, if not we can do something more specific with an enum TypeOf (exploding, other Type Of Barrel)
        TowerTile tileInstance = Instantiate(AllowCreatingExplosiveTile(floor) ? SpecialTilePrefabs[Random.Range(0, SpecialTilePrefabs.Length)] : TilePrefab, position, direction * TilePrefab.transform.rotation, transform);
        tileInstance.SetColorIndex(Mathf.FloorToInt(Random.value * TileColorManager.Instance.ColorCount));
        tileInstance.SetFreezed(true);
        tileInstance.Floor = floor;
        tileInstance.OnTileDestroyed += OnTileDestroyedCallback;
        tileInstance.OnTileDestroyed += OnTileDestroyed;
        return tileInstance;
    }

    private bool AllowCreatingExplosiveTile(int floor)
    {
        if (RemoteConfig.BOOL_EXPLOSIVE_BARRELS_ENABLED && Random.value <= SpecialTileChance && floor >= RemoteConfig.INT_EXPLOSIVE_BARRELS_MIN_LEVEL)
        {
            return true;
        }
        return false;
    }
    
    public void OnTileDestroyed(TowerTile tile)
    {
        if (maxFloor > PlayableFloors - 1 && tilesByFloor != null) {
            float checkHeight = (maxFloor - 1) * TileHeight + TileHeight * 0.9f;
            float maxHeight = 0;
            foreach (List<TowerTile> floor in tilesByFloor) {
                foreach (TowerTile t in floor) {
                    if (t != null)
                        maxHeight = Mathf.Max(t.transform.position.y, maxHeight);
                }
            }
            if (maxHeight < checkHeight) {
                maxFloor--;
                if (currentFloor > 0) {
                    SetCurrentFloor(currentFloor - 1);
                }
            }
        }
    }

    public void ResetTower()
    {
        if (tilesByFloor != null) {
            foreach (List<TowerTile> tileList in tilesByFloor) {
                foreach (TowerTile tile in tileList) {
                    if (Application.isPlaying)
                        Destroy(tile.gameObject);
                    else
                        DestroyImmediate(tile.gameObject);
                }
                tileList.Clear();
            }
            tilesByFloor.Clear();
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameSequence());
    }

    IEnumerator StartGameSequence()
    {
        for (int i = 0; i < tilesByFloor.Count - PlayableFloors; i++) {
            yield return new WaitForSeconds(0.075f * Time.timeScale);
            SetFloorActive(i, false, false);
        }

        yield return null;
    }

    public void SetCurrentFloor(int floor)
    {
        currentFloor = floor;
        CameraTarget.position = transform.position + Vector3.up * floor * TileHeight;
        SetFloorActive(currentFloor, true);
    }

    public void SetFloorActive(int floor, bool value, bool setFreezed = true)
    {
        foreach (TowerTile tile in tilesByFloor[floor]) {
            if (tile && tile.isActiveAndEnabled) {
                tile.SetEnabled(value);
                if (setFreezed)
                    tile.SetFreezed(!value);
            }
        }
    }

    public TowerTile GetRandomTile()
    {
        List<int> listSelectedFloors = new List<int>();
        for (int i = currentFloor; i < maxFloor; i++)
        {
            listSelectedFloors.Add(i);
        }
        
        int selectedFloor = listSelectedFloors[Random.Range (0, listSelectedFloors.Count)];
        List<TowerTile> listSelectedByFloor = tilesByFloor[selectedFloor];

        return listSelectedByFloor[Random.Range (0, listSelectedByFloor.Count)];

    }
    
    
    public TowerTile GetMultiShootTile(Vector3 sourcePos)
    {
        TowerTile target = null;
        int halfFloor = maxFloor - currentFloor;
        List<int> listSelectedFloors = new List<int>();
        for (int i = currentFloor; i <= halfFloor; i++)
        {
            listSelectedFloors.Add(i);
        }

        float tmpRadius = CaculateTowerRadius();
        while (tmpRadius > 0f)
        {
            Vector3 tmpSourcePos = sourcePos;
            List<int> tempListSelectedFloors = new List<int>(listSelectedFloors);
            Vector3 dir = CameraTarget.position - tmpSourcePos;
            dir.y = 0;
            float dynamicAngle = Mathf.Atan(tmpRadius / Vector3.Distance(CameraTarget.position, tmpSourcePos));
            dynamicAngle *= Mathf.Rad2Deg;
            int sign = Random.Range(0, 2) * 2 - 1;
            dir = Quaternion.Euler(0, sign * dynamicAngle, 0) * dir;
            //Debug.DrawLine(tmpSourcePos, tmpSourcePos + dir, Color.red, 10f);

            if (TryGetTileOnColumn(ref tmpSourcePos, dir, tempListSelectedFloors, out target))
            {
                return target;
            }
            
            dir = Quaternion.Euler(0, -2 * sign * dynamicAngle, 0) * dir;
            //Debug.DrawLine(tmpSourcePos, tmpSourcePos + dir, Color.green, 10f);
            tempListSelectedFloors = new List<int>(listSelectedFloors);
            if (TryGetTileOnColumn(ref tmpSourcePos, dir, tempListSelectedFloors, out target))
            {
                return target;
            }
            tmpRadius -= TileRadius * 2f;
        }
        
        if (target == null)
        {
            target = GetRandomTile();
        }
        return target;
    }

    private bool TryGetTileOnColumn(ref Vector3 tmpSourcePos, Vector3 direction, List<int> listSelectedFloors, out TowerTile target)
    {
        while (listSelectedFloors.Count > 0)
        {
            GetRandomHeight(ref tmpSourcePos, listSelectedFloors);
            if (TryRaycastTile(tmpSourcePos, direction, out target))
            {
                return true;
            }
            //Debug.DrawLine(tmpSourcePos, tmpSourcePos + direction, Color.blue, 10f);
        }
        target = null;
        return false;
    }
    
    private void GetRandomHeight(ref Vector3 tmpSourcePos, List<int> listSelectedFloors)
    {
        int selectedFloor = listSelectedFloors[Random.Range (0, listSelectedFloors.Count)];
        tmpSourcePos.y = transform.position.y + selectedFloor * TileHeight + TileHeight/2f;
        listSelectedFloors.Remove(selectedFloor);
    }

    private bool TryRaycastTile(Vector3 source, Vector3 direction, out TowerTile target)
    {
        Ray ray = new Ray(source, direction);
        RaycastHit hit;
        if (Physics.SphereCast(ray, 0.15f, out hit, 100f, 1, QueryTriggerInteraction.Ignore))
        {
            target = hit.collider.GetComponent<TowerTile>();
            return true;
        }

        target = null;
        return false;
    }
}
