namespace Orcus.Core.Cache.CacheKeys
{
    public class CacheKey
    {
        private readonly string _format = "{0}-{1}-{2}";
        private readonly string _generatedKey;

        public CacheKey(string entity, int entityId, string userName)
        {
            _generatedKey = string.Format(_format, entity.ToString(), entityId.ToString(), userName);
        }

        public static CacheKey New(string entity, int entityId, string userName)
        {
            return new CacheKey(entity, entityId, userName);
        }

        public override string ToString()
        {
            return _generatedKey;
        }
    }
}
