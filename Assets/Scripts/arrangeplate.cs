using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class arrangeplate : MonoBehaviour
{
    public float rotOffset = 11.5f;
    public int PlatesCount;
    public Vector3 localscale;
    public float x;
    private void OnValiate()
    {
        if (PlatesCount > 30)
            PlatesCount = 30;
        x =  30f/(float)PlatesCount ;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i>=PlatesCount)
            transform.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < PlatesCount; i++)
        {
            var item = transform.GetChild(i);
            item.localScale = new Vector3(x, 1f, 1f);
            item.localRotation = Quaternion.Euler(0f, 0f, rotOffset * i*item.localScale.x);
            item.gameObject.SetActive(true);
        }
    }
}
