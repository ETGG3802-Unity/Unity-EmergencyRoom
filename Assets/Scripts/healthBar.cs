using UnityEngine;
using System.Collections;
using MyExtensions;

public class healthBar : MonoBehaviour {

	// Use this for initialization
public
	float health = 100;
	Vector2 size;
private
	RectTransform panel;
	float startWidth;
	void Start () {
		panel = GetComponent<RectTransform> ();
		size = panel.sizeDelta;
		startWidth = GetComponent<RectTransform>().GetWidth();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("space")) {
			GetComponent<RectTransform>().anchoredPosition -= new Vector2(5, 0);
			Vector2 tmp = GetComponent<RectTransform>().anchoredPosition;
			print (tmp);
			GetComponent<RectTransform>().SetWidth(GetComponent<RectTransform>().GetWidth() - 10);

			//GetComponent<RectTransform>().localPosition[0] -= 10;
			//GetComponent<RectTransform>().SetPositionOfPivot(new Vector2(tmp.x - 10, tmp.y));
		}
	}
}
