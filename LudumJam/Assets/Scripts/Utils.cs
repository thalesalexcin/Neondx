using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Util
{
    public static class Utils
    {
        public static Vector3 GetWorldPositionOnPlane(Camera camera, Vector3 screenPosition, float z)
        {
            Ray ray = camera.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }
    }
}
