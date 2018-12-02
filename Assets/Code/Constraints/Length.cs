﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class Length : ValueConstraint {

	public ExpVector p0exp { get { return GetPointInPlane(0, sketch.plane); } }
	public ExpVector p1exp { get { return GetPointInPlane(1, sketch.plane); } }

	public Length(Sketch sk) : base(sk) { }

	public Length(Sketch sk, IEntity e) : base(sk) {
		AddEntity(e);
		Satisfy();
	}

	public override IEnumerable<Exp> equations {
		get {
			yield return GetEntity(0).Length() - value;
		}
	}
	
	ExpVector GetPointInPlane(int i, IPlane plane) {
		return GetEntity(0).GetPointAtInPlane(i, plane);
	}

	protected override void OnDraw(LineCanvas canvas) {
		//Vector3 p0p = GetPointInPlane(0, null).Eval();
		//Vector3 p1p = GetPointInPlane(1, null).Eval();
		//drawPointsDistance(p0p, p1p, canvas, Camera.main, false, true, true, 0);
		var e = GetEntity(0);
		Vector3 p0 = e.PointOn(0.0).Eval();
		Vector3 p1 = e.PointOn(1.0).Eval();

		Vector3 t0 = e.TangentAt(0.0).Eval().normalized;
		Vector3 t1 = e.TangentAt(1.0).Eval().normalized;
		Vector3 n = e.plane.n.normalized;
		Vector3 perp0 = Vector3.Cross(t0, n);
		Vector3 perp1 = Vector3.Cross(t1, n);
		float pix = getPixelSize();
		canvas.DrawLine(p0, p0 + perp0 * 25f * pix);
		canvas.DrawLine(p1, p1 + perp1 * 25f * pix);

		e.DrawParamRange(canvas, 20f * pix, 0.0, 1.0, 0.05);
		pos = e.OffsetAt(0.5, 30f * pix).Eval();

		var ap0 = e.OffsetAt(0.0, 20f * pix).Eval();
		var ap1 = e.OffsetAt(1.0, 20f * pix).Eval();
		bool stroke = (ap0 - ap1).magnitude < (R_ARROW_W * 2f + 1f) * pix;
		drawArrow(canvas, ap0, -e.TangentAt(0.0).Eval(), stroke);
		drawArrow(canvas, ap1, e.TangentAt(1.0).Eval(), stroke);
	}

	protected override Matrix4x4 OnGetBasis() {
		//var p0pos = GetPointInPlane(0, null).Eval();
		//var p1pos = GetPointInPlane(1, null).Eval();
		//return getPointsDistanceBasis(p0pos, p1pos, sketch.plane);
		return Matrix4x4.identity;
	}

}
