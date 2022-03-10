using System;

namespace Krypto_One_Time_Pad.Models.Daos;

public interface IDao
{
	byte[] Read();
	void Write(byte[] bytes);
}

public class DaoException : Exception
{ }

public class WritingDaoException : DaoException
{ }

public class ReadingDaoException : DaoException
{ }