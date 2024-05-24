﻿using ScreepsDotNet.Interop;
using ScreepsDotNet.API.Arena;

namespace ScreepsDotNet.Native.Arena
{
    [System.Runtime.Versioning.SupportedOSPlatform("wasi")]
    internal partial class NativeStructureExtension : NativeOwnedStructure, IStructureExtension
    {
        private NativeStore? storeCache;

        public IStore Store => CachePerTick(ref storeCache) ??= new NativeStore(proxyObject.GetPropertyAsJSObject(Names.Store));

        public NativeStructureExtension(INativeRoot nativeRoot, JSObject proxyObject)
            : base(nativeRoot, proxyObject)
        { }

        protected override void ClearNativeCache()
        {
            base.ClearNativeCache();
            storeCache = null;
        }

        public override string ToString()
            => Exists ? $"StructureExtension({Id}, {Position})" : "StructureExtension(DEAD)";
    }
}
