﻿#if FIXED_POINT_MATH
using ME.ECS.Mathematics;
using tfloat = sfloat;
#else
using Unity.Mathematics;
using tfloat = System.Single;
#endif
using ME.ECS;
using Unity.Jobs;
using Unity.Burst;
using ME.ECS.Buffers;
using Unity.Collections;

namespace ME.ECS.Essentials.Destroy.Systems {

    #pragma warning disable
    using Components; using Modules; using Systems; using Markers;
    #pragma warning restore
    
    #if ECS_COMPILE_IL2CPP_OPTIONS
    [Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.NullChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.ArrayBoundsChecks, false),
     Unity.IL2CPP.CompilerServices.Il2CppSetOptionAttribute(Unity.IL2CPP.CompilerServices.Option.DivideByZeroChecks, false)]
    #endif
    public sealed class DestroyByTimeSystem : ISystem, IAdvanceTick {
        
        private Filter filter;
        
        public World world { get; set; }

        void ISystemBase.OnConstruct() {
            
            Filter.Create("Filter-DestroyByTimeSystem")
                                     .With<DestroyByTime>()
                                     .Push(ref this.filter);
                                     
        }
        
        void ISystemBase.OnDeconstruct() {}
        
        void IAdvanceTick.AdvanceTick(in float deltaTime) {

            foreach (var entity in this.filter) {
                
                ref var timer = ref entity.Get<DestroyByTime>();
                timer.time -= deltaTime;
                if (timer.time <= 0f) {
                
                    entity.Destroy();
                
                }
                
            }

        }

    }
    
}