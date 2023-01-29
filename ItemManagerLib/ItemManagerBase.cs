namespace ItemManagerLib
{
    public class ItemManagerBase<TItem, TKey>
    {
        #region Fields

        Dictionary<TKey, TItem> m_storage;

        #endregion

        #region Ctor

        public ItemManagerBase()
        {
            m_storage = new Dictionary<TKey, TItem>();
        }

        #endregion

        #region Methods

        public TItem GetItem(TKey key)
        {
            TItem item;

            m_storage.TryGetValue(key, out item);

            return item;
        }

        public void AddItem(TItem item, TKey key)
        {
            if (m_storage.ContainsKey(key))
            {
                m_storage[key] = item;
            }
            else
            {
                m_storage.Add(key, item);
            }
        }

        public void RemoveItem(TKey key)
        {
            if (m_storage.ContainsKey(key))
            {
                m_storage?.Remove(key);
            }            
        }

        public IEnumerable<TItem> GetallItems()
        { 
            return m_storage.Values;
        }

        public IEnumerable<TKey> GetallKeys()
        {
            return m_storage.Keys;
        }

        #endregion
    }
}