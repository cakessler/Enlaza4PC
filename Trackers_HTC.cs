using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace Valve.VR
{

    public class Trackers_HTC : MonoBehaviour
    {
        public enum EIndex
        {
            None = -1,
            Hmd = (int)OpenVR.k_unTrackedDeviceIndex_Hmd,
            Device1,
            Device2,
            Device3,
            Device4,
            Device5,
            Device6,
            Device7,
            Device8,
            Device9,
            Device10,
            Device11,
            Device12,
            Device13,
            Device14,
            Device15,
            Device16
        }
        public GameObject Head;

        private Transform This_Object;

        private EIndex index = EIndex.Hmd;
        [Tooltip("If not set, relative to parent")]
        public bool first_time = true;
        private SteamVR_Utils.RigidTransform init;
        public bool isValid { get; private set; }

        private void Start()
        {
            if (Head)
            {
                This_Object = Head.transform;
            }
        }

        private void OnNewPoses(TrackedDevicePose_t[] poses)
        {
            if (index == EIndex.None)
                return;

            var i = (int)index;

            isValid = false;
            if (poses.Length <= i)
                return;

            if (!poses[i].bDeviceIsConnected)
                return;

            if (!poses[i].bPoseIsValid)
                return;

            isValid = true;

            var pose = new SteamVR_Utils.RigidTransform(poses[i].mDeviceToAbsoluteTracking);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                first_time = true;
            }
            if (first_time)
            {
                init = pose;
                first_time = false;
            }
            if (This_Object)
            {
                This_Object.localRotation = Quaternion.Inverse(init.rot * Quaternion.Inverse(pose.rot)); //Like a 'substraction'
            }
        }

        SteamVR_Events.Action newPosesAction;

        Trackers_HTC()
        {
            newPosesAction = SteamVR_Events.NewPosesAction(OnNewPoses);
        }

        void OnEnable()
        {
            var render = SteamVR_Render.instance;
            if (render == null)
            {
                enabled = false;
                return;
            }

            newPosesAction.enabled = true;
        }

        void OnDisable()
        {
            newPosesAction.enabled = false;
            isValid = false;
        }

        public void SetDeviceIndex(int index)
        {
            if (System.Enum.IsDefined(typeof(EIndex), index))
                this.index = (EIndex)index;
        }

        public void Set_Vive_Tracker_Position(string index, GameObject position)
        {
            if (index == "1") { this.index = EIndex.Device1; }
            else if (index == "2") { this.index = EIndex.Device2; }
            else if (index == "3") { this.index = EIndex.Device3; }
            else if (index == "4") { this.index = EIndex.Device4; }
            else if (index == "5") { this.index = EIndex.Device5; }
            else if (index == "6") { this.index = EIndex.Device6; }
            else if (index == "7") { this.index = EIndex.Device7; }
            else if (index == "8") { this.index = EIndex.Device8; }
            else if (index == "9") { this.index = EIndex.Device9; }

            This_Object = position.transform;
            OnEnable();
        }
    }
}