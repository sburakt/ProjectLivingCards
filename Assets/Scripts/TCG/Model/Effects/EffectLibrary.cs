using System;
using System.Collections.Generic;

namespace TCG.Model.Effects
{
    // string ids for now only used at the begining of the match anyway
    public static class EffectLibrary
    {
        private static readonly Dictionary<string, Func<int, Effect>> _registry = new()
        {
            { "ScavengerEffect", ownerInstanceId => new ScavengerEffect(ownerInstanceId) } 
        };

        public static Effect Create(string effectId, int ownerInstanceId)
        {
            if (_registry.TryGetValue(effectId, out var constructor))
                return constructor(ownerInstanceId);
            throw new Exception($"Unknown effect ID: {effectId}");
        }
    }
}