using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleDungeon : MonoBehaviour
{
    [Header("Dungeon")]
    [Min(2)] public int roomCount = 8;
    [Min(3)] public int minRoomSize = 4;
    [Min(3)] public int maxRoomSize = 7;
    [Min(5)] public int roomSpacing = 8;
    [Min(10)] public int maxPlacementAttempts = 200;
    public bool generateOnStart = true;

    [Header("Kenney Models")]
    [Tooltip("template-floor.fbx 또는 바닥 프리팹")]
    public GameObject floorModel;
    [Tooltip("template-wall.fbx 또는 벽 프리팹")]
    public GameObject wallModel;
    [Tooltip("보물로 사용할 모델. 비어 있으면 Cube를 사용합니다.")]
    public GameObject treasureModel;
    [Min(0.1f)] public float tileSize = 2f;
    [Min(0.02f)] public float wallThickness = 0.2f;
    [Tooltip("FBX 벽의 기본 방향이 맞지 않을 때 90도 단위로 조정합니다.")]
    public float wallRotationOffset;
    public bool automaticallyFitModels = true;

    [Header("Objects")]
    public bool spawnMarkers = true;
    [Min(0)] public int enemiesPerNormalRoom = 2;

    private readonly Dictionary<Vector2Int, Room> rooms = new();    //방의 중심 좌표를 key로 사용하여 각 Room 정보를 저장 합니다.

    private readonly HashSet<Vector2Int> floors = new();            //방과 복도를 포함한 모단 바닥 좌표를 중복 없이 저장합니다.

    private readonly HashSet<Vector2Int> roomFloors = new();        //복도를 제외한 방 자체의 바닥 좌표만 저장 합니다.

    private Transform generatedRoot;                                //생성된 던전 오브젝트들의 부모 Transform을 저장합니다.

    private static readonly Vector2Int[] Directions =               //던전 탐색에 사용할 상화좌우 네 방향으로 배열을 저장
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        // R 키를 누르면 던전을 다시 생성한다.
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            Generate();
    }

    [ContextMenu("Generate Dungeon")]
    public void Generate()
    {
        ClearDungeon();
        CreateRooms();
        ConnectRooms();
        RenderFloors();
        RenderWalls();
    }

    private void CreateRooms()
    {
        // 실습 1: 원점에 시작 방을 만든다.

        // 실습 2: 기존 방을 기준으로 새 방을 반복 생성한다.
    }

    private bool TryAddRoom(Vector2Int center, int size, RoomType type)
    {
        //방 중심으로 방이 시작되는 상대 좌표를 계산
        int min = -size / 2;
        int max = min + size;

        //기본 방 주변 한칸의 여백까지 검사하여 방끼리 바로 붙지 않게 합니다.
        for(int x = min -1; x <= max; x++)      //x 범위보다 한칸
        {
            for(int y = min - 1; y <= max; y++) //Y 범위보다 한칸
            {
                if (roomFloors.Contains(center + new Vector2Int(x, y))) return false;
                //검사하려는 좌표가 기본 방 바닥에 포함되어 있으면 방 생성을 실패
            }
        }

        Room room = new Room(center, size, type);
        rooms.Add(center, room);                //방의 중심 좌표를   Key로 사용하여 Dictionary에 방을 추가합니다.

        for(int x = min; x <= max; x++)
        {
            for(int y = min; y < max; y++)
            {
                Vector2Int cell = center + new Vector2Int(x, y);            //방 중심에 상대 좌표를 더하여 실제 셀 좌표를 계산 합니다.
                floors.Add(cell);                                       //해당 위치를 전체 던전의 바닥 목록에 추가
                roomFloors.Add(cell);                                   //해당 위치를 방 전용 바닥 목록에도 추가
            }
        }
        // 실습 3: 새 방이 기존 바닥과 겹치는지 검사한다.

        // 실습 4: 겹치지 않으면 방과 바닥 좌표를 저장한다.

        return false;                       //정상적으로 방 생성을 했으므로 true를 반환한다.
    }

    private void ConnectRooms()
    {
        // 실습 5: 두 방의 중심 좌표를 복도 생성 함수에 전달한다.
    }

    private void CreateCorridor(Vector2Int start, Vector2Int end)
    {
        // 실습 6: X축과 Y축으로 이동하며 L자 복도를 만든다.
    }

    private void RenderFloors()
    {
        // 실습 7: 모든 바닥 좌표에 바닥 모델을 생성한다.
    }

    private void RenderWalls()
    {
        // 실습 8: 인접 바닥이 없는 방향에만 벽을 생성한다.
    }

    private Vector3 CellToWorld(Vector2Int cell)
    {
        // 격자 좌표를 Unity 월드 좌표로 변환한다.
        return transform.position + new Vector3(cell.x * tileSize, 0f, cell.y * tileSize);
    }

    private void ClearGenerated()
    {
        rooms.Clear();               //자정되어 있던 모든 방 정보를 삭제 합니다.
        floors.Clear();             //저장되어있던 모든 바닥과 복도 좌표를 삭제 합니다.
        roomFloors.Clear();         //저장되어있던 방 전용 바닥 좌표를 삭제 합니다.

        Transform oldRoot = transform.Find("Generated Dungeon");

        if (oldRoot == null) return;            //기존에 생성된 던전이 없다면 종료

        if (Application.isPlaying) Destroy(oldRoot.gameObject);
        else DestroyImmediate(oldRoot.gameObject);              //에디터 모드라면 즉시 삭제 할 수 있는 Dest
    }

    private void ClearDungeon()
    {
        rooms.Clear();
        floors.Clear();

        Transform oldRoot = transform.Find("Generated Dungeon");
        if (oldRoot != null)
            Destroy(oldRoot.gameObject);

        generatedRoot = new GameObject("Generated Dungeon").transform;
        generatedRoot.SetParent(transform, false);
    }

    private static List<Vector2Int> ShuffledDirections()
    {
        List<Vector2Int> result = new(Directions);      //원본 방향 배열을 수정하지 않도록 새로운 List 만든다.
        for(int i = result.Count - 1; i >= 0; i--)      //뒤쪽 원소부터 하나씩 랜덤 위치와 교환 하는 Fisher-Yates 방식
        {
            int index = Random.Range(0, i + 1);         //현재 범위 안에서 교환할 임의의 인덱스를 선택
            (result[i], result[index]) = (result[index], result[i]);            //두 방향의 위치를 서로 교환합니다.
        }

        return result;
    }

    private void WalkX(ref Vector2Int current, int target)
    {
        //현재 X좌표가 목표 X 좌표와 같아질 때까지 반복
        while(current.x != target)
        {
            floors.Add(current);
            current.x += current.x < target ? 1 : -1;           //목표가 오른쪽이면 +1, 왼쪽이면 -1씩 X 좌표를 이동
        }
    }
}
