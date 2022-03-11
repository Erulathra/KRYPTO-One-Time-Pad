using System;

namespace Krypto_One_Time_Pad.Models.OneTimePad;

class OneTimePad : IOneTimePad
{
	public byte[] Encrypt(byte[] plainText, byte[] key)
	{
		throw new NotImplementedException();
	}

	public byte[] Decrypt(byte[] cipher, byte[] key)
	{
		throw new NotImplementedException();
	}

	public byte[] GenerateKey(int lenght)
	{
		throw new NotImplementedException();
	}
}