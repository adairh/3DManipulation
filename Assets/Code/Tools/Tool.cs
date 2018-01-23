﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Tool : MonoBehaviour {

	[HideInInspector] public ToolBar toolbar;
	public KeyCode[] hotkeys;

	public bool shouldStop { get; private set; }

	void Start() {
		GetComponent<Button>().onClick.AddListener(Click);
	}

	void Click() {
		toolbar.ActiveTool = this;
	}

	protected virtual void OnActivate() { }
	protected virtual void OnDeactivate() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnMouseDown(Vector3 pos, ICADObject sko) { }
	protected virtual void OnMouseUp(Vector3 pos, ICADObject sko) { }
	protected virtual void OnMouseMove(Vector3 pos, ICADObject sko) { }
	protected virtual void OnMouseDoubleClick(Vector3 pos, ICADObject sko) { }

	public void Activate() {
		shouldStop = false;
		OnActivate();
	}

	public void Deactivate() {
		OnDeactivate();
	}

	public void DoUpdate() {
		OnUpdate();
	}

	public void MouseDown(Vector3 pos, ICADObject sko) {
		OnMouseDown(pos, sko);
	}

	public void MouseUp(Vector3 pos, ICADObject sko) {
		OnMouseUp(pos, sko);
	}

	public void MouseMove(Vector3 pos, ICADObject sko) {
		OnMouseMove(pos, sko);
	}

	public void MouseDoubleClick(Vector3 pos, ICADObject sko) {
		OnMouseDoubleClick(pos, sko);
	}

	public bool IsActive() {
		return toolbar.ActiveTool == this;
	}

	public static Vector3 MousePos {
		get {
			var sk = DetailEditor.instance.currentSketch;
			var pos = WorldPlanePos;
			if(sk != null) {
				pos = sk.WorldToLocal(pos);
			}
			return pos;
		}
	}

	public static Vector3 WorldMousePos {
		get {
			var mousePos = Input.mousePosition;
#if UNITY_WEBGL
			if(Input.touches.Length > 0) mousePos = Input.touches[0].position;
#endif
			var plane = new Plane(Camera.main.transform.forward, Vector3.zero);
			var ray = Camera.main.ScreenPointToRay(mousePos);
			float cast;
			plane.Raycast(ray, out cast);
			return ray.GetPoint(cast);
		}
	}

	public static Vector3 WorldPlanePos {
		get {
			var mousePos = Input.mousePosition;
#if UNITY_WEBGL
			if(Input.touches.Length > 0) mousePos = Input.touches[0].position;
#endif
			var plane = new Plane(Camera.main.transform.forward, Vector3.zero);
			var sk = DetailEditor.instance.currentSketch;
			if(sk != null) {
				plane = new Plane(sk.GetNormal(), sk.GetPosition());
			}
			var ray = Camera.main.ScreenPointToRay(mousePos);
			float cast;
			plane.Raycast(ray, out cast);
			return ray.GetPoint(cast);
		}
	}


	public static Vector3 CenterPos {
		get {
			var plane = new Plane(Camera.main.transform.forward, Vector3.zero);
			var ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
			float cast;
			plane.Raycast(ray, out cast);
			return ray.GetPoint(cast);
		}
	}

	public void StopTool() {
		shouldStop = true;
	}

	protected bool AutoConstrainCoincident(PointEntity point, Entity with) {
		if(with is PointEntity) {
			var p1 = with as PointEntity;
			new PointsCoincident(point.sketch, point, p1);
			point.SetPosition(p1.GetPosition());
			return true;
		}
		return false;
	}

	public string GetDescription() {
		var result = this.GetType().Name;
		if(hotkeys.Length > 0) {
			result += " [" + hotkeys[0].ToString() + "]";
		}
		var desc = OnGetDescription();
		if(desc != "") {
			result += ": " + desc;
		}
		return result;
	}

	public string GetTooltip() {
		var result = this.GetType().Name + ". " + OnGetDescription();
		if(hotkeys.Length > 0) {
			result += "[" + hotkeys[0].ToString() + "]";
		}
		return result;
	}

	protected virtual string OnGetDescription() {
		return "";
	}

	protected virtual string OnGetTooltip() {
		return "";
	}

}
