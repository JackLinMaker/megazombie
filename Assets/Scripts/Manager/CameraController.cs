using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    #region 公有变量

    public float XMargin = 0f;
    public float YMargin = 0f;
    public float Smooth = 4f;
    public float ResolutionX;
    public float ResolutionY;
    public GameObject CurrentMap;
    public bool IsControl { get; set; }
    public float Speed;

    //public Transform StartPoint;
    #endregion

    #region 私有变量

    public Transform player;


    private float width;
    private float height;
    private float cameraHalfWith;
    private float cameraHalfHeight;
    private Vector2 maxXAndY;
    private Vector2 minXAndY;
    private TweenAlpha mask;
    private Vector3 target;
    private bool bossMode;
    private GameObject backgrounds;

    #endregion

    #region 系统方法

    // Use this for initialization
    void Awake()
    {
        target = Vector3.zero;
        bossMode = false;
        IsControl = true;
        backgrounds = GameObject.Find("Backgrounds1-1");
        //player.position = StartPoint.position;
        //Camera.main.backgroundColor = DataManager.Instance.LevelBackgroundColors[int.Parse(Application.loadedLevelName.Substring(5, 1))];
        if (CurrentMap)
        {
            ResetCamera();
        }
    }

    void Start()
    {
    }

    void Update()
    {

    }
    // Update is called once per frame
    void LateUpdate()
    {

        if (IsControl)
        {
            // 地图比相机小 则不移动
            if ((maxXAndY.x - minXAndY.x) < (this.GetComponent<Camera>().orthographicSize * (ResolutionX / ResolutionY)) - 1.5)
            {
                return;
            }

            if (!GetComponent<CameraCanShake>().isSimpleShake && !GetComponent<CameraCanShake>().IsJumapShake)
            {
                // 获取默认位置
                float targetX = transform.position.x;
                float targetY = transform.position.y;

                // 如果超出了角色移动范围，则移动相机
                if (CheckXMargin())
                {
                    targetX = Mathf.Lerp(transform.position.x, player.position.x, Smooth * Time.deltaTime);
                }

                if (CheckYMargin())
                {
                    targetY = Mathf.Lerp(transform.position.y, player.position.y, Smooth * Time.deltaTime);
                }

                targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
                targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

                transform.position = new Vector3(targetX, targetY, transform.position.z);
            }
            else
            {
                // 获取默认位置
                float targetX = transform.position.x;
                // 如果超出了角色移动范围，则移动相机
                if (CheckXMargin())
                {
                    targetX = Mathf.Lerp(transform.position.x, player.position.x, Smooth * Time.deltaTime);
                }

                targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);

                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (bossMode)
            {
                if ((player.position.x <= (transform.position.x - this.GetComponent<Camera>().orthographicSize * ResolutionX / ResolutionY) && player.GetComponent<Player>().normalizedHorizontalSpeed < 0)
                    || (player.position.x >= (transform.position.x + this.GetComponent<Camera>().orthographicSize * ResolutionX / ResolutionY)) && player.GetComponent<Player>().normalizedHorizontalSpeed > 0)
                {
                    player.GetComponent<Player>().IsMovable = false;
                }
                else
                {
                    player.GetComponent<Player>().IsMovable = true;
                }

                // 获取默认位置
                float targetY = transform.position.y;
                // 如果超出了角色移动范围，则移动相机
                if (CheckYMargin())
                {
                    targetY = Mathf.Lerp(transform.position.y, player.position.y, Smooth * Time.deltaTime);
                }

                targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);

                transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
            }
        }

        backgrounds.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, backgrounds.transform.position.z);


    }

    #endregion

    #region 公有方法

    public void ResetCamera()
    {
        float factor = ResolutionX / ResolutionY;
        float width = CurrentMap.GetComponent<Tiled2Unity.TiledMap>().GetMapWidthInPixels() * 0.01f;
        float height = CurrentMap.GetComponent<Tiled2Unity.TiledMap>().GetMapHeightInPixels() * 0.01f;
        float cameraHalfWith = this.GetComponent<Camera>().orthographicSize * factor;
        float cameraHalfHeight = this.GetComponent<Camera>().orthographicSize;

        maxXAndY = new Vector2(width - cameraHalfWith, 0 - cameraHalfHeight) + new Vector2(CurrentMap.transform.position.x, CurrentMap.transform.position.y);
        minXAndY = new Vector2(cameraHalfWith, 0 - height + cameraHalfHeight) + new Vector2(CurrentMap.transform.position.x, CurrentMap.transform.position.y);
    }

    public void StartBossMode()
    {
        IsControl = false;
        bossMode = true;
    }

    public void StopBossMode()
    {
        IsControl = true;
        bossMode = false;

    }

    #endregion

    #region 私有方法

    private bool CheckXMargin()
    {
        return Mathf.Abs(transform.position.x - player.position.x) > XMargin;
    }

    private bool CheckYMargin()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > YMargin;
    }

    #endregion

    public void MoveToPosition(Vector3 pos)
    {
        Debug.Log("Camera MovetoPosition");
        target = pos;


    }


}
