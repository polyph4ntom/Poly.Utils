namespace Poly.Common
{
    [System.Serializable]
    public struct FPolySerializableKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public FPolySerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
