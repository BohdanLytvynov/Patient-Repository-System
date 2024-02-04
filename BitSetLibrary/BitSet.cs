namespace BitSetLibrary
{
    public static class BitSet
    {
        /// <summary>
        /// Checks wether bit under numberRank is 1
        /// </summary>
        /// <param name="bitSet">Bitset for check</param>
        /// <param name="numberRank">Index of the number rank from 0 to 32 from right to left</param>
        /// <returns>value greater then 0 if bit is not 0</returns>
        public static bool IsBitOne(int bitSet, int numberRank)
        {
            if(bitSet == 0)
                return false;

            return ( bitSet & (1 << numberRank) ) != 0;
        }

        /// <summary>
        /// Sets the bit under the numberRank to 1
        /// </summary>
        /// <param name="bitSet">Bitset for modification</param>
        /// <param name="numberRank">Index of the number rank from 0 to 32 from right to left</param>
        public static void SetBit(ref int bitSet, int numberRank)
        {
            bitSet |= (1 << numberRank);
        }

        /// <summary>
        /// Inverts the bit under the number rank
        /// </summary>
        /// <param name="bitSet">Bitset for the invertion</param>
        /// <param name="numberRank">bit under the number rank that have to be inverted</param>
        public static void InvertBit(ref int bitSet, int numberRank)
        { 
            bitSet ^= (1 << numberRank);
        }
        /// <summary>
        /// Sets the bit under the numberRank to 1
        /// </summary>
        /// <param name="bitSet">Bitset for check</param>
        /// <param name="numberRank">Index of the number rank from 0 to 32 from right to left</param>
        /// <returns>The new bitset, with modified bit under the numberRank</returns>
        public static int SetBit(int bitSet, int numberRank)
        {
            return bitSet | (1 << numberRank);
        }

        /// <summary>
        /// Inverts the bit under the number rank
        /// </summary>
        /// <param name="bitSet">Bitset for the invertion</param>
        /// <param name="numberRank">bit under the number rank that have to be inverted</param>
        /// <returns>The new bitset, with inverted bit under the numberRank</returns>
        public static int InvertBit(int bitSet, int numberRank)
        {
            return bitSet ^ (1 << numberRank);
        }
    }
}