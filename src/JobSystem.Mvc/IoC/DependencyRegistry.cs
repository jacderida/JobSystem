using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Autofac;
using JobSystem.BusinessLogic.IoC;
using JobSystem.DataAccess.NHibernate.Repositories;
using JobSystem.DataModel;
using JobSystem.DataModel.Storage;
using JobSystem.Framework.Configuration;
using JobSystem.Framework.Security;
using JobSystem.Mvc.Configuration;
using JobSystem.Mvc.Core;
using JobSystem.Framework.Messaging;

namespace JobSystem.Mvc.IoC
{
	public static class DependencyRegistry
	{
		public static void RegisterAll(ContainerBuilder builder)
		{
			builder.Register(c => new HttpContextWrapper(HttpContext.Current))
				.As<HttpContextBase>();
			builder.RegisterType<NullQueueDispatcher>().As<IQueueDispatcher<IMessage>>();
			builder.RegisterType<WebUserContext>().As<IUserContext>();
			builder.RegisterType<CryptographicService>().As<ICryptographicService>();
			builder.RegisterModule(new ServiceModule());
			builder.RegisterAssemblyTypes(typeof(UserAccountRepository).Assembly)
				.Where(t => t.Name.EndsWith("Repository"))
				.AsImplementedInterfaces().SingleInstance();
			builder.RegisterType<FormsAuthenticationService>().As<IFormsAuthenticationService>();
			builder.RegisterType<HostRequestConfigDomainProvider>().As<IConfigDomainProvider>();
			builder.RegisterType<FileSystemAttachmentStorage>().As<IJobAttachmentDataRepository>();

			if (Config.UseRemoteConfig())
				builder.RegisterType<RemoteTenantConfig>().As<ITenantConfig>().InstancePerLifetimeScope();
			else
				builder.RegisterType<LocalTenantConfig>().As<ITenantConfig>().InstancePerLifetimeScope();
		}
	}

	public class FileSystemAttachmentStorage : IJobAttachmentDataRepository
	{
		private string _attachmentLocation = @"c:\temp\jobattachments";

		public AttachmentData GetById(Guid id)
		{
			var metaFilePath = GetAttachmentPath(id.ToString(), "metadata");
			var doc = XDocument.Load(metaFilePath);
			var contentType = doc.Descendants("value")
				.Where(x => x.Attribute("name") != null && x.Attribute("name").Value.Equals("content-type"))
				.Select(x => x.Attribute("value").Value)
				.Single();
			var filename = doc.Descendants("value")
				.Where(x => x.Attribute("name") != null && x.Attribute("name").Value.Equals("filename"))
				.Select(x => x.Attribute("value").Value)
				.Single();
			var attachmentPath = GetAttachmentPath(id.ToString(), "attachment");
			return new AttachmentData
			{
				Id = id,
				Filename = filename,
				ContentType = contentType,
				Content = File.OpenRead(attachmentPath)
			};
		}

		public void Put(AttachmentData attachmentData)
		{
			if (!Directory.Exists(_attachmentLocation))
				Directory.CreateDirectory(_attachmentLocation);
			PutAttachment(attachmentData);
			PutMetadata(attachmentData);
		}

		private void PutAttachment(AttachmentData attachmentData)
		{
			var path = GetAttachmentPath(attachmentData.Id.ToString(), "attachment");
			using (var outputStream = File.OpenWrite(path))
			{
				using (var inputStream = attachmentData.Content)
				{
					var bufferSize = 1024;
					var buffer = new byte[bufferSize];
					var byteCount = 0;
					while ((byteCount = inputStream.Read(buffer, 0, bufferSize)) > 0)
						outputStream.Write(buffer, 0, byteCount);
				}
			}
		}

		private string GetAttachmentPath(string filePath, string extension)
		{
			return Path.Combine(_attachmentLocation, String.Format("{0}.{1}", filePath, extension));
		}

		private void PutMetadata(AttachmentData attachmentData)
		{
			var doc = new XmlDocument();
			var root = doc.CreateElement("attachment-meta-data");
			doc.AppendChild(root);
			root.AppendChild(CreateValueNode(doc, "content-type", attachmentData.ContentType));
			root.AppendChild(CreateValueNode(doc, "filename", attachmentData.Filename));
			var path = GetAttachmentPath(attachmentData.Id.ToString(), "metadata");
			doc.Save(path);
		}

		private XmlElement CreateValueNode(XmlDocument doc, string name, string value)
		{
			var valueElement = doc.CreateElement("value");
			var nameAttr = doc.CreateAttribute("name");
			nameAttr.Value = name;
			var valueAttr = doc.CreateAttribute("value");
			valueAttr.Value = value;
			valueElement.Attributes.Append(nameAttr);
			valueElement.Attributes.Append(valueAttr);
			return valueElement;
		}
	}
}