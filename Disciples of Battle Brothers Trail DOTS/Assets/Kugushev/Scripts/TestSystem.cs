// using Unity.Entities;
// using Unity.Transforms;
//
// namespace Kugushev.Scripts
// {
//     public class TestSystem : SystemBase
//     {
//         protected override void OnUpdate()
//         {
//             var delta = Time.DeltaTime;
//
//             Entities.ForEach((ref Translation translation) =>
//                     translation = new Translation {Value = translation.Value + delta})
//                 .Schedule();
//         }
//     }
// }