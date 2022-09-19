using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace FemDesign.Grasshopper
{
    public static class GeomUtility
    {
		internal static bool CurveSegments(List<Curve> list, Curve curve, bool recursive)
		{
			if (curve == null)
			{
				return false;
			}
			PolyCurve polyCurve = curve as PolyCurve;
			if (polyCurve != null)
			{
				if (recursive)
				{
					polyCurve.RemoveNesting();
				}
				Curve[] array = polyCurve.Explode();
				if (array == null)
				{
					return false;
				}
				if (array.Length == 0)
				{
					return false;
				}
				if (recursive)
				{
					Curve[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						Curve curve2 = array2[i];
						CurveSegments(list, curve2, recursive);
					}
				}
				else
				{
					Curve[] array3 = array;
					for (int j = 0; j < array3.Length; j++)
					{
						Curve item = array3[j];
						list.Add(item);
					}
				}
				return true;
			}
			else
			{
				PolylineCurve polylineCurve = curve as PolylineCurve;
				if (polylineCurve != null)
				{
					for (int k = 0; k < polylineCurve.PointCount - 1; k++)
					{
						list.Add(new LineCurve(polylineCurve.Point(k), polylineCurve.Point(k + 1)));
					}
					return true;
				}
				Polyline polyline;
				if (curve.TryGetPolyline(out polyline))
				{
					for (int l = 0; l < polyline.Count - 1; l++)
					{
						list.Add(new LineCurve(polyline[l], polyline[l + 1]));
					}
					return true;
				}
				LineCurve lineCurve = curve as LineCurve;
				if (lineCurve != null)
				{
					list.Add(lineCurve.DuplicateCurve());
					return true;
				}
				ArcCurve arcCurve = curve as ArcCurve;
				if (arcCurve != null)
				{
					list.Add(arcCurve.DuplicateCurve());
					return true;
				}
				return CurveSegments(list, curve.ToNurbsCurve());
			}
		}
		private static bool CurveSegments(List<Curve> list, NurbsCurve nurbs)
        {
            int count = list.Count;
            if (nurbs == null)
            {
                return false;
            }
            double num = nurbs.Domain.Min;
            double max = nurbs.Domain.Max;
            //double num2;
            while (nurbs.GetNextDiscontinuity(Continuity.C1_locus_continuous, num, max, out double num2))
            {
                Interval interval = new Interval(num, num2);
                num = num2;
                if (interval.Length >= 1E-16)
                {
                    Curve curve = nurbs.Trim(interval);
                    if (curve.IsValid)
                    {
                        list.Add(curve);
                    }
                }
            }
            Interval interval2 = new Interval(num, max);
            if (interval2.Length > 1E-16)
            {
                Curve curve2 = nurbs.Trim(interval2);
                if (curve2.IsValid)
                {
                    list.Add(curve2);
                }
            }
            if (list.Count == count)
            {
                list.Add(nurbs);
            }
            return true;
        }
        internal static List<Curve> Explode(Curve curve, bool recursive = false)
        {
			List<Curve> list = new List<Curve>();
			CurveSegments(list, curve, recursive);
            return list;
        }

    }
}
