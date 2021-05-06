using Microsoft.ML.Data;

namespace KaderService.ML
{
    public class ProductEntry
    {
        private const ulong _keyType = 10000;

        [KeyType(_keyType)]
        public uint UserNumber { get; set; }

        [KeyType(_keyType)]
        public uint RelatedPostId { get; set; }
    }
}