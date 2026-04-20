using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectDetector : MonoBehaviour
{
    [Header("物理探测设置")]
    public float physicalRadius = 5f;        // 探测半径
    public float expansionTime = 1.5f;       // 扩散总时长
    public string targetTag = "Interactable"; // 目标标签
    public LayerMask targetLayer;            // 建议设置层级过滤以提高性能

    [Header("视觉反馈")]
    public GameObject effectObject;          // 挂载Shader材质的子物体
    public Color highlightColor = Color.yellow;
    public float highlightDuration = 2f;

    private Material effectMaterial;


    private int radiusID;


    // 用于记录正在被探测的物体，防止重复触发

    private bool isDetecting = false;

    //用来判断是否被触发过的物体；
    private HashSet<Collider2D> alreadyDetected = new HashSet<Collider2D>();

    


    void Start()
    {
        if (effectObject != null)
        {
            effectMaterial = effectObject.GetComponent<Renderer>().material;
            radiusID = Shader.PropertyToID("_Radius");
            effectObject.SetActive(false);
            effectMaterial.SetFloat(radiusID, 0f);
        }
    }

    void Update()
    {
        // 按下E键且当前没在探测
        if (Input.GetKeyDown(KeyCode.E) && !isDetecting)
        {
            StartCoroutine(PerformSyncedDetection());
        }
    }

    IEnumerator PerformSyncedDetection()
    {

        isDetecting = true;
        //清空探测过的物体状态
        alreadyDetected.Clear();
        //闪烁纹理，后期请替换掉吧
        effectObject.SetActive(true);


        //初始时间计时

        float elapsed = 0f;



        while (elapsed < expansionTime)
        {
            elapsed += Time.deltaTime;
            //随时间扩散进度增加
            float progress = elapsed / expansionTime; 

            //Shader视觉半径
            float shaderValue = progress * 0.5f;
            effectMaterial.SetFloat(radiusID, shaderValue);

            // 物理距离计算
            float currentPhysicalDistance = progress * physicalRadius;

            // 动态计算圆环内物体碰撞
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentPhysicalDistance);
            
            foreach (var hit in hits)
            {   
                if (hit!=null && !alreadyDetected.Contains(hit))
                {
                    alreadyDetected.Add(hit);
                    StartCoroutine(HighlightAndShowInfo(hit.gameObject));
                    
                }
            }

        }
        // 探测结束，稍微停留后隐藏圆环
        yield return new WaitForSeconds(0.1f);
        effectObject.SetActive(false);
        isDetecting = false;
    }

    IEnumerator HighlightAndShowInfo(GameObject target)
    {
        SignalData info = target.GetComponent<SignalData>();
        if (info == null) yield break;
        info.isBeingScanned = true;


        if (info.IsImportant())
        {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        float signalStrength = Mathf.Clamp01(1.0f - (distance / physicalRadius));
        if (RadarManager.Instance != null)
        {
            RadarManager.Instance.RegisterDetectedObject(target);
        }

        if (SignalerUIManager.Instance != null)
        {
            SignalerUIManager.Instance.SignalShowInfo(target.transform, info, highlightDuration);
        }
    
        }
        yield return new WaitForSeconds(highlightDuration);
    
        // 探测结束后移除雷达点
        if (RadarManager.Instance != null)
        {
            RadarManager.Instance.RemoveObject(target);
        }
    }
    // 可视化探测范围
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, physicalRadius);
    }
}
