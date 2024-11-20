using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// заготовка из жабы

public class RopeJointsSkinned : MonoBehaviour {
    public GameObject startPoint;
    public GameObject endPoint;
    public int segmentCount = 10;
    public float segmentLength = 0.5f;
    public GameObject segmentPrefab;

    private GameObject[] segments;

    void Start () {
        CreateRope();
    }

    void CreateRope () {
        segments = new GameObject[segmentCount];
        Vector3 segmentDirection = (endPoint.transform.position - startPoint.transform.position).normalized;

        Vector3 currentPos = startPoint.transform.position;

        for (int i = 0; i < segmentCount; i++) {
            GameObject segment = Instantiate(segmentPrefab, currentPos, Quaternion.identity, transform);
            Rigidbody rb = segment.GetComponent<Rigidbody>();
            if (i > 0) {
                HingeJoint joint = segment.AddComponent<HingeJoint>();
                joint.connectedBody = segments[i - 1].GetComponent<Rigidbody>();
                joint.anchor = new Vector3(0, -segmentLength / 2, 0);
                joint.connectedAnchor = new Vector3(0, segmentLength / 2, 0);
            } else {
                HingeJoint joint = segment.AddComponent<HingeJoint>();
                joint.connectedBody = startPoint.GetComponent<Rigidbody>();
                joint.anchor = new Vector3(0, -segmentLength / 2, 0);
                joint.connectedAnchor = new Vector3(0, 0, 0);
            }

            segments[i] = segment;
            currentPos += segmentDirection * segmentLength;
        }

        // Присоединяем последний сегмент к конечной точке
        HingeJoint endJoint = segments[segmentCount - 1].AddComponent<HingeJoint>();
        endJoint.connectedBody = endPoint.GetComponent<Rigidbody>();
        endJoint.anchor = new Vector3(0, segmentLength / 2, 0);
        endJoint.connectedAnchor = new Vector3(0, 0, 0);

        // Добавляем SkinnedMeshRenderer
        SkinnedMeshRenderer renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
        Mesh mesh = new Mesh();
        // Конфигурируем mesh (здесь нужно создать mesh с костями)
        renderer.sharedMesh = mesh;
        // Настройка костей и скининга для рендеринга веревки
    }
}
