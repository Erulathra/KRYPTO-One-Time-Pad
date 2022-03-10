using System;
using System.IO;

namespace Krypto_One_Time_Pad.Models.Daos;

public class FileDao : IDao
{
	private readonly string path;

	public FileDao(string path)
	{
		this.path = path;
	}

	public byte[] Read()
	{
		try
		{
			return File.ReadAllBytes(path);
		}
		catch (IOException e)
		{
			throw new WritingFileDaoException();
		}
	}

	public void Write(byte[] bytes)
	{
		try
		{
			
			File.WriteAllBytes(path, bytes);
		}
		catch (IOException e)
		{
			throw new ReadingFileDaoException();
		}
	}
}

public class WritingFileDaoException : WritingDaoException
{ }

public class ReadingFileDaoException : ReadingDaoException
{ }