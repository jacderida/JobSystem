using System.IO;

namespace JobSystem.DataModel.Storage
{
	public class AttachmentData
	{
		public Stream Content { get; set; }
		public string ContentType { get; set; }
		public string Filename { get; set; }
		public System.Guid Id { get; set; }
	}
}