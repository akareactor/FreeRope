using UnityEngine;

public class RopeSegmentPrefab : Rope {
    public int segmentCount = 4;
    [Header("Anchors")]
    // The joints are placed on a starting points of every segment
    public GameObject anchorStart; // Start anchor pos obviously coincides with a zero segment
    public GameObject anchorEnd; // End anchor pos coincides with a last segment ending point
    [Header("Segment")]
    // kind of rope surface
    public GameObject segmentPrefab;

    private void Start () {
        if (segmentPrefab == null || anchorStart == null || anchorEnd == null) {
            Debug.LogError("Необходимо задать префаб сегмента и начальный и конечный объекты.");
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
            GameObject currentSegment = Instantiate(segmentPrefab, segmentPosition, Quaternion.LookRotation(segmentDirection), transform);
            ConfigurableJoint joint = currentSegment.AddComponent<ConfigurableJoint>();
            SetupJoint(joint);
            joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
            previousSegment = currentSegment;
        }
        ConfigurableJoint endJoint = anchorEnd.AddComponent<ConfigurableJoint>();
        SetupJoint(endJoint);
        endJoint.connectedBody = previousSegment.GetComponent<Rigidbody>();
    }


}
