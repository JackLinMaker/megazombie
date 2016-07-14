using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundScrolling : MonoBehaviour
{

    #region 公有变量

    public Transform[] BackgroundImages;

    public Transform BackgroundParent;

    public Transform Player;

    #endregion

    #region 私有变量

    private List<BackgroundImageInfo> imageList;
    private Transform playerAnimator;
    private int currentImageIndex;
    private float imageWith;

    #endregion

    #region 私有类

    private class BackgroundImageInfo
    {
        public int Index { get; set; }
        public Transform Image { get; set; }
    }

    #endregion

    #region 系统方法

    void Awake()
    {
        imageList = new List<BackgroundImageInfo>();
        playerAnimator = Player.FindChild("Animator");
    }

    // Use this for initialization
    void Start()
    {

        createBackgroundByIndex(0);
        createBackgroundByIndex(1);
    }

    // Update is called once per frame
    void Update()
    {
        currentImageIndex = checkCameraInImage().Index;
        

        // 相机向左移动
        if (Mathf.Sign(playerAnimator.localScale.x) == 1)
        {
            // 判断如果不存在下一张背景，则创建相机当前所在的图片的下一张
            if (isNeedCreateBackground(Mathf.Sign(playerAnimator.localScale.x))) createBackgroundByIndex(currentImageIndex + 1);

            // 实时移除当前背景后面的第二张图片
            removeBackgroundByIndex(currentImageIndex - 2);
        }
        else
        {
            // 判断如果不存在下一张背景，则创建相机当前所在的图片的下一张
            if (isNeedCreateBackground(Mathf.Sign(playerAnimator.localScale.x))) createBackgroundByIndex(currentImageIndex - 1);

            // 实时移除当前背景后面的第二张图片
            removeBackgroundByIndex(currentImageIndex + 2);
        }


    }

    #endregion

    #region 私有方法

    private Transform findImageByIndex(int index)
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            if (imageList[i].Index == index)
            {
                return imageList[i].Image;
            }
        }
        return null;
    }

    private void createBackgroundByIndex(int index)
    {
        Vector2 position = Vector2.zero;
        Transform image = Instantiate(BackgroundImages[index], position, Quaternion.identity) as Transform;
        image.GetComponent<Renderer>().sortingOrder = -1;
        image.parent = BackgroundParent;
        float with = image.GetComponent<Renderer>().bounds.size.x;
        Vector2 offset = new Vector2(index * with, 0);
        image.localPosition = position + offset;

        BackgroundImageInfo imageInfo = new BackgroundImageInfo() { Index = index, Image = image };
        imageList.Add(imageInfo);
    }

    private void removeBackgroundByIndex(int index)
    {
        BackgroundImageInfo deleteItem = new BackgroundImageInfo();
        foreach (BackgroundImageInfo item in imageList)
        {
            if (item.Index == index)
            {
                deleteItem = item;
                Destroy(item.Image.gameObject);
            }
        }

        imageList.Remove(deleteItem);
        //for (int i = 0; i < imageList.Count; i++)
        //{
        //    if (imageList[i].Index == currentImageIndex - 2)
        //    {
        //        imageList.Remove(imageList[i]);
        //        Destroy(imageList[i].Image.gameObject);
        //    }
        //}
    }

    private BackgroundImageInfo checkCameraInImage()
    {
        foreach (BackgroundImageInfo image in imageList)
        {
            if (transform.position.x > image.Image.GetComponent<Renderer>().bounds.min.x && transform.position.x < image.Image.GetComponent<Renderer>().bounds.max.x)
            {
                return image;
            }
        }
        return null;
    }

    private bool isNeedCreateBackground(float dir)
    {
        if (dir == 1)
        {
            foreach (BackgroundImageInfo item in imageList)
            {
                if (item.Index == currentImageIndex + 1)
                {
                    return false;
                }
            }
        }
        else 
        {
            foreach (BackgroundImageInfo item in imageList)
            {
                if (item.Index == currentImageIndex - 1)
                {
                    return false;
                }
            }
        }

        // 当前达到最后一张或者达到第一张
        if (currentImageIndex == BackgroundImages.Length - 1 || currentImageIndex == 0)
        {
            return false;
        }

        return true;
    }

    #endregion
}


