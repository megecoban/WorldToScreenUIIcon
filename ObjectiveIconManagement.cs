using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class ObjectiveIconManagement : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private Canvas objectiveCanvas;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<GameObject> objects = new List<GameObject>();
    private List<GameObject> imageObj = new List<GameObject>();

    [Header("Settings")]
    [Header("Lerp Settings")]
    [SerializeField] private bool useLerp = true;
    [SerializeField][Range(1f, 600f)] private float lerpSpeed = 30f;

    [Header("Distance Settings")]
    [SerializeField] private bool useDistance = true;
    [SerializeField][Range(0.005f, 3f)] private float minDistance = 0.1f;
    [SerializeField][Range(3f, 10000f)] private float maxDistance = 10000f;

    [Header("Icon Settings")]
    [SerializeField] private Vector2 iconSize = new Vector2(50f, 50f);
    [SerializeField] private Color iconColorTint = new Color(1f, 1f, 1f, 1f);


    private void Start()
    {
        /*
         * foreach (Transform child in transform)
        {
            if (child.GetComponent<ObjectiveIcon>() != null)
            {
                objects.Add(child.gameObject);
            }
        }
         */

        for (int i=0; i<objects.Count; i++)
        {
            GameObject temp = new GameObject("Icon");
            temp.AddComponent<RectTransform>();
            temp.transform.parent = objectiveCanvas.transform;
            temp.AddComponent<Image>();
            temp.GetComponent<Image>().sprite = objects[i].GetComponent<ObjectiveIcon>().Icon;

            temp.GetComponent<RectTransform>().anchorMin = objectiveCanvas.transform.GetComponent<RectTransform>().anchorMin;
            temp.GetComponent<RectTransform>().anchorMax = objectiveCanvas.transform.GetComponent<RectTransform>().anchorMax;
            temp.GetComponent<RectTransform>().pivot = new Vector2(1, 0);


            imageObj.Add(temp);
        }
    }

    private void Update()
    {
        for(int i=0; i<objects.Count;i++)
        {
            if(i==0)
            {
                Debug.Log(imageObj[i].GetComponent<RectTransform>().anchorMin + " min | max "+
                    imageObj[i].GetComponent<RectTransform>().anchorMax + "\n" + imageObj[i].GetComponent<RectTransform>().pivot);
            }

            Debug.Log(objects[i].name);

            Vector3 screenPos = Vector3.zero;

            if (useDistance == false ||
                (
                Vector3.Distance(objects[i].transform.position, mainCamera.transform.position) >= minDistance 
                && Vector3.Distance(objects[i].transform.position, mainCamera.transform.position) <= maxDistance))
            {
                if(Vector3.Dot(mainCamera.transform.forward
                    , (objects[i].transform.position - mainCamera.transform.position).normalized) > 0f)
                {
                    screenPos = mainCamera.WorldToScreenPoint(objects[i].transform.position);
                }
                else
                {
                    screenPos = Vector3.zero;
                }
            }
            else
            {
                screenPos = Vector3.zero;
            }

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)objectiveCanvas.transform, (Vector2)screenPos, mainCamera, out Vector2 localPos))
            {
                Vector2 temp = (Vector2)screenPos + (Vector2.down * iconSize.y / 2) + (Vector2.right * iconSize.x / 2);
                Vector2 prev = imageObj[i].GetComponent<RectTransform>().anchoredPosition;

                imageObj[i].GetComponent<RectTransform>().anchoredPosition =

                    useLerp ?

                    Vector2.Lerp(prev
                    , ClampToCanvas(temp)
                    , Time.deltaTime * lerpSpeed)

                    :

                    ClampToCanvas(temp);
                    ;

                imageObj[i].GetComponent<RectTransform>().sizeDelta = iconSize;
                imageObj[i].GetComponent<Image>().color = iconColorTint;
            }
            else
            {
                Vector2 temp = (Vector2)screenPos + (Vector2.down * iconSize.y / 2) + (Vector2.right * iconSize.x / 2);
                Vector2 prev = imageObj[i].GetComponent<RectTransform>().anchoredPosition;

                imageObj[i].GetComponent<RectTransform>().anchoredPosition =

                    useLerp ?

                    Vector2.Lerp(prev
                    , ClampToCanvas(temp)
                    , Time.deltaTime * lerpSpeed)

                    :

                    ClampToCanvas(temp);
                ;

                imageObj[i].GetComponent<RectTransform>().sizeDelta = iconSize;
                imageObj[i].GetComponent<Image>().color = iconColorTint;
            }
        }
    }

    Vector2 ClampToCanvas(Vector2 point)
    {
        // Canvas'ýn boyutlarý
        float canvasWidth = objectiveCanvas.GetComponent<RectTransform>().rect.width;
        float canvasHeight = objectiveCanvas.GetComponent<RectTransform>().rect.height;

        // Ýstenen noktanýn Canvas içinde olmasý için sýnýrlarý kontrol et
        float clampedX = Mathf.Clamp(point.x, iconSize.x, canvasWidth);
        float clampedY = Mathf.Clamp(point.y, 0f, canvasHeight - iconSize.y);

        return new Vector2(clampedX, clampedY);
    }
}
