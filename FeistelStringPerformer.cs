namespace WPF_FeistelWith_long_key
{
    public class FeistelStringPerformer
    {
        public enum ParserMode { left, right, all };

        private UshortNodeFeistel[] pairs;

        public byte Size { get { return (byte)((pairs == null) ? 0 : pairs.Length); } }

        private FeistelStringPerformer(ushort[] data, ParserMode pm)
        {
            //if (data == null)return;
            if (data.Length % 2 == 1) throw new System.ArgumentException("not divide by 2");
            pairs =new UshortNodeFeistel[(pm== ParserMode.all)?data.Length/2:data.Length];
            switch (pm)
            {
                case (ParserMode.left):
                    {
                        for (uint index = 0; index < pairs.Length; index++)
                            pairs[index] = new UshortNodeFeistel(data[index], '\u263a');// '\u263a' is :-)
                        return;
                    }
                case (ParserMode.right):
                    {
                        for (byte index = 0; index < pairs.Length; index++)
                            pairs[index] = new UshortNodeFeistel('\u263a', data[index]);// '\u263a' is :-)
                        return;
                    }
                case (ParserMode.all):
                    {
                        for (byte index = 0; index < pairs.Length; index++)
                            pairs[index] = new UshortNodeFeistel(data[index * 2], data[index * 2 + 1]);
                        return;
                    }
            }
        }


        public static FeistelStringPerformer UploadString(string message, ParserMode pm) { return new FeistelStringPerformer(BicycleConverter.StringToUshortArray(message), pm); }

        /// <summary>
        /// subcommand for native user (human)
        /// </summary>
        /// <returns>rezult of operations</returns>
        public string ReadStringFromNumbers(ParserMode pm)
        {
            ushort[] numbers = null;
            switch (pm)
            {
                case (ParserMode.left):
                    {
                        numbers = new ushort[pairs.Length];
                        for (uint index = 0; index < pairs.Length; index++)
                            numbers[index] = pairs[index].left_part;
                        break;
                    }
                case (ParserMode.right):
                    {
                        numbers = new ushort[pairs.Length];
                        for (uint index = 0; index < pairs.Length; index++)
                            numbers[index] = pairs[index].right_part;
                        break;
                    }
                case (ParserMode.all):
                    {
                        numbers = new ushort[pairs.Length * 2];
                        for (uint index = 0; index < pairs.Length; index++)
                        {
                            numbers[index * 2] = pairs[index].left_part;
                            numbers[index * 2 + 1] = pairs[index].right_part;
                        }
                        break;
                    }
            }
            return BicycleConverter.UshortArrayToString(numbers);
        }

        /// <summary>
        /// </summary>
        /// <returns>left parts</returns>
        public override string ToString() { return ReadStringFromNumbers(ParserMode.all); }

        /// <summary>
        /// </summary>
        /// <returns>chinese signs</returns>
        public string Encoder(byte[] deep)
        {
            for (byte index = 0; index < pairs.Length; index++)
                pairs[index].Encrypt(deep[index]);
            return ReadStringFromNumbers(ParserMode.all);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>plaintext of secret message</returns>
        public string Decoder(byte[] deep)
        {
            for (byte index = 0; index < pairs.Length; index++)
                pairs[index].Decrypt(deep[index]);
            return ReadStringFromNumbers(ParserMode.left);
        }
    }
}
