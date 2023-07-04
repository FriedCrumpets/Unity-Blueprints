using System;
using System.Collections.Generic;
using Blueprints.DoD;
using UnityEngine;

namespace Features.XR
{
    [Serializable]
    public class Rig
    {
        private IDataSet _data;
        [SerializeField] private Data<Transform> @base;
        [SerializeField] private Data<Transform> offset;
        [SerializeField] private Data<Transform> head;
        [SerializeField] private Data<Transform> leftHand;
        [SerializeField] private Data<Transform> rightHand;

        public Rig() { }

        public Rig(
            Transform @base, Transform offset,
            Transform head, Transform leftHand, Transform rightHand
        )
        {
            this.@base = new Data<Transform>(@base);
            this.offset = new Data<Transform>(offset);
            this.head = new Data<Transform>(head);
            this.leftHand = new Data<Transform>(leftHand);
            this.rightHand = new Data<Transform>(rightHand);
        }
        
        public IData<Transform> Base => @base;
        public IData<Transform> Offset => offset;
        public IData<Transform> Head => head;
        public IData<Transform> LeftHand => leftHand;
        public IData<Transform> RightHand => rightHand;

        public IDataSet Data
        {
            get
            {
                return _data ??= new DataSet(
                    new KeyValuePair<string, object>(nameof(@base), Base),
                    new KeyValuePair<string, object>(nameof(offset), Offset),
                    new KeyValuePair<string, object>(nameof(head), Head),
                    new KeyValuePair<string, object>(nameof(leftHand), LeftHand),
                    new KeyValuePair<string, object>(nameof(rightHand), RightHand)
                );
            }
        }

        public static Rig Construct()
        {
            var @base = new GameObject("XRRig").transform;
            var offset = new GameObject("Offset") { transform = { parent = @base } }.transform;
            return new Rig(
               @base, offset,
               new GameObject("Head") { transform = { parent = offset }}.transform,
               new GameObject("LeftHand") { transform = { parent = offset }}.transform,
               new GameObject("RightHand") { transform = { parent = offset }}.transform
            );
        }
    }
}