using System;

namespace AssetPacker.ComponentPackers {
    public interface ISingleComponentPacker {
        Type ComponentType { get; }
    }
}