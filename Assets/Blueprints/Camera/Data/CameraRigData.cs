// using Blueprints.DoD;
// using Edd.Cameras;
// using Collections;
// using Features.Camera.Deprecated;
//
// namespace Edd.Cameras
// {
//     public class CameraRigData
//     {
//         public string Id; // can be a number or a name; preferably human readable
//         public ICameraRig Rig; // what scene or object does this Rig belong to
//         public bool Active; // whether it's presently active or not
//     }
//
//     public class CameraRigModule 
//     {
//         private DataSet _data;
//         
//         public CameraRigModule(string id, ICameraRig rig, string owner)
//         {
//             Id = new ROData<string>(id);
//             Rig = new ROData<ICameraRig>(rig);
//             Active = new RWData<bool>(); // All cameras remain inactive until they are activated
//         }
//
//         public ROData<string> Id { get; }
//         public ROData<ICameraRig> Rig { get; }
//         public RWData<bool> Active { get; }
//
//         public DataSet Data =>
//             _data ??= new(
//                 new KeyValue<string, object>(nameof(Id), Id),
//                 new KeyValue<string, object>(nameof(Rig), Rig),
//                 new KeyValue<string, object>(nameof(Active), Active)
//             );
//     }
// }