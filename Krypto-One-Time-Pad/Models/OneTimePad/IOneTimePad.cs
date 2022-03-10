using System;

namespace Krypto_One_Time_Pad.Models.OneTimePad;

public interface IOneTimePad
{
	byte[] Encrypt(byte[] plainText ,byte[] key);
	byte[] Decrypt(byte[] cipher ,byte[] key);
	byte[] GenerateKey(int lenght);
}