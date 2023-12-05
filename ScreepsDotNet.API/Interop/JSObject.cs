﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ScreepsDotNet.Interop
{
    public enum JSPropertyType
    {
        Undefined,
        String,
        Number,
        BigInt,
        Boolean,
        Object,
        Function,
        Symbol,
    }

    [System.Runtime.Versioning.SupportedOSPlatform("wasi")]
    public partial class JSObject : IDisposable, IEquatable<JSObject?>
    {
        #region Imports

        [Interop.JSImport("object", "hasProperty")]
        private static partial bool HasPropertyOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getTypeOfProperty")]
        private static partial int GetTypeOfPropertyOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getKeys")]
        private static partial ImmutableArray<string> GetKeysOnObject(JSObject obj);

        [Interop.JSImport("object", "getProperty")]
        private static partial bool GetPropertyAsBooleanOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getProperty")]
        private static partial int GetPropertyAsInt32OnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getProperty")]
        private static partial double GetPropertyAsDoubleOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getProperty")]
        private static partial string GetPropertyAsStringOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getProperty")]
        private static partial JSObject? GetPropertyAsJSObjectOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "getProperty")]
        private static partial byte[]? GetPropertyAsByteArrayOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "setProperty")]
        private static partial void SetPropertyOnObject(JSObject obj, string key, bool value);

        [Interop.JSImport("object", "setProperty")]
        private static partial void SetPropertyOnObject(JSObject obj, string key, int value);

        [Interop.JSImport("object", "setProperty")]
        private static partial void SetPropertyOnObject(JSObject obj, string key, double value);

        [Interop.JSImport("object", "setProperty")]
        private static partial void SetPropertyOnObject(JSObject obj, string key, string? value);

        [Interop.JSImport("object", "setProperty")]
        private static partial void SetPropertyOnObject(JSObject obj, string key, JSObject? value);

        [Interop.JSImport("object", "setProperty")]
        private static partial void SetPropertyOnObject(JSObject obj, string key, byte[]? value);

        [Interop.JSImport("object", "deleteProperty")]
        private static partial void DeletePropertyOnObject(JSObject obj, string key);

        [Interop.JSImport("object", "create")]
        private static partial JSObject CreateObject(JSObject? prototypeObj = null);

        #endregion

        #region Static Interface

        public static JSObject Create(JSObject? prototypeObj = null) => CreateObject(prototypeObj);

        #endregion

        private readonly IntPtr jsHandle;
        private bool disposedValue;

        internal IntPtr JSHandle => jsHandle;

        internal JSObject(IntPtr jsHandle)
        {
            this.jsHandle = jsHandle;
        }

        public bool HasProperty(string propertyName) => HasPropertyOnObject(this, propertyName);

        public JSPropertyType GetTypeOfProperty(string propertyName) => (JSPropertyType)GetTypeOfPropertyOnObject(this, propertyName);

        public ImmutableArray<string> GetPropertyNames() => GetKeysOnObject(this);

        public bool GetPropertyAsBoolean(string propertyName) => GetPropertyAsBooleanOnObject(this, propertyName);

        public int GetPropertyAsInt32(string propertyName) => GetPropertyAsInt32OnObject(this, propertyName);

        public double GetPropertyAsDouble(string propertyName) => GetPropertyAsDoubleOnObject(this, propertyName);

        public string? GetPropertyAsString(string propertyName) => GetPropertyAsStringOnObject(this, propertyName);

        public JSObject? GetPropertyAsJSObject(string propertyName) => GetPropertyAsJSObjectOnObject(this, propertyName);

        public byte[]? GetPropertyAsByteArray(string propertyName) => GetPropertyAsByteArrayOnObject(this, propertyName);

        public void SetProperty(string propertyName, bool value) => SetPropertyOnObject(this, propertyName, value);

        public void SetProperty(string propertyName, int value) => SetPropertyOnObject(this, propertyName, value);

        public void SetProperty(string propertyName, double value) => SetPropertyOnObject(this, propertyName, value);

        public void SetProperty(string propertyName, string? value) => SetPropertyOnObject(this, propertyName, value);

        public void SetProperty(string propertyName, JSObject? value) => SetPropertyOnObject(this, propertyName, value);

        public void SetProperty(string propertyName, byte[]? value) => SetPropertyOnObject(this, propertyName, value);

        public void DeleteProperty(string propertyName) => DeletePropertyOnObject(this, propertyName);

        public override string ToString()
            => $"JSObject[{jsHandle}]{(disposedValue ? " (DISPOSED)" : "")}";

        public override bool Equals(object? obj) => Equals(obj as JSObject);

        public bool Equals(JSObject? other) => other is not null && jsHandle.Equals(other.jsHandle);

        public override int GetHashCode() => HashCode.Combine(jsHandle);

        public static bool operator ==(JSObject? left, JSObject? right) => EqualityComparer<JSObject>.Default.Equals(left, right);

        public static bool operator !=(JSObject? left, JSObject? right) => !(left == right);

        #region IDisposable

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                Native.ReleaseJSObject(jsHandle);
                disposedValue = true;
            }
        }

        ~JSObject()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
