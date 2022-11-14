using System;

namespace OnlineGameStore.Common.Converters
{
    public static class GuidIntConverter
    {
        public static int? GuidToInt(Guid value)
        {
            if (value == null)
            {
                return null;
            }
            byte[] bytes = value.ToByteArray();
            return BitConverter.ToInt32(bytes, 0);
        }

        public static Guid? IntToGuid(int? value)
        {
            if (value == null)
            {
                return null;
            }

            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value.Value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
}
