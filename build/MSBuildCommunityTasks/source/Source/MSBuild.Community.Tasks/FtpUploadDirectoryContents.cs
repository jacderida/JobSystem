﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;
using System.Diagnostics;

namespace MSBuild.Community.Tasks
{
	public class FtpUploadDirectoryContents : Task
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:FtpUpload"/> class.
		/// </summary>
		public FtpUploadDirectoryContents()
		{
			_username = "anonymous";
			_password = string.Empty;
		}

		/// <summary>
		/// Gets or sets the local file to upload.
		/// </summary>
		/// <value>The local file.</value>
		[Required]
		public string DirectoryPath
		{
			get;
			set;
		}

		private string _remoteUri;

		/// <summary>
		/// Gets or sets the remote URI to upload.
		/// </summary>
		/// <value>The remote URI.</value>
		[Required]
		public string RemoteUri
		{
			get { return _remoteUri; }
			set { _remoteUri = value; }
		}

		private string _username;

		/// <summary>
		/// Gets or sets the username.
		/// </summary>
		/// <value>The username.</value>
		public string Username
		{
			get { return _username; }
			set { _username = value; }
		}

		private string _password;

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		private bool _usePassive;

		/// <summary>
		/// Gets or sets the behavior of a client application's data transfer process.
		/// </summary>
		/// <value><c>true</c> if [use passive]; otherwise, <c>false</c>.</value>
		public bool UsePassive
		{
			get { return _usePassive; }
			set { _usePassive = value; }
		}

		/// <summary>
		/// When overridden in a derived class, executes the task.
		/// </summary>
		/// <returns>
		/// true if the task successfully executed; otherwise, false.
		/// </returns>
		public override bool Execute()
		{
			if (!Directory.Exists(DirectoryPath))
			{
				Log.LogError("The directory {0} was not found", DirectoryPath);
				return false;
			}
			foreach (var localFilePath in Directory.GetFiles(DirectoryPath))
			{
				var localFile = new FileInfo(localFilePath);
				Log.LogMessage(Properties.Resources.FtpUploading, localFile.FullName, _remoteUri);

				Uri ftpUri;
				if (!Uri.TryCreate(_remoteUri, UriKind.Absolute, out ftpUri))
				{
					Log.LogError(Properties.Resources.FtpUriInvalid, _remoteUri);
					return false;
				}

				FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUri);
				request.Method = WebRequestMethods.Ftp.UploadFile;
				request.UseBinary = true;
				request.ContentLength = localFile.Length;
				request.UsePassive = _usePassive;

				if (!string.IsNullOrEmpty(_username))
					request.Credentials = new NetworkCredential(_username, _password);

				const int bufferLength = 2048;
				byte[] buffer = new byte[bufferLength];
				int readBytes = 0;
				long totalBytes = localFile.Length;
				long progressUpdated = 0;
				long wroteBytes = 0;

				try
				{
					Stopwatch watch = Stopwatch.StartNew();
					using (Stream fileStream = localFile.OpenRead(),
											requestStream = request.GetRequestStream())
					{
						do
						{
							readBytes = fileStream.Read(buffer, 0, bufferLength);
							requestStream.Write(buffer, 0, readBytes);
							wroteBytes += readBytes;

							// log progress every 5 seconds
							if (watch.ElapsedMilliseconds - progressUpdated > 5000)
							{
								progressUpdated = watch.ElapsedMilliseconds;
								Log.LogMessage(MessageImportance.Low,
										Properties.Resources.FtpPercentComplete,
										wroteBytes * 100 / totalBytes,
										FormatBytesPerSecond(wroteBytes, watch.Elapsed.TotalSeconds, 1));
							}
						}
						while (readBytes != 0);
					}
					watch.Stop();

					using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
					{
						Log.LogMessage(MessageImportance.Low, Properties.Resources.FtpUploadComplete, response.StatusDescription);
						Log.LogMessage(Properties.Resources.FtpTransfered,
								FormatByte(totalBytes, 1),
								FormatBytesPerSecond(totalBytes, watch.Elapsed.TotalSeconds, 1),
								watch.Elapsed.ToString());
						response.Close();
					}
				}
				catch (Exception ex)
				{
					Log.LogErrorFromException(ex);
					return false;
				}
			}			

			return true;
		}

		private string FormatByte(long bytes, int rounding)
		{
			if (bytes >= System.Math.Pow(2, 80))
				return System.Math.Round(bytes / System.Math.Pow(2, 70), rounding).ToString() + " YB"; //yettabyte
			if (bytes >= System.Math.Pow(2, 70))
				return System.Math.Round(bytes / System.Math.Pow(2, 70), rounding).ToString() + " ZB"; //zettabyte
			if (bytes >= System.Math.Pow(2, 60))
				return System.Math.Round(bytes / System.Math.Pow(2, 60), rounding).ToString() + " EB"; //exabyte
			if (bytes >= System.Math.Pow(2, 50))
				return System.Math.Round(bytes / System.Math.Pow(2, 50), rounding).ToString() + " PB"; //petabyte
			if (bytes >= System.Math.Pow(2, 40))
				return System.Math.Round(bytes / System.Math.Pow(2, 40), rounding).ToString() + " TB"; //terabyte
			if (bytes >= System.Math.Pow(2, 30))
				return System.Math.Round(bytes / System.Math.Pow(2, 30), rounding).ToString() + " GB"; //gigabyte
			if (bytes >= System.Math.Pow(2, 20))
				return System.Math.Round(bytes / System.Math.Pow(2, 20), rounding).ToString() + " MB"; //megabyte
			if (bytes >= System.Math.Pow(2, 10))
				return System.Math.Round(bytes / System.Math.Pow(2, 10), rounding).ToString() + " KB"; //kilobyte

			return bytes.ToString() + " Bytes"; //byte
		}

		private string FormatBytesPerSecond(long bytes, double secounds, int rounding)
		{
			double bytesPerSecounds = bytes / secounds;

			if (bytesPerSecounds >= System.Math.Pow(2, 80))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 70), rounding).ToString() + " YB/s"; //yettabyte
			if (bytesPerSecounds >= System.Math.Pow(2, 70))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 70), rounding).ToString() + " ZB/s"; //zettabyte
			if (bytesPerSecounds >= System.Math.Pow(2, 60))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 60), rounding).ToString() + " EB/s"; //exabyte
			if (bytesPerSecounds >= System.Math.Pow(2, 50))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 50), rounding).ToString() + " PB/s"; //petabyte
			if (bytesPerSecounds >= System.Math.Pow(2, 40))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 40), rounding).ToString() + " TB/s"; //terabyte
			if (bytesPerSecounds >= System.Math.Pow(2, 30))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 30), rounding).ToString() + " GB/s"; //gigabyte
			if (bytesPerSecounds >= System.Math.Pow(2, 20))
				return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 20), rounding).ToString() + " MB/s"; //megabyte

			return System.Math.Round(bytesPerSecounds / System.Math.Pow(2, 10), rounding).ToString() + " KB/s"; //kilobyte
		}
	}
}
