﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SketchTool : Tool {
	IEntity p;
	IEntity u;
	IEntity v;

	protected override void OnMouseDown(Vector3 pos, ICADObject sko) {
		if(sko == null) return;
		if(p == null) {
			p = sko as IEntity;
		} else if(u == null) {
			u = sko as IEntity;
		} else if(v == null) {
			v = sko as IEntity;
			StopTool();
			var feature = new SketchFeature();
			DetailEditor.instance.AddFeature(feature); 
			feature.u = u;
			feature.v = v;
			feature.p = p;
			feature.source = DetailEditor.instance.activeFeature;

			IPlane plane = feature as IPlane;

			if(Vector3.Dot(plane.n, Camera.main.transform.forward) < 0f) {
				feature.u = v;
				feature.v = u;
			}

			DetailEditor.instance.ActivateFeature(feature);
			CameraController.instance.AnimateToPlane(feature);
		}
	}

	protected override void OnDeactivate() {
		u = null;
		v = null;
		p = null;
	}

	protected override void OnActivate() {
	}

	protected override string OnGetDescription() {
		return "click some point first and then click two non-parallel lines to define a new workplane. The point will be origin of the plane, and plane normal will be perpendicular to lines.";
	}

}
