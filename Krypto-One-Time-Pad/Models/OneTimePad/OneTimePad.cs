using System;

namespace Krypto_One_Time_Pad.Models.OneTimePad;

class OneTimePad : IOneTimePad
{
	public byte[] Encrypt(byte[] plainText, byte[] key)
	{
		return PerformXOR(plainText, key);
	}

	public byte[] Decrypt(byte[] cipher, byte[] key)
	{
		return PerformXOR(cipher, key);
	}

	public byte[] GenerateKey(int lenght)
	{
		byte[] generatedKey = new byte[lenght];
		Random random = new Random();

		random.NextBytes(generatedKey);
		return generatedKey;
	}


	private byte[] PerformXOR(byte[] text, byte[] key)
    {
		if (text.Length != key.Length)
			throw new OneTimePadKeyException("Tekst i klucz róŸni¹ siê d³ugoœci¹");

		byte[] finalText = new byte[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            finalText[i] = (byte)(text[i] ^ key[i]);
        }

		return finalText;
    }
}