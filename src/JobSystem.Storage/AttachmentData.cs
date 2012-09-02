using System;
using System.IO;

namespace JobSystem.Storage
{
	public class AttachmentData
	{
		public Stream Content { get; set; }
		public string ContentType { get; set; }
		public string Filename { get; set; }
		public Guid Id { get; set; }
	}
}