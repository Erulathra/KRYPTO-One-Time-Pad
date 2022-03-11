using System;
using JetBrains.Annotations;

namespace Krypto_One_Time_Pad.Models.OneTimePad;

public interface IOneTimePad
{
	byte[] Encrypt(byte[] plainText ,byte[] key);
	byte[] Decrypt(byte[] cipher ,byte[] key);
	byte[] GenerateKey(int lenght);
}

public abstract class OneTimePadException : Exception
{
	protected OneTimePadException([CanBeNull] string? message) : base(message)
	{ }

	protected OneTimePadException([CanBeNull] string? message, [CanBeNull] Exception? innerException) : base(message, innerException)
	{ }
}

public class OneTimePadKeyException : OneTimePadException
{
	public OneTimePadKeyException([CanBeNull] string? message) : base(message)
	{ }

	public OneTimePadKeyException([CanBeNull] string? message, [CanBeNull] Exception? innerException) : base(message, innerException)
	{ }
}