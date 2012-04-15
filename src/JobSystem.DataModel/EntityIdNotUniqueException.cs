using System;

namespace JobSystem.DataModel
{
	public class EntityIdNotUniqueException : Exception
	{
		public EntityIdNotUniqueException(string message) : base(message)
		{
		}
	}
}