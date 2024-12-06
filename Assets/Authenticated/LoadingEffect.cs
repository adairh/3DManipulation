using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingEffect : MonoBehaviour
{

    public float Speed = 500;

    [Space(5)] 
    public Text StateText = null;
    public static bool Loading = false;

    private Image image = null;

    private void Awake()
    {
        image = this.GetComponent<Image>();
    }

    private void Update()
    {
        if (Loading)
        {
            // This is where we implement our rotating effect
            this.transform.Rotate(((Vector3.forward * this.Speed) * Time.deltaTime), Space.World);

            Color alpha = image.color;

            if (alpha.a < 1f)
            {
                // this is where I implement my fade out
                alpha.a = Mathf.Lerp(alpha.a, 1f, Time.deltaTime * 2);     
            }

            image.color = alpha;
        }
        else
        {
            Color alpha = image.color;
            if (alpha.a > 0f)
            {
                // our fading in
                alpha.a = Mathf.Lerp(alpha.a, 0f, Time.deltaTime * 2);  
            }

            image.color = alpha;
        }
    }

    public void ChangeText(string t = "", bool loading = false, float hide = 0.0f)
    {
        if (StateText != null)
        {
            StateText.text = t;
            Loading = loading;
            if (hide > 0.0f)
            {
                StopAllCoroutines();
                StartCoroutine(Hide(hide));
            }
        }
    }

    IEnumerator Hide(float t)
    {
        yield return new WaitForSeconds(t);
        StateText.text = "";
        Loading = false;
    }
}
