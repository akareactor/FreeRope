using UnityEngine;

public class Rope : MonoBehaviour {

    [Header("Rope links params")]
    // Global link parameters
    public float LinkMass = 1.0f;
    public int LinkSolverIterationCount = 200;
    public float LinkJointAngularXLimit = 120.0f;
    public float LinkJointAngularYLimit = 120.0f;
    public float LinkJointAngularZLimit = 120.0f;
    public float LinkJointSpringValue = 20.0f;
    public float LinkJointDamperValue = 0.0f;
    public float LinkJointMaxForceValue = 20.0f;
    public float LinkJointBreakForce = Mathf.Infinity;
    public float LinkJointBreakTorque = Mathf.Infinity;
    public bool LockStartEndInZAxis = false;

    protected void SetupJoint (ConfigurableJoint joint) {
        SoftJointLimit jointLimit = new SoftJointLimit {
            contactDistance = 0.0f,
            bounciness = 0.0f
        };
        JointDrive jointDrive = new JointDrive {
            positionSpring = LinkJointSpringValue,
            positionDamper = LinkJointDamperValue,
            maximumForce = LinkJointMaxForceValue
        };
        joint.axis = Vector3.right;
        joint.secondaryAxis = Vector3.up;
        joint.breakForce = LinkJointBreakForce;
        joint.breakTorque = LinkJointBreakTorque;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = Mathf.Approximately(LinkJointAngularXLimit, 0.0f) == false ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
        joint.angularYMotion = Mathf.Approximately(LinkJointAngularYLimit, 0.0f) == false ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
        joint.angularZMotion = Mathf.Approximately(LinkJointAngularZLimit, 0.0f) == false ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
        jointLimit.limit = -LinkJointAngularXLimit;
        joint.lowAngularXLimit = jointLimit;
        jointLimit.limit = LinkJointAngularXLimit;
        joint.highAngularXLimit = jointLimit;
        jointLimit.limit = LinkJointAngularYLimit;
        joint.angularYLimit = jointLimit;
        jointLimit.limit = LinkJointAngularZLimit;
        joint.angularZLimit = jointLimit;
        joint.angularXDrive = jointDrive;
        joint.angularYZDrive = jointDrive;
    }


}
