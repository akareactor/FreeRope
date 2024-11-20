using System.Collections.Generic;
using UnityEngine;
namespace KulibinSpace.FreeRope {

    public class RopeMeshRenderer : Rope {
        public int segmentCount = 4;
        [Header("Anchors")]
        // The joints are placed on a starting points of every segment
        public GameObject anchorStart; // Start anchor pos obviously coincides with a zero segment
        public GameObject anchorEnd; // End anchor pos coincides with a last segment ending point
        [Header("Segment")]
        public float radius = 0.5f;
        public int segments = 4;
        public Material material;

        private void Start () {
            if (anchorStart == null || anchorEnd == null) {
                Debug.LogError("Необходимо задать начальный и конечный объекты.");
                return;
            }
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
                // generate mesh between segments
                PrimitiveGenerator.GenerateCylinder(currentSegment, segments, 1, radius, segmentDirection.magnitude);
                // next segment
                previousSegment = currentSegment;
            }
            ConfigurableJoint endJoint = anchorEnd.AddComponent<ConfigurableJoint>();
            SetupJoint(endJoint);
            endJoint.connectedBody = previousSegment.GetComponent<Rigidbody>();
        }

    }

}
