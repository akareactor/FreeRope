using System.Collections.Generic;
using UnityEngine;
namespace KulibinSpace.FreeRope {

    public class RopeLineRenderer : Rope {
        public int segmentCount = 4; // число сегментов верёвки
        [Header("Anchors")]
        // The joints are placed on a starting points of every segment
        public GameObject anchorStart; // Start anchor pos obviously coincides with a zero segment
        public GameObject anchorEnd; // End anchor pos coincides with a last segment ending point
        [Header("Segment")]
        public float radius = 0.5f;
        public Material material;
        private LineRenderer lineRenderer;

        private void Start () {
            if (anchorStart == null || anchorEnd == null) {
                Debug.LogError("Необходимо задать начальный и конечный объекты.");
                return;
            }
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = segmentCount + 1;
            lineRenderer.startWidth = radius;
            lineRenderer.endWidth = radius;
            lineRenderer.material = material;
            CreateRope();
        }

        void CreateRope () {
            Vector3 startPosition = anchorStart.transform.position;
            Vector3 endPosition = anchorEnd.transform.position;
            Vector3 segmentDirection = (endPosition - startPosition) / segmentCount;
            GameObject previousSegment = anchorStart;
            for (int i = 0; i < segmentCount; i++) {
                Vector3 segmentPosition = startPosition + segmentDirection * i;
                // generate mesh on every segment
                GameObject currentSegment = new GameObject("RopeSegment " + i, typeof(MeshFilter), typeof(MeshRenderer));
                currentSegment.transform.rotation = Quaternion.LookRotation(segmentDirection);
                currentSegment.transform.SetParent(transform);
                currentSegment.transform.position = segmentPosition;
                currentSegment.GetComponent<MeshRenderer>().material = material;
                // joint
                ConfigurableJoint joint = currentSegment.AddComponent<ConfigurableJoint>();
                SetupJoint(joint);
                joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
                // next segment
                previousSegment = currentSegment;
            }
            ConfigurableJoint endJoint = anchorEnd.AddComponent<ConfigurableJoint>();
            SetupJoint(endJoint);
            endJoint.connectedBody = previousSegment.GetComponent<Rigidbody>();
        }

        void Update () {
            int i = 0;
            foreach (Transform segment in transform) {
                lineRenderer.SetPosition(i, segment.transform.position);
                i += 1;
            }
            lineRenderer.SetPosition(transform.childCount, anchorEnd.transform.position);
        }

    }

}
