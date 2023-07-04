using System;
using System.Collections.Generic;
using Blueprints.DoD;
using UnityEngine;
using UnityEngine.XR;

namespace Features.XR
{
    public enum TrackingMode : byte
    {
        NotSpecified,
        Device,
        Floor
    }

    public enum CameraState : byte
    {
        Inactive,
        Initializing,
        Failed,
        Initialized,
    }
    
    [Serializable]
    public class Origin 
    {
        private IDataSet _data;
        [SerializeField] private Data<Camera> camera;
        [SerializeField] private Data<float> offset;
        private Data<Transform> _trackablesParent;
        [SerializeField] private Data<TrackingMode> requestedTrackingMode;
        private Data<TrackingOriginModeFlags> _currentTrackingMode;
        private Data<CameraState> _cameraState;

        public Origin()
        {
            _trackablesParent = new Data<Transform>();
            _currentTrackingMode = new Data<TrackingOriginModeFlags>();
            _cameraState = new Data<CameraState>();
        }
        
        public IData<Camera> Camera => camera;
        public IData<float> Offset => offset;
        public IData<Transform> TrackablesParent => _trackablesParent;
        public IData<TrackingMode> RequestedTrackingMode => requestedTrackingMode;
        public IData<TrackingOriginModeFlags> CurrentTrackingMode => _currentTrackingMode;
        public IData<CameraState> CameraState => _cameraState;
        
        public IDataSet Data
        {
            get
            {
                return _data ??= new DataSet(
                    new KeyValuePair<string, object>(nameof(camera), Camera),
                    new KeyValuePair<string, object>(nameof(offset), Offset),
                    new KeyValuePair<string, object>(nameof(requestedTrackingMode), RequestedTrackingMode)
                );
            }
        }
        
    }
}