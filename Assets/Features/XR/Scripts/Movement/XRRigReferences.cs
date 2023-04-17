using System;
using UnityEngine;
using Utils;

namespace Features.XR
{
    [Serializable]
    public class XRRigReferences
    {
        public const string UNTAGGED = "Untagged";
        public const string ORIGIN_BASE_TAG = "XRBase";
        public const string HEAD_TAG = "XRHead";
        public const string LEFT_HAND_TAG = "XRLeftHand";
        public const string RIGHT_HAND_TAG = "XRRightHand";
        public const string CANVAS_PIVOT_TAG = "XRCanvasPivot";
        
        [SerializeField] public Transform originBase;
        [SerializeField] public Transform head;
        [SerializeField] public Transform leftHand;
        [SerializeField] public Transform rightHand;
        [SerializeField] public Transform canvasPivot;
        
        public Transform OriginBase 
        {
            get
            {
                originBase = LocateXRReference(originBase, ORIGIN_BASE_TAG);

                return originBase;
            }
            private set
            {
                if(originBase != null)
                {
                    originBase.gameObject.tag = UNTAGGED;
                }
                
                originBase = LocateXRReference(value, ORIGIN_BASE_TAG);
            }
        }
        public Transform Head 
        { 
            get
            {
                head = LocateXRReference(head, HEAD_TAG);

                return head;
            }
            private set
            {
                if(head != null)
                {
                    head.gameObject.tag = UNTAGGED;
                }
                
                head = LocateXRReference(value, HEAD_TAG);
            }
        }
        public Transform LeftHand 
        { 
            get
            {
                leftHand = LocateXRReference(leftHand, LEFT_HAND_TAG);

                return leftHand;
            }
            private set
            {
                if(leftHand != null)
                {
                    leftHand.gameObject.tag = UNTAGGED;
                }
                
                leftHand = LocateXRReference(value, LEFT_HAND_TAG);
            }
        }
        public Transform RightHand 
        { 
            get
            {
                rightHand = LocateXRReference(rightHand, RIGHT_HAND_TAG);

                return rightHand;
            }
            private set
            {
                if(rightHand != null)
                {
                    rightHand.gameObject.tag = UNTAGGED;
                }
                
                rightHand = LocateXRReference(value, RIGHT_HAND_TAG);
            }
        }

        public Transform CanvasPivot
        {
            get
            {
                canvasPivot = LocateXRReference(canvasPivot, CANVAS_PIVOT_TAG);

                return canvasPivot;
            }
            private set
            {
                if(canvasPivot != null)
                {
                    canvasPivot.gameObject.tag = UNTAGGED;
                }
                
                canvasPivot = LocateXRReference(value, CANVAS_PIVOT_TAG);
            }
        }

        public void Initialise() => TryFindReferences();
        
        private void TryFindReferences()
        {
            OriginBase = LocateXRReference(OriginBase, ORIGIN_BASE_TAG);
            Head = LocateXRReference(Head, HEAD_TAG);
            LeftHand = LocateXRReference(LeftHand, LEFT_HAND_TAG);
            RightHand = LocateXRReference(RightHand, RIGHT_HAND_TAG);
            CanvasPivot = LocateXRReference(CanvasPivot, CANVAS_PIVOT_TAG);
        }

        private Transform LocateXRReference(Transform transform, string gameObjectTag)
        {
            if (Utilities.NullCheck(transform))
            {
                transform = TryFindTransformWithTag(gameObjectTag);
            }
            else
            {
                transform.gameObject.tag = gameObjectTag;
            }

            return transform;
        }

        private static Transform TryFindTransformWithTag(string tag)
        {
            var transform = GameObject.FindGameObjectWithTag(tag).transform;

            if (transform == null)
            {
                throw new NullReferenceException($"GameObject with tag['{tag}'] could not be found");
            }

            return transform;
        }
    }
}