using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour 
{
	public GameObject popup;
	public List<Button> buttons = new List<Button>();

	private int[,] btnPos =
	{
		{0, 0},
		{0, 1},
		{0, 2},

		{1, 1},
		{1, 0},
		{1, 2},

		{2, 1},
		{2, 2},
		{2, 0},

		{4, 1},
		{4, 2},
		{4, 0},

		{3, 1},
		{3, 2},
		{3, 0},

		{5, 1},
		{5, 2},
		{5, 0},
	};

	private int selectedIndex = 0;
	private Vector3 prevVector = Vector3.zero;
	private float currentCooldown = 0;
	private float swipeCooldown = 0.02f;

	void Update () 
	{
		int buttonCount = buttons.Count;
		for(int i = 0; i < buttonCount; ++i)
		{
			if (i == selectedIndex)
			{
				buttons[i].Select();
			}
		}

		if (TalonConnect.TalonRing == null)
			return;

		TalonSDK.TalonRing ring = TalonConnect.TalonRing;

		if (ring.BottomButtonDown)
		{
			popup.SetActive(!popup.activeSelf);
		}

		if (popup.activeSelf)
			return;
		
		Vector3 forward = ring.Orientation * Vector3.forward;

		if (prevVector != Vector3.zero && !ring.Recentered)
		{
			if (currentCooldown > 0)
			{
				currentCooldown -= Time.deltaTime;
			}
			else
			{
				Vector3 speed = (forward - prevVector) / Time.deltaTime;

				float Xmag = Mathf.Abs(speed.x);
				float Ymag = Mathf.Abs(speed.y);

				if (Xmag > 2.5 || Ymag > 2.5) 
				{
					currentCooldown = swipeCooldown;
					if (Xmag > Ymag) {
						if (speed.x < 0) {
							OnLeft();
						} else {
							OnRight();
						}
					} else {
						if (speed.z < 0) {
							OnUp();
						} else {
							OnDown();
						}
					}
				}
			}
		}

		prevVector = forward;
	}

	private void OnLeft()
	{
		Debug.Log("OnLeft");
		int x = btnPos[selectedIndex, 0];
		int y = btnPos[selectedIndex, 1];
		if (x > 0) x--;
		int idx = getIndex(x, y);
		if (idx != -1) selectedIndex = idx;
	}

	private void OnRight()
	{
		Debug.Log("OnRight");
		int x = btnPos[selectedIndex, 0];
		int y = btnPos[selectedIndex, 1];
		if (x < 5) x++;
		int idx = getIndex(x, y);
		if (idx != -1) selectedIndex = idx;
	}

	private void OnUp()
	{
		Debug.Log("OnUp");
		int x = btnPos[selectedIndex, 0];
		int y = btnPos[selectedIndex, 1];
		if (y > 0) y--;
		int idx = getIndex(x, y);
		if (idx != -1) selectedIndex = idx;
	}

	private void OnDown()
	{
		Debug.Log("OnDown");
		int x = btnPos[selectedIndex, 0];
		int y = btnPos[selectedIndex, 1];
		if (y < 2) y++;
		int idx = getIndex(x, y);
		if (idx != -1) selectedIndex = idx;
	}

	private int getIndex(int x, int y)
	{
		int buttonCount = buttons.Count;
		for(int i = 0; i < buttonCount; ++i)
		{
			if (x == btnPos[i, 0] && y == btnPos[i, 1])
				return i;
		}
		return -1;
	}
}
