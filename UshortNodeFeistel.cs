// https://habr.com/ru/post/140404/
using System;

namespace WPF_FeistelWith_long_key
{
    public struct UshortNodeFeistel
    {
        /// <summary>when = 0 , end of crypto process</summary>
        byte undone_rounds;
        /// <summary>times of call Encrypt</summary>
        static byte limit_rounds = 0;

        /// <summary>L,R data parts</summary>
        public ushort left_part { get; private set; }
        public ushort right_part { get; private set; }

        /// <summary>Function must be volatile by left_part and by undone_rounds</summary>
        /// <remarks>must be very difficult!</remarks>
        /// <returns>key for xor</returns>
        public ushort DifficulCryptoFunction(byte current_round) { return (ushort)((3 * left_part + 8 + 4 * current_round - 3) % ushort.MaxValue); }

        public UshortNodeFeistel(ushort start_left_part, ushort start_right_part)//,//byte deep=8*/)
        {
            this = new UshortNodeFeistel();
            undone_rounds = 0;// = limit_rounds;
            left_part = start_left_part;
            right_part = start_right_part;
        }

        /// <summary>function in functional style</summary>
        /// <remarks>temp exist because dont allov void f in ternar operator</remarks>
        /// <param name="encrypt">hide or show text</param>
        private void Round(bool encrypt) { int temp = (encrypt) ? EncoderRound() : DecoderRound(); }

        /// <summary>
        /// step for hide text
        /// </summary>
        /// <returns>0 (always), for use in ternar operator</returns>
        private int EncoderRound()
        {
            ushort rezult_xor = (ushort)(right_part ^ DifficulCryptoFunction((byte)(limit_rounds - undone_rounds + 1)));
            if (undone_rounds == 1)// else do r-l replase
                right_part = rezult_xor;
            else
            {
                right_part = left_part;
                left_part = rezult_xor;
            }
            undone_rounds--;
            return 0;
        }

        /// <summary>
        /// step for show text
        /// </summary>
        /// <returns>0 (always), for use in ternar operator</returns>
        private int DecoderRound()
        {
            ushort rezult_xor = (ushort)(right_part ^ DifficulCryptoFunction(undone_rounds));
            if (undone_rounds == 1)right_part = rezult_xor;// else do r-l replase
            else
            {
                right_part = left_part;
                left_part = rezult_xor;
            }
            undone_rounds--;
            return 0;
        }

        /// <summary>
        /// hide a text
        /// </summary>
        /// <param name="deep">count of rounds for hide a text</param>
        public void Encrypt(byte deep)
        {
            undone_rounds =limit_rounds= deep;
            while (undone_rounds > 0) Round(true);
        }

        /// <summary>
        /// show hidden message
        /// </summary>
        public void Decrypt(byte deep)
        {
            undone_rounds =limit_rounds= deep;
            while (undone_rounds > 0) Round(false);
        }
    }
}
